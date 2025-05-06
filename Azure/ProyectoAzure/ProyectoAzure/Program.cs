using Microsoft.EntityFrameworkCore;
using ProyectoAzure.Data;
using ProyectoAzure.Services; // Aseg�rate de importar el namespace de los servicios

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios MVC
builder.Services.AddControllersWithViews();

// Configurar conexi�n a la base de datos (Azure SQL)
builder.Services.AddDbContext<AplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("StringConnection")));

// Registrar el servicio de Azure Service Bus
builder.Services.AddScoped<IServiceBusService, ServiceBusService>();

var app = builder.Build();

// Configuraci�n del pipeline de solicitudes HTTP
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Configurar rutas de los controladores
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

//using Microsoft.EntityFrameworkCore;
//using ProyectoAzure.Data;
//using Azure.Messaging.ServiceBus;

//var builder = WebApplication.CreateBuilder(args);

//// Agregar servicios MVC
//builder.Services.AddControllersWithViews();

//// Configurar conexi�n a la base de datos (Azure SQL)
//builder.Services.AddDbContext<AplicationDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("StringConnection")));

//// Configurar conexi�n a Azure Service Bus
//string serviceBusConnectionString = builder.Configuration.GetConnectionString("AzureServiceBusConnection");
//builder.Services.AddSingleton<ServiceBusClient>(new ServiceBusClient(serviceBusConnectionString));

//var app = builder.Build();

//// Configuraci�n del pipeline de solicitudes HTTP
//if (!app.Environment.IsDevelopment()) {
//    app.UseExceptionHandler("/Home/Error");
//    app.UseHsts();
//}

//app.UseHttpsRedirection();
//app.UseStaticFiles();

//app.UseRouting();
//app.UseAuthorization();

//// Configurar rutas de los controladores
//app.MapControllerRoute(
//    name: "default",
//    pattern: "{controller=Home}/{action=Index}/{id?}");

//app.Run();
