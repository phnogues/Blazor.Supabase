using BlazorSupabase.Data;
using BlazorSupabase.Web.Components;
using BlazorSupabase.Web.Providers;
using BlazorSupabase.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

// Data repositories
builder.Services.AddTransient<CrepeRepository>();
builder.Services.AddTransient<IngredientRepository>();

// Authenticated services
builder.Services.AddTransient<AuthService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddScoped<AuthenticationStateProvider, SupabaseAuthenticationStateProvider>();
builder.Services.AddSingleton<IAuthorizationMiddlewareResultHandler, BlazorAuthorizationMiddlewareResultHandler>();
builder.Services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Supabase
var url = builder.Configuration["SUPABASE:Url"];
var key = builder.Configuration["SUPABASE:Key"];

var options = new Supabase.SupabaseOptions
{
	AutoConnectRealtime = true
};

builder.Services.AddScoped(provider => new Supabase.Client(url, key, options));

builder.Services.AddHealthChecks();
builder.Services.AddRadzenComponents();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();
app.UseHealthChecks("/health");

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
