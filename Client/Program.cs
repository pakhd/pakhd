using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Pakhd.Client.Areas.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.ResponseCompression;
using Pakhd.Client.Hubs;
using Pakhd.Shared.Data;
using Microsoft.AspNetCore.Identity.UI.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<PakhdContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddDbContext<PakhdContext>(options =>
            options.UseSqlServer(connectionString,
                p => p.MigrationsAssembly("Pakhd.Shared")));



builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<PakhdUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<PakhdContext>();

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<PakhdUser>>();

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });  
});

builder.Services.AddScoped<ItemService>();
builder.Services.AddScoped<LotteryService>();
builder.Services.AddScoped<CurrencyService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<ReservationService>();
builder.Services.AddScoped<SalesOrderService>();
builder.Services.AddScoped<SalesLineService>();
builder.Services.AddScoped<ReferralService>();
builder.Services.AddScoped<WinnerService>();



builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.Configure<AuthMessageSenderOptions>(builder.Configuration);

builder.Services.Configure<IdentityOptions>(options => options.ClaimsIdentity.UserIdClaimType = ClaimTypes.NameIdentifier);


builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapBlazorHub();
app.MapHub<ItemHub>("/itemhub");
app.MapFallbackToPage("/_Host");

app.Run();
