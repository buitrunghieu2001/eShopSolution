using eShopSolution.ApiIntegration;
using eShopSolution.Application.System.Users;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login/Index";
        options.AccessDeniedPath = "/User/Forbidden";
    });

// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());

// set time expire session
builder.Services.AddSession(options =>
{
    // Thiết lập thời gian hủy của phiên làm việc khi không có hoạt động
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IUserApiClient, UserApiClient>();
builder.Services.AddTransient<IRoleApiClient, RoleApiClient>();
builder.Services.AddTransient<ILanguageApiClient, LanguageApiClient>();
builder.Services.AddTransient<IProductApiClient, ProductApiClient>();
builder.Services.AddTransient<ICategoryApiClient, CategoryApiClient>();
builder.Services.AddTransient<ISlideApiClient, SlideApiClient>();


// package runtime automatically compile without rerun
var enviroment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
#if DEBUG
if (enviroment == Environments.Development)
{
    builder.Services.AddRazorPages().AddRazorRuntimeCompilation();
}
#endif

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

app.UseAuthentication();

app.UseRouting();

app.UseAuthorization();
app.UseSession();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
