using BlazorEcommerce.Application.Contracts.Identity;
using BlazorEcommerce.Domain.Entities;
using BlazorEcommerce.Shared.Constant;
using Microsoft.EntityFrameworkCore;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace BlazorEcommerce.Persistence.Repositories.Queries;

public class ProductQueryRepository : QueryRepository<Product, int>, IProductQueryRepository
{
    public ProductQueryRepository(PersistenceDataContext context) : base(context)
    {
    }

    public async Task<IList<Product>> GetAllAdminProductAsync()
    {
        return await context.Products
            .Include(p => p.Images)
            .Include(p => p.Variants).ThenInclude(p => p.ProductType)
            .ToListAsync();
    }

    public async Task<Product?> GetProductByIdAsync(int id, bool isAdminRole)
    {
        if (isAdminRole)
        {
            return await context.Products
                .Include(p => p.Images)
                .Include(p => p.Variants).ThenInclude(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id);
        }
        else
        {
            return await context.Products
                .Include(p => p.Images)
                .Include(p => p.Variants).ThenInclude(p => p.ProductType)
                .FirstOrDefaultAsync(p => p.Id == id && p.IsActive);
        };
    }

    /// <summary>
    /// gets product image as a stream
    /// </summary>
    /// <param name="productId"></param>
    /// <returns></returns>
    public async Task<Stream> GetProductImage(int productId)
    {
        var stream = new MemoryStream();
        var image = await context.Images.OrderBy(x => x.Id).FirstOrDefaultAsync(x => x.ProductId == productId);
        if (image != null)
        {
            // Remove the prefix if it exists
            var base64Data = image.Data.Replace($"data:{Constants.ImageFormat};base64,", string.Empty);
            var imageData = Convert.FromBase64String(base64Data);
            await stream.WriteAsync(imageData, 0, imageData.Length);
        }
        stream.Position = 0;
        return stream;
    }
}
