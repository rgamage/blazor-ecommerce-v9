using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorEcommerce.Client.Services
{
    public class RefreshTokenService
	{
		private readonly AuthenticationStateProvider _authProvider;
		private readonly IAuthService _authService;

		public RefreshTokenService(AuthenticationStateProvider authProvider, IAuthService authService)
		{
			_authProvider = authProvider;
			_authService = authService;
		}

		public async Task<string> TryRefreshToken()
		{
			var authState = await _authProvider.GetAuthenticationStateAsync();
			ClaimsPrincipal user = authState.User;
			var exp = user?.FindFirst(c => c.Type.Equals("exp"))?.Value;
            if (string.IsNullOrEmpty(exp))
            {
				// no user or no exp time found for token
                return string.Empty;
            }

            var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));

			var timeUTC = DateTime.UtcNow;

			var diff = expTime - timeUTC;
			if (diff.TotalMinutes <= 2)
				return await _authService.RefreshToken();

			return string.Empty;
		}
	}
}
