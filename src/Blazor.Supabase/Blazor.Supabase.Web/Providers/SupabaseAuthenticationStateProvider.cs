using BlazorSupabase.Web.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BlazorSupabase.Web.Providers;

public class SupabaseAuthenticationStateProvider : AuthenticationStateProvider
{
	private readonly AuthService _authService;

	public SupabaseAuthenticationStateProvider(AuthService authService)
	{
		_authService = authService;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		try
		{
			var claims = await _authService.GetLoginInfoAsync();
			var claimsIdentity = claims.Count != 0
				? new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme, "name", "role")
				: new ClaimsIdentity();
			var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
			return new AuthenticationState(claimsPrincipal);
		}
		catch (Exception)
		{
			// very bad but fix issue with javascript not available
			return new AuthenticationState(new ClaimsPrincipal());
		}
	}
}
