using eShopSolution.Application.Catalog.Cart;
using eShopSolution.Application.Catalog.Carts;
using eShopSolution.Application.Catalog.Categories;
using eShopSolution.Application.Catalog.Orders;
using eShopSolution.Application.Catalog.Products;
using eShopSolution.Application.Catalog.Reviews;
using eShopSolution.Application.Common;
using eShopSolution.Application.System.Brands;
using eShopSolution.Application.System.Languages;
using eShopSolution.Application.System.Roles;
using eShopSolution.Application.System.Users;
using eShopSolution.Application.Utilities.Slides;
using eShopSolution.BackendApi.Controllers;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Constants;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Connect to SqlServer
builder.Services.AddDbContext<EShopDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString(SystemConstants.MainConnectionString)));

builder.Services.AddIdentity<AppUser, AppRole>()
    .AddEntityFrameworkStores<EShopDbContext>()
    .AddDefaultTokenProviders();
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings
    options.Password.RequireDigit = true; // yêu cầu số
    options.Password.RequiredLength = 8; // độ dài 6
    options.Password.RequireUppercase = true; // chữ thường
    options.Password.RequireLowercase = true; // chữ hoa
    options.Password.RequireNonAlphanumeric = true; // Yêu cầu ít nhất một ký tự đặc biệt

    // Lockout settings
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30); //bị khóa sau khi đăng nhập ko thành công số lần quy định
    options.Lockout.MaxFailedAccessAttempts = 10; // tối đa 10 lần đăng nhập sai
    options.Lockout.AllowedForNewUsers = true; // người dùng mới có bị khóa không

    // User settings
    options.User.RequireUniqueEmail = true; // email là duy nhất
});
// Declare DI
// If requesting an object IPublicProductService it will create instance PublicProductService
builder.Services.AddTransient<IProductService, ProductService>();
builder.Services.AddTransient<IStorageService, FileStorageService>();
builder.Services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
builder.Services.AddTransient<SignInManager<AppUser>, SignInManager<AppUser>>();
builder.Services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
builder.Services.AddTransient<IUserService, UserService>();
builder.Services.AddTransient<ILanguageService, LanguageService>();
builder.Services.AddTransient<IRoleService, RoleService>();
builder.Services.AddTransient<ICategoryService, CategoryService>();
builder.Services.AddTransient<ISlideService, SlideService>();
builder.Services.AddTransient<ICartService, CartService>();
builder.Services.AddTransient<IReviewService, ReviewService>();
builder.Services.AddTransient<IBrandService, BrandService>();
builder.Services.AddTransient<IOrderService, OrderService>();

//builder.Services.AddTransient<IValidator<LoginRequest>, LoginRequestValidator>();
//builder.Services.AddTransient<IValidator<RegisterRequest>, RegisterRequestValidator>();


// Add services to the container.
// phương thức RegisterValidatorsFromAssemblyContaining sẽ quét toàn bộ các class có kế thừa từ lớp AbstractValidator trong assembly chứa lớp LoginRequestValidator và đăng ký chúng với FluentValidation.
// Do đó, nếu trong cùng assembly đó có các validator khác cũng được kế thừa từ AbstractValidator, thì chúng cũng sẽ được đăng ký và sử dụng được trong ứng dụng.
builder.Services.AddControllers()
    .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<LoginRequestValidator>());
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Swagger eShopSolution", Version = "v1" });

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header using the Bearer scheme. <br />
                      Enter 'Bearer' [space] and then your token in the text input below.<br />
                      Example: 'Bearer c3af813ff6d3a7c21568512bec6e4b3c'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
            Reference = new OpenApiReference
                {
                Type = ReferenceType.SecurityScheme,
                Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

builder.Services.AddSession();


//cho phép tất cả web đều có thể đọc được API
builder.Services.AddCors(option =>
{
    option.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
builder.Services.AddControllers();


string issuer = builder.Configuration.GetValue<string>("Tokens:Issuer");
string signingKey = builder.Configuration.GetValue<string>("Tokens:Key");
byte[] signingKeyBytes = System.Text.Encoding.UTF8.GetBytes(signingKey);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = issuer,
        ValidateAudience = true,
        ValidAudience = issuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = System.TimeSpan.Zero,
        IssuerSigningKey = new SymmetricSecurityKey(signingKeyBytes)
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();

// Use swagger
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Swagger eShopSolution V1");
});

app.UseCors();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
