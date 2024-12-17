using BlazorEcommerce.Client.CustomExceptions;
using Microsoft.AspNetCore.Components;
using System.Net;
using System.Net.Http.Headers;
using Toolbelt.Blazor;

namespace BlazorEcommerce.Client.Services;

public class HttpInterceptorService
{
    private readonly HttpClientInterceptor _interceptor;
    private readonly NavigationManager _navManager;
    private readonly RefreshTokenService _refreshTokenService;
    private readonly IAuthService _authService;

    public HttpInterceptorService(HttpClientInterceptor interceptor, NavigationManager navManager, RefreshTokenService refreshTokenService, IAuthService authService)
    {
        _interceptor = interceptor;
        _navManager = navManager;
        _refreshTokenService = refreshTokenService;
        _authService = authService;
    }

    public void RegisterEvent()
    {
        _interceptor.AfterSend += InterceptResponse;
        _interceptor.BeforeSendAsync += InterceptBeforeHttpAsync;
    }

    public async Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e)
    {
        var absPath = e.Request.RequestUri?.AbsolutePath ?? string.Empty;

        var isUserAuthenticated = await _authService.IsUserAuthenticated();

        if (!absPath.Contains("auth") && isUserAuthenticated)
        {
            var token = await _refreshTokenService.TryRefreshToken();

            if (!string.IsNullOrEmpty(token))
            {
                e.Request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token.Replace("\"", ""));
            }
        }
    }

    private void InterceptResponse(object? sender, HttpClientInterceptorEventArgs e)
    {
        string message;
        if (e.Response == null)
        {
            var requestUrl = Uri.EscapeDataString(e.Request?.RequestUri?.ToString() ?? string.Empty);
            _navManager.NavigateTo($"/400?requestUrl={requestUrl}");
            return;
        }
        if (!e.Response.IsSuccessStatusCode)
        {
            var statusCode = e.Response.StatusCode;

            switch (statusCode)
            {
                case HttpStatusCode.NotFound:
                    _navManager.NavigateTo("/404");
                    message = "The requested resource was not found.";
                    break;
                case HttpStatusCode.Unauthorized:
                    //var errorMessage = "Unauthorized. Please enter valid credentials";
                    //_navManager.NavigateTo($"/login");
                    //message = "User is not authorized";
                    //break;
                    return;
                case HttpStatusCode.BadRequest:
                    var requestUrl = Uri.EscapeDataString(e.Request?.RequestUri?.ToString() ?? string.Empty);
                    _navManager.NavigateTo($"/400?requestUrl={requestUrl}");
                    return;
                default:
                    _navManager.NavigateTo("/500");
                    message = "Something went wrong, please contact Administrator";
                    break;
            }

            throw new HttpResponseException(message);
        }
    }

    public void DisposeEvent()
    {
        _interceptor.AfterSend -= InterceptResponse;
        _interceptor.BeforeSendAsync -= InterceptBeforeHttpAsync;
    }
}