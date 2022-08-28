using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using MVCTestProject.Data;
using MVCTestProject.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MVCTestProjectContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("MVCTestProjectContext") ?? throw new InvalidOperationException("Connection string 'MVCTestProjectContext' not found.")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => 
    {
        options.LoginPath = new PathString("/Account/Login");
    });

builder.Services.AddTransient<IDatabaseManager<MVCTestProjectContext>, DatabaseManager>();

builder.Services.AddSingleton<IHostedService, DataLoaderService>((ctx) =>
{
    IServiceProvider svc = ctx.GetService<IServiceProvider>();
    return new DataLoaderService(svc);
});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CryptocurrencyListing}/{action=Index}/{id?}");

app.Run();
