using E_Food.Configuraciones;
using EFood.AccesoDatos.Data;
using EFood.AccesoDatos.Repositorio;
using EFood.AccesoDatos.Repositorio.IRepositorio;
using EFood.AccesoDatos.Servicio;
using EFood.Utilidades;
using Humanizer.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Options;
using System.Configuration;
using Microsoft.Extensions.Azure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var configuration = builder.Configuration; // Obtiene la configuraci�n
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = $"/Identity/Account/Login";
    options.LogoutPath = $"/Identity/Account/Logout";
    options.AccessDeniedPath = $"/Identity/Account/AccessDenied";
});

builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddScoped<IUnidadTrabajo, UnidadTrabajo>();

builder.Services.AddRazorPages();

builder.Services.AddSingleton<IEmailSender, EmailSender>();

// Bind Azure Blob Storage configuration from appsettings.json
builder.Services.Configure<AzureBlobStorageConfiguration>(configuration.GetSection("AzureBlobStorage"));

// Register the IStorageService with Azure Blob Storage configuration
builder.Services.AddScoped<IServicioStorage, ServicioStorage>(provider =>
{
    var azureBlobStorageConfiguration = provider.GetRequiredService<IOptions<AzureBlobStorageConfiguration>>().Value;
    return new ServicioStorage(azureBlobStorageConfiguration.ConnectionString);
});
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(builder.Configuration["AzureBlobStorage:ConnectionString:blob"]!, preferMsi: true);
    clientBuilder.AddQueueServiceClient(builder.Configuration["AzureBlobStorage:ConnectionString:queue"]!, preferMsi: true);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
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


//var sharedFolderPath = Path.Combine(Directory.GetParent(app.Environment.ContentRootPath).FullName, "Imagenes");
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(sharedFolderPath),
//    RequestPath = "/Imagenes"
//});



app.MapControllerRoute(
    name: "default",
    pattern: "{area=Inventario}/{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
