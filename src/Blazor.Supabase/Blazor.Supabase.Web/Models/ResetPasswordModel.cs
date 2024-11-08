namespace BlazorSupabase.Web.Models;

public class ResetPasswordModel
{
	public string? Password { get; set; }

	public string? RepeatPassword { get; set; }

	public string? AccessToken { get; set; }

	public string? RefreshToken { get; set; }
}
