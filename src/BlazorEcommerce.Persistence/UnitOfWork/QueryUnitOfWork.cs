using BlazorEcommerce.Application.UnitOfWork;

namespace BlazorEcommerce.Persistence.UnitOfWork;

public class QueryUnitOfWork : IQueryUnitOfWork
{
    private readonly PersistenceDataContext _context;

    public QueryUnitOfWork(PersistenceDataContext context)
    {
        _context = context;
    }

    public CategoryQueryRepository? _categoryQuery;
    public AddressQueryRepository? _addressQuery;
    public CartItemQueryRepository? _cartItemQuery;
    public ImageQueryRepository? _imageQuery;
    public OrderItemQueryRepository? _orderItemQuery;
    public OrderQueryRepository? _orderQuery;
    public ProductQueryRepository? _productQuery;
    public ProductTypeQueryRepository? _productTypeQuery;
    public ProductVariantQueryRepository? _productVariantQuery;

    public ICategoryQueryRepository CategoryQuery => _categoryQuery ??= new CategoryQueryRepository(_context);

    public IAddressQueryRepository AddressQuery => _addressQuery ??= new AddressQueryRepository(_context);

    public ICartItemQueryRepository CartItemQuery => _cartItemQuery ??= new CartItemQueryRepository(_context);

    public IImageQueryRepository ImageQuery => _imageQuery ??= new ImageQueryRepository(_context);

    public IOrderItemQueryRepository OrderItemQuery => _orderItemQuery ??= new OrderItemQueryRepository(_context);

    public IOrderQueryRepository OrderQuery => _orderQuery ??= new OrderQueryRepository(_context);

    public IProductQueryRepository ProductQuery => _productQuery ??= new ProductQueryRepository(_context);

    public IProductTypeQueryRepository ProductTypeQuery => _productTypeQuery ??= new ProductTypeQueryRepository(_context);

    public IProductVariantQueryRepository ProductVariantQuery => _productVariantQuery ??= new ProductVariantQueryRepository(_context);
}
