using Infrastructure.Contexts;
using Infrastructure.Entities;
using Infrastructure.Factories;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

builder.Services.AddAuthentication().AddFacebook(x =>
{
    x.AppId = "285305684586290";
    x.AppSecret = "3f2c7094856cd54cbea8f35009b3a2c8";
    x.Fields.Add("first_name");
    x.Fields.Add("last_name");
});

builder.Services.AddAuthentication().AddGoogle(x =>
{
    x.ClientId = "449921598247-6k43tk5s698j0ohlqhrc9kj6m8c95do4.apps.googleusercontent.com";
    x.ClientSecret = "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";
    x.CallbackPath = "/signin-google";
});


builder.Services.AddScoped<AddressRepository>();
builder.Services.AddScoped<AddressFactory>();
builder.Services.AddScoped<AddressService>();

builder.Services.AddScoped<UserRepository>();
builder.Services.AddScoped<UserFactory>();
builder.Services.AddScoped<UserService>();

builder.Services.AddScoped<CourseRepository>();
builder.Services.AddScoped<CourseFactory>();
builder.Services.AddScoped<CourseService>();

builder.Services.AddScoped<SavedCoursesRepository>();

builder.Services.AddScoped<SubscriberRepository>();
builder.Services.AddScoped<SubscriberFactory>();
builder.Services.AddScoped<SubscriberService>();

builder.Services.AddScoped<ContactRepository>();
builder.Services.AddScoped<ContactFactory>();
builder.Services.AddScoped<ContactService>();


builder.Services.AddDefaultIdentity<UserEntity>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.SignIn.RequireConfirmedAccount = false;
    x.Password.RequiredLength = 8;
    x.Password.RequireNonAlphanumeric = false;
}).AddEntityFrameworkStores<DataContext>();

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/signin";
    x.Cookie.HttpOnly = true;
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    x.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    x.SlidingExpiration = true;
});

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseStatusCodePagesWithRedirects("/404");

app.Run();
