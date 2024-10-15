using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore; // Asegúrate de que esta línea esté presente
using TechnicalTest.Models;
using TechnicalTest.Services;
using TechnicalTest.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la cadena de conexión a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Asegúrate de que UseSqlServer esté disponible

// Agregar el servicio de autenticación Negotiate
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

// Agregar autorización por defecto
builder.Services.AddAuthorization(options =>
{
  options.FallbackPolicy = options.DefaultPolicy;
});

// Registrar EmployeeService para inyección de dependencias
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

var app = builder.Build();

// Configurar el pipeline HTTP.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();  // Habilitar autenticación
app.UseAuthorization();   // Habilitar autorización

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.Run();
