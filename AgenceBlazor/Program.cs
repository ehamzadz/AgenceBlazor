using AgenceBlazor.Components;
using AgenceBlazor.Data;
using AgenceBlazor.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddMudServices();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/login";
        options.AccessDeniedPath = "/login";
        options.LogoutPath = "/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(7);
        options.SlidingExpiration = true;
    });

builder.Services.AddAuthorizationCore(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Add DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Services
builder.Services.AddScoped<TripService>();
builder.Services.AddScoped<AgencyService>();
builder.Services.AddScoped<HotelService>();
builder.Services.AddScoped<AgencyBookingService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<HotelInfoService>();
builder.Services.AddScoped<ExpenseService>();
builder.Services.AddScoped<TreasuryService>();
builder.Services.AddScoped<DirectPilgrimService>();
builder.Services.AddScoped<ITripGuideService, TripGuideService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets().AllowAnonymous();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

// Login endpoint
app.MapPost("/Account/Login", async (HttpContext http,
    [FromForm] string username,
    [FromForm] string password,
    [FromForm] string? returnUrl) =>
{
    if (username == "khoudour" && password == "123456**")
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, "Admin")
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);

        await http.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal,
            new AuthenticationProperties { IsPersistent = true });

        var redirectUrl = string.IsNullOrEmpty(returnUrl) ? "/TripsGallery" : returnUrl;
        return Results.Redirect(redirectUrl);
    }

    return Results.Redirect("/login?error=1");
})
.AllowAnonymous()
.DisableAntiforgery();

// Logout endpoint - Support both GET and POST
app.MapGet("/Account/Logout", async (HttpContext http) =>
{
    await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
})
.AllowAnonymous();

app.MapPost("/Account/Logout", async (HttpContext http) =>
{
    await http.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    return Results.Redirect("/login");
})
.AllowAnonymous()
.DisableAntiforgery();

app.Run();