using BlazorEcommerce.Application.Contracts.Identity;
using BlazorEcommerce.Shared.Cart;
using BlazorEcommerce.Shared.Order;
using System.Net;

namespace BlazorEcommerce.Application.Features.Order.Command.PlaceOrder;

public record PlaceOrderCommandRequest(List<CartProductResponse> products) : IRequest<IResponse>;

public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommandRequest, IResponse>
{
    private readonly ICommandUnitOfWork<int> _command;
    private readonly IMapper _mapper;
    private readonly ICurrentUser _currentUser;
    private readonly IQueryUnitOfWork _query;

    public PlaceOrderCommandHandler(ICommandUnitOfWork<int> command, IMapper mapper, ICurrentUser currentUser, IQueryUnitOfWork query)
    {
        _command = command;
        _mapper = mapper;
        _currentUser = currentUser;
        _query = query;
    }

    public async Task<IResponse> Handle(PlaceOrderCommandRequest request, CancellationToken cancellationToken)
    {
        string userId = (_currentUser.UserId ?? request.products.FirstOrDefault()?.UserId) ?? string.Empty;

        if (string.IsNullOrEmpty(userId))
        {
            return new DataResponse<string?>(null, HttpStatusCodes.NotFound, string.Format(Messages.InvalidCredentials, "Order"), false);
        }

        decimal totalPrice = 0;
        request.products.ForEach(product => totalPrice += product.Price * product.Quantity);

        var orderItems = new List<OrderItemDto>();
        request.products.ForEach(product => orderItems.Add(new OrderItemDto
        {
            ProductId = product.ProductId,
            ProductTypeId = product.ProductTypeId,
            Quantity = product.Quantity,
            TotalPrice = product.Price * product.Quantity
        }));

        var order = new OrderDto
        {
            UserId = userId,
            OrderDate = DateTime.Now,
            TotalPrice = totalPrice,
            OrderItems = orderItems
        };

        await _command.OrderCommand.AddAsync(_mapper.Map<Domain.Entities.Order>(order));

        var cartItems = await _query.CartItemQuery.GetAllWithIncludeAsync(false, ci => ci.UserId == userId);

        _command.CartItemCommand.RemoveRange(cartItems);

        await _command.SaveAsync();

        return new DataResponse<string?>(null);
    }
}
