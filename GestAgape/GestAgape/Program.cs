using FluentValidation.AspNetCore;
using GestAgape.Core.Entities;
using GestAgape.Models;
using GestAgape.Service.Admissions;
using GestAgape.Service.Identity;
using GestAgape.Service.MailService;
using GestAgape.Service.Parametrages;
using GestAgape.Service.Scolarite;
using GestAgape.UnitOfWork;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
#pragma warning disable CS0618 // Le type ou le membre est obsolète
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles
).AddRazorRuntimeCompilation().AddFluentValidation(options =>
{
    // Validate child properties and root collection elements
    options.ImplicitlyValidateChildProperties = true;
    options.ImplicitlyValidateRootCollectionElements = true;

    // Automatic registration of validators in assembly
    options.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
});
#pragma warning restore CS0618 // Le type ou le membre est 
builder.Services.AddDbContext<IdentityContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
    //AsNoTracking()
});
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IIdentityManagement, IdentityManagementService>();
builder.Services.AddTransient<IParametrage, ParametrageService>();
builder.Services.AddTransient<IAdmission, AdmissionService>();
builder.Services.AddTransient<IScolarite, ScolariteService>();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();
builder.Services.AddSession();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(55);
});
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddIdentity<ApplicationUsers, IdentityRole>().AddEntityFrameworkStores<IdentityContext>().AddDefaultTokenProviders();

// ajouservicete l'email 



//recupere la chaine de connection pour l'email
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

builder.Services.Configure<IdentityOptions>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireLowercase = true;
    opt.Password.RequireNonAlphanumeric = true;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireDigit = true;
    opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromSeconds(30);
    opt.Lockout.MaxFailedAccessAttempts = 5;
    //opt.SignIn.RequireConfirmedAccount = true;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Home/Accessdenied");
    options.LoginPath = "/IdentityManagement/Login";
    //options.LogoutPath = "/IdentityManagement/LogOut";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    options.SlidingExpiration = true;

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
app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=IdentityManagement}/{action=Login}/{id?}");

DataSeeding();

app.Run();

void DataSeeding()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider
            .GetRequiredService<IIdentityManagement>();
        dbInitializer.Initialize();
    }
}