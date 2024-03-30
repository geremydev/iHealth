using iHealth.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using iHealth.Infrastructure.Persistence.Repository;
using iHealth.Core.Application;
using iHealth.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

var connectionString = builder.Configuration.GetConnectionString("Default");
builder.Services.AddSession();
builder.Services.AddDbContext<iHealthContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddPersistenceInfrastructure(builder.Configuration);
builder.Services.AddPersistenceApplication();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<ValidateUserSession, ValidateUserSession>();

var app = builder.Build();




app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Menu/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Menu}/{action=Login}/{id?}");

app.Run();
