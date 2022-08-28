using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using MVCTestProject.Data;
using MVCTestProject.Services;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddDbContext<UserContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("UserContext") ?? throw new InvalidOperationException("Connection string 'MVCTestProjectContext' not found.")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options => //CookieAuthenticationOptions
    {
        options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
    });
//builder.Services.AddSingleton<IHostedService, DataLoader>(s => new DataLoader(new UserContext(null, builder.Configuration.GetConnectionString("UserContext"))));
builder.Services.AddTransient<IDatabaseManager<UserContext>, DatabaseManager>();
builder.Services.AddSingleton<IHostedService, DataLoaderService>();
builder.Services.AddSingleton<IHostedService, DataLoaderService>((ctx) =>
{
    IServiceProvider svc = ctx.GetService<IServiceProvider>();
    return new DataLoaderService(svc);
});

//builder.Services.AddSingleton<IDataLoader, DataLoader>();
//builder.Services.AddControllersWithViews();

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

//using (var serviceScope = app.Services.CreateScope())
//{
//    var serviceScope = app.Services.CreateScope();
//    var services = serviceScope.ServiceProvider;

//    var myDependency = services.GetRequiredService<IDataLoader>();
//    myDependency.SetService(services);
//    myDependency.StartAsync(new CancellationToken());
//}

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

app.UseAuthentication();    // аутентификация
app.UseAuthorization();     // авторизация

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=CryptocurrencyListing}/{action=Index}/{id?}");

app.Run();
