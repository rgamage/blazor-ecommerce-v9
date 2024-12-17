using BlazorEcommerce.Application.Contracts.Identity;
using BlazorEcommerce.Shared.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlazorEcommerce.Application.Features.Image.Query.GetImageByProduct;

public record GetImageByProductQueryRequest(int ProductId) : IRequest<IResponse>;

public class GetImageByProductQueryHandler : IRequestHandler<GetImageByProductQueryRequest, IResponse>
{
    private readonly IQueryUnitOfWork _query;

    public GetImageByProductQueryHandler(IQueryUnitOfWork query)
    {
        _query = query;
    }

    public async Task<IResponse> Handle(GetImageByProductQueryRequest request, CancellationToken cancellationToken)
    {
        var stream = await _query.ProductQuery.GetProductImage(request.ProductId);

        if (stream == null)
        {
            return new DataResponse<Stream?>(null, HttpStatusCodes.NotFound, String.Format(Messages.NotExist, "Image"), false);
        }
        else
        {
            return new DataResponse<Stream>(stream, HttpStatusCodes.Accepted);
        }

    }
}