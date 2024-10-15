using Microsoft.AspNetCore.Authentication.Negotiate;
using Microsoft.EntityFrameworkCore; // Aseg�rate de que esta l�nea est� presente
using TechnicalTest.Models;
using TechnicalTest.Services;
using TechnicalTest.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Agregar servicios al contenedor.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configurar la cadena de conexi�n a la base de datos
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))); // Aseg�rate de que UseSqlServer est� disponible

// Agregar el servicio de autenticaci�n Negotiate
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
    .AddNegotiate();

// Agregar autorizaci�n por defecto
builder.Services.AddAuthorization(options =>
{
  options.FallbackPolicy = options.DefaultPolicy;
});

// Registrar EmployeeService para inyecci�n de dependencias
builder.Services.AddScoped<IEmployeeService, EmployeeService>();

var app = builder.Build();

// Configurar el pipeline HTTP.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();  // Habilitar autenticaci�n
app.UseAuthorization();   // Habilitar autorizaci�n

app.UseMiddleware<RequestLoggingMiddleware>();

app.MapControllers();

app.Run();
