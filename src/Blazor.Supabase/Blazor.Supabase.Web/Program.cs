using Blazor.Supabase.Data;
using Blazor.Supabase.Web.Components;
using Radzen;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

builder.Services.AddTransient<CrepeRepository>();
builder.Services.AddTransient<IngredientRepository>();

var url = builder.Configuration["SUPABASE:Url"];
var key = builder.Configuration["SUPABASE:Key"];

var options = new Supabase.SupabaseOptions
{
	AutoConnectRealtime = true
};

builder.Services.AddSingleton(provider => new Supabase.Client(url, key, options));
builder.Services.AddRadzenComponents();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();

app.Run();
