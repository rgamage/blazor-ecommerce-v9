using BlazorEcommerce.Application.Contracts.Identity;
using BlazorEcommerce.Application.Features.Cart.Commands.AddToCart;
using BlazorEcommerce.Application.Features.Cart.Commands.RemoveItemFromCart;
using BlazorEcommerce.Application.Features.Cart.Commands.StoreCartItems;
using BlazorEcommerce.Application.Features.Cart.Commands.UpdateQuantity;
using BlazorEcommerce.Application.Features.Cart.Query.GetCartItemsCount;
using BlazorEcommerce.Application.Features.Cart.Query.GetCartProducts;
using BlazorEcommerce.Application.Features.Cart.Query.GetDbCartProducts;
using BlazorEcommerce.Shared.Cart;
using BlazorEcommerce.Shared.Constant;
using BlazorEcommerce.Shared.Response.Concrete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUser _currentUser;

        public CartController(IMediator mediator, ICurrentUser currentUser)
        {
            _mediator = mediator;
            _currentUser = currentUser;
        }

        [HttpPost("products")]
        public async Task<ActionResult<IResponse>> GetCartProducts(List<CartItemDto> cartItems)
        {
            var response = await _mediator.Send(new GetCartProductsQueryRequest(cartItems));
            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<IResponse>> StoreCartItems(List<CartItemDto> cartItems)
        {
            // first check if there are any duplicate items already in the user's cart
            var getItemsResponse = await _mediator.Send(new GetDbCartProductsQueryRequest());
            var existingItems = getItemsResponse as DataResponse<List<CartProductResponse>>;
            if (existingItems?.Data != null)
            {
                var itemsToDelete = cartItems
                    .Where(c => existingItems.Data.Exists(d => d.UserId == _currentUser.UserId && d.ProductId == c.ProductId && d.ProductTypeId == c.ProductTypeId))
                    .ToList();
                foreach(var item in itemsToDelete)
                {
                    cartItems.Remove(item);
                }
            }

            var result = await _mediator.Send(new StoreCartItemsCommandRequest(cartItems));
            if (!result.Success)
            {
                return new DataResponse<List<CartProductResponse>>([], HttpStatusCodes.NotFound);
            }
            else
            {
                return Ok(await _mediator.Send(new GetDbCartProductsQueryRequest()));
            }
           
        }

        [HttpPost("add")]
        public async Task<ActionResult<IResponse>> AddToCart(CartItemDto cartItem)
        {
            var response = await _mediator.Send(new AddToCartCommandRequest(cartItem));
            return Ok(response);
        }

        [HttpPut("update-quantity")]
        public async Task<ActionResult<IResponse>> UpdateQuantity(CartItemDto cartItem)
        {
            var response = await _mediator.Send(new UpdateQuantityCommandRequest(cartItem));
            return Ok(response);
        }

        [HttpDelete("{productId}/{productTypeId}")]
        public async Task<ActionResult<IResponse>> RemoveItemFromCart(int productId, int productTypeId)
        {
            var response = await _mediator.Send(new RemoveItemFromCartCommandRequest(productId, productTypeId));
            return Ok(response);
        }

        [HttpGet("count")]
        public async Task<ActionResult<IResponse>> GetCartItemsCount()
        {
            var response = await _mediator.Send(new GetCartItemsCountQueryRequest());
            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<IResponse>> GetDbCartProducts()
        {
            var response = await _mediator.Send(new GetDbCartProductsQueryRequest());
            return Ok(response);
        }
    }
}
