using System.Text.Json.Serialization;
using API.Endpoints;
using API.Errores;
using API.Restricciones;
using DA;
using DA.Semilla;
using Flujo;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// ---------- Servicios ----------
builder.Services
    .AddControllers()
    // Los enums viajan como texto ("EnProceso") en el JSON de los controladores.
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Lo mismo para Minimal API (usa otra configuración de JSON).
builder.Services.ConfigureHttpJsonOptions(o => o.SerializerOptions.Converters.Add(new JsonStringEnumConverter()));

// Restricción de ruta personalizada: {codigo:codigoTicket}
builder.Services.Configure<RouteOptions>(o => o.ConstraintMap.Add("codigoTicket", typeof(CodigoTicketConstraint)));

// Errores estándar (RFC 9457, application/problem+json) y manejador global de excepciones de negocio.
builder.Services.AddProblemDetails();
builder.Services.AddExceptionHandler<ManejadorExcepcionesGlobal>();

builder.Services.AddOpenApi();

// Cada capa registra lo suyo. El API solo orquesta (es la "raíz de composición").
var cadenaConexion = builder.Configuration.GetConnectionString("SistemaSoporte")
    ?? throw new InvalidOperationException("Falta ConnectionStrings:SistemaSoporte (appsettings o User Secrets).");

// "Memoria" = Práctica 1 (lista en memoria). "Sql" = Entity Framework Core (Semana 8 en adelante).
var ticketsEnMemoria = string.Equals(
    builder.Configuration["Almacenamiento:Tickets"], "Memoria", StringComparison.OrdinalIgnoreCase);

builder.Services.AddDatos(cadenaConexion, ticketsEnMemoria);
builder.Services.AddFlujo();

var app = builder.Build();

// ---------- Comando de semilla: dotnet run --project API -- --seed [--yes] ----------
if (args.Contains("--seed"))
{
    using var alcance = app.Services.CreateScope();
    await alcance.ServiceProvider.GetRequiredService<SemillaDatos>().EjecutarAsync(args.Contains("--yes"));
    return;
}

// ---------- Pipeline ----------
app.UseExceptionHandler();   // atrapa excepciones y las pasa al ManejadorExcepcionesGlobal
app.UseStatusCodePages();    // convierte respuestas vacías (404, 405...) en problem+json

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseDefaultFiles();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();
app.MapClientesMinimal();
app.MapTicketsMinimal();
app.MapEtiquetas();

app.Run();
