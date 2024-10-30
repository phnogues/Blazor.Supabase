using BlazorSupabase.Models.Dtos;
using BlazorSupabase.Web.Helpers;
using BlazorSupabase.Web.Models;
using FluentResults;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Supabase.Gotrue;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text.Json;

namespace BlazorSupabase.Web.Services;

public class AuthService
{
	private const string AccessToken = nameof(AccessToken);
	private const string RefreshToken = nameof(RefreshToken);

	private readonly ProtectedLocalStorage _localStorage;
	private readonly NavigationManager _navigation;
	private readonly IConfiguration _configuration;
	private readonly Supabase.Client _supabase;

    public AuthService(ProtectedLocalStorage localStorage, NavigationManager navigation, IConfiguration configuration, Supabase.Client supabase)
	{
		_localStorage = localStorage;
		_navigation = navigation;
		_configuration = configuration;
		_supabase = supabase;
	}

    public async Task<Result> LoginAsync(string email, string password)
	{
		try
		{
			var response = await _supabase.Auth.SignIn(email, password);
			if (string.IsNullOrEmpty(response?.AccessToken))
			{
				return Result.Fail("An error occured");
			}

			await _localStorage.SetAsync(AccessToken, response.AccessToken);
			await _localStorage.SetAsync(RefreshToken, response.RefreshToken);

			return Result.Ok();
		}
		catch (Exception ex)
		{
			var error = JsonSerializer.Deserialize<AuthError>(ex.Message);

			return Result.Fail(GetIgoTrueError(error.error_code));
		}
	}

	public async Task<Result> RegisterAsync(RegisterModel model)
	{
		try
		{
			SignUpOptions signUpOptions = new()
			{
				Data = new Dictionary<string, object>
			{
				{ "first_name", model.FirstName },
				{ "last_name", model.LastName },
			}
			};

			var data = await _supabase.Auth.SignUp(model.Email, model.Password, signUpOptions);
			return Result.Ok();
		}
		catch (Exception ex)
		{
			var error = JsonSerializer.Deserialize<AuthError>(ex.Message);

			return Result.Fail(GetIgoTrueError(error.error_code));
		}
	}

	public async Task<Result> ForgotPasswordAsync(string email)
	{
		ResetPasswordForEmailOptions resetPasswordForEmailOptions = new ResetPasswordForEmailOptions(email)
		{
			RedirectTo = $"{_configuration["RedirectUrl"]}account/reset-password",
		};

		await _supabase.Auth.ResetPasswordForEmail(resetPasswordForEmailOptions);
		return Result.Ok();
	}

	public async Task<Result<User>> UpdatePassword(ResetPasswordModel model)
	{
		try
		{
			UserAttributes userAttributes = new()
			{
				Password = model.Password
			};

			var session = await _supabase.Auth.SetSession(model.AccessToken, model.RefreshToken);
			var user = await _supabase.Auth.Update(userAttributes);

			if (user is null)
				return Result.Fail("An error occured");

			return Result.Ok(user);
		}
		catch (Exception ex)
		{
			var error = JsonSerializer.Deserialize<AuthError>(ex.Message);

			return Result.Fail<User>(GetIgoTrueError(error.error_code));
		}
	}

	public async Task LogoutAsync()
	{
		await RemoveAuthDataFromStorageAsync();
		Thread.Sleep(300);
		_navigation.NavigateTo("/", true);
	}

	public async Task<List<Claim>> GetLoginInfoAsync()
	{
		var emptyResult = new List<Claim>();
		ProtectedBrowserStorageResult<string> accessToken;
		ProtectedBrowserStorageResult<string> refreshToken;

		try
		{
			accessToken = await _localStorage.GetAsync<string>(AccessToken);
			refreshToken = await _localStorage.GetAsync<string>(RefreshToken);
		}
		catch (CryptographicException)
		{
			await LogoutAsync();
			return emptyResult;
		}

		if (accessToken.Success is false || accessToken.Value == default)
			return emptyResult;

		var claims = JwtTokenHelper.ValidateDecodeToken(accessToken.Value, _configuration);

		if (claims.Count != 0)
			return claims;

		if (refreshToken.Value != default)
		{
			// TODO : Implement refresh token logic
		}
		else
		{
			await LogoutAsync();
		}
		return claims;
	}

	private async Task RemoveAuthDataFromStorageAsync()
	{
		await _localStorage.DeleteAsync(AccessToken);
		await _localStorage.DeleteAsync(RefreshToken);
	}

	private string GetIgoTrueError(string errorCode)
	{
		string cleanMessage = "";

		switch (errorCode)
		{
			case "user_already_exists":
				cleanMessage = "An account already exists with this email, please log in";
				break;
			case "weak_password":
				cleanMessage = "Password is too weak, please enter at least 6 characters (lowercase, uppercase and numbers)";
				break;
			case "invalid_credentials":
				cleanMessage = "Incorrect email or password";
				break;
			case "same_password":
				cleanMessage = "The new password must be different from the old one";
				break;
			case "email_not_confirmed":
				cleanMessage = "Email not confirmed, please check your inbox";
				break;
		}

		return cleanMessage;
	}
}
