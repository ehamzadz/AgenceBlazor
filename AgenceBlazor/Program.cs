using AgenceBlazor.Components;
using AgenceBlazor.Data;
using AgenceBlazor.Services;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddMudServices();

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
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();