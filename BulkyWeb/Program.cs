using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository;
using Bulky.DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Bulky.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;
using Stripe;
using Microsoft.Build.Framework;
using Bulky.DataAccess.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
// This line of code indicates that the server connection is established and we connect to the particular database server using the connection string.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));

// All services mentioned as "builder.Services.service<Type, Implementation>()".

// Service which deals with the login feature.
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

// Service which redirects the user to these specific custom links if such errors 
// occur while the program runs. Basically these links redirect user to the webpages 
// tat are designed specifically for clean display and user readability.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

// Adding the service to configure facebook authorization in the app.
builder.Services.AddAuthentication().AddFacebook(option =>
{
    option.AppId = "1890222058166919";
    option.AppSecret = "c0a83a69f44c06c85a322fa0421fe6bb";
});

// Adding the Session to the services.
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddScoped<IDbInitializer, DbInitializer>();

// Add the Razor Pages.
builder.Services.AddRazorPages();

// Adding the Category Repository in the depedency injection.
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Adding the EmailServices implementation.
builder.Services.AddScoped<IEmailSender, EmailSender>();


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

StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseRouting();

// Provide routing for Razor pages.
app.MapRazorPages();

// Checks if the username or password is valid. It will also 
// validate the role of the user, checks whether the user is 
// ADMIN or Customer.
app.UseAuthentication();
app.UseAuthorization();
// Now, the app is configured to use Session.
app.UseSession();
SeedDatabase();
app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();

void SeedDatabase()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbIntializer = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
        dbIntializer.Initialize();

    }
}
