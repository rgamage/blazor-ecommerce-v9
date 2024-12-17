using BlazorEcommerce.Application.Features.Image.Query.GetImageByProduct;
using BlazorEcommerce.Application.Features.Order.Query.GetOrder;
using BlazorEcommerce.Shared.Constant;
using BlazorEcommerce.Shared.Response.Concrete;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BlazorEcommerce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ImageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductImage(int productId)
        {
            var response = await _mediator.Send(new GetImageByProductQueryRequest(productId));
            if (!response.Success)
            {
                return StatusCode(response.StatusCode);
            }

            if (response is not DataResponse<Stream> imageResponse)
            {
                return StatusCode(500, $"GetProductImage - Invalid response type for product Id {productId}");
            }
            if (imageResponse.Data == null)
            {
                return StatusCode(404, $"GetProductImage - Image not found for product Id {productId}");
            }

            return File(imageResponse.Data, Constants.ImageFormat, $"product{productId}.png");
        }
    }
}
