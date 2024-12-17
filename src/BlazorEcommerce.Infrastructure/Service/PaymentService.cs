using BlazorEcommerce.Application.Contracts.Identity;
using BlazorEcommerce.Application.Contracts.Payment;
using BlazorEcommerce.Application.Model;
using BlazorEcommerce.Shared.Cart;
using BlazorEcommerce.Shared.Constant;
using BlazorEcommerce.Shared.Response.Abstract;
using BlazorEcommerce.Shared.Response.Concrete;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.Checkout;

namespace BlazorEcommerce.Infrastructure.Services.PaymentService
{
    public class PaymentService : IPaymentService
    {
        private readonly ICurrentUser _currentUser;
        private readonly StripeConfig _stripeConfig;
        private readonly AppConfig _appConfig;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly string secret = string.Empty;

        public PaymentService(ICurrentUser currentUser, IOptions<StripeConfig> stripeConfig, IOptions<AppConfig> appConfig, IHttpContextAccessor httpContextAccessor)
        {
            _currentUser = currentUser;
            _stripeConfig = stripeConfig.Value;
            _appConfig = appConfig.Value;
            _httpContextAccessor = httpContextAccessor;
            StripeConfiguration.ApiKey = _stripeConfig.ApiKey;
            secret = _stripeConfig.Secret;
        }

        public Task<IResponse> CreateCheckoutSession(List<CartProductResponse> products)
        {
            var lineItems = new List<SessionLineItemOptions>();

            products.ForEach(product => lineItems.Add(new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions
                {
                    UnitAmountDecimal = product.Price * 100,
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = product.Title,
                        // if there is no image url, then create one based on the image url endpoint
                        Images = [$"{(string.IsNullOrEmpty(product.ImageUrl) ? GetImageUrl(product.ProductId) : product.ImageUrl)}"]
                    }
                },
                Quantity = product.Quantity
            }));

            var options = new SessionCreateOptions
            {
                ClientReferenceId = _currentUser.UserId,
                CustomerEmail = _currentUser.UserEmail,
                ShippingAddressCollection =
                    new SessionShippingAddressCollectionOptions
                    {
                        AllowedCountries = new List<string> { "US" }
                    },
                PaymentMethodTypes = new List<string>
                    {
                        "card"
                    },
                LineItems = lineItems,
                Mode = "payment",
                SuccessUrl = $"{_appConfig.ClientUrl}order-success",
                CancelUrl = $"{_appConfig.ClientUrl}cart"
            };

            var service = new SessionService();
            Session session = service.Create(options);

            return Task.FromResult<IResponse>(new DataResponse<string>(session.Url, HttpStatusCodes.Accepted));
        }

        public async Task<IResponse> FulfillOrder(HttpRequest request)
        {
            var json = await new StreamReader(request.Body).ReadToEndAsync();
            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                        json,
                        request.Headers["Stripe-Signature"],
                        secret
                    );
                if (stripeEvent.Type == EventTypes.CheckoutSessionCompleted)
                {
                    var session = stripeEvent.Data.Object as Session;
                    var userId = session?.ClientReferenceId;
                    return new DataResponse<string?>(userId);
                }
                if (stripeEvent.Type == EventTypes.ChargeSucceeded)
                {
                    // todo: handle this event in the future?
                }

                return new DataResponse<string?>(null, HttpStatusCodes.InternalServerError, stripeEvent.StripeResponse.Content, false);
            }
            catch (StripeException e)
            {
                return new DataResponse<string?>(null, HttpStatusCodes.InternalServerError, e.Message, false);
            }
        }

        /// <summary>
        /// given a product id, return the image url relative to the absolute web app base url
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        private string GetImageUrl(int productId)
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                throw new InvalidOperationException("Unable to determine base URL.");
            }

            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            var imageUrl = $"{baseUrl}/api/image/{productId}";
            return imageUrl;
        }
    }
}
