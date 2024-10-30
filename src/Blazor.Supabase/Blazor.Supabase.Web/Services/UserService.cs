using BlazorSupabase.Models.Dtos;
using BlazorSupabase.Web.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Text.Json;

namespace BlazorSupabase.Web.Services;

public class UserService
{
	private readonly AuthenticationStateProvider _customAuthenticationStateProvider;

	public UserService(AuthenticationStateProvider customAuthenticationStateProvider)
	{
		_customAuthenticationStateProvider = customAuthenticationStateProvider;
	}

	public async Task<ApplicationUser?> GetUser()
	{
		var session = await _customAuthenticationStateProvider.GetAuthenticationStateAsync();

		if (session?.User?.Identity is not null && session?.User?.Identity?.IsAuthenticated == true)
		{
			var metaData = JsonSerializer.Deserialize<UserMetaData>(session.User.FindFirstValue("user_metadata"));

			ApplicationUser applicationUser = new ApplicationUser
			{
				Id = Guid.Parse(session.User.FindFirstValue("sub")),
				Email = metaData.email,
				FirstName = metaData.first_name,
				LastName = metaData.last_name,
				Role = session.User.FindFirstValue("role")
			};

			return applicationUser;
		}

		return null;
	}
}
