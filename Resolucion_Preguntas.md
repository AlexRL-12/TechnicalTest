# Mi API con ASP.NET Core: Implementación de Autenticación, Autorización, Middleware y Optimización

## Sección 2: Autenticación y Autorización

Para manejar la autenticación en mi API, utilizo **JWT (JSON Web Tokens)** para autenticar a los usuarios. Configuro la autenticación en el archivo `Program.cs` agregando un middleware de JWT en la configuración de servicios:

```csharp
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
Autorización
Para restringir el acceso a ciertos endpoints, uso el atributo [Authorize] en los controladores, especificando los roles necesarios. Por ejemplo:

```csharp

[Authorize(Roles = "Admin")]
public ActionResult<Employee> AddEmployee([FromBody] Employee employee)
{
    // Código para agregar empleados
}

Middleware en ASP.NET Core
El middleware en ASP.NET Core es clave para procesar cada solicitud HTTP que llega a mi API. Se ejecuta en una secuencia definida y puede modificar tanto la solicitud como la respuesta. Si quiero crear un middleware personalizado para registrar los detalles de cada solicitud entrante, lo hago de la siguiente manera:

```csharp
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;

    public RequestLoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        // Llamo al siguiente middleware en la cadena
        await _next(context);

        stopwatch.Stop();
        var logMessage = $"[{context.Request.Method}] {context.Request.Path} responded in {stopwatch.ElapsedMilliseconds} ms";
        Console.WriteLine(logMessage);  
    }
}

Para registrar este middleware en la tubería de la aplicación, lo agrego en Program.cs:

```csharp

app.UseMiddleware<RequestLoggingMiddleware>();
Sección 4: Diseño de Base de Datos y EF Core
Consulta LINQ: Empleados de un departamento en al menos un proyecto
Si necesito obtener empleados de un departamento específico que estén asignados a al menos un proyecto, utilizo esta consulta LINQ:

```csharp
var employeesInDepartment = _context.Employees
    .Include(e => e.Projects)
    .Where(e => e.DepartmentId == departmentId && e.Projects.Any())
    .ToList();
Aquí, me aseguro de incluir los proyectos asociados a los empleados y filtro a aquellos que pertenecen al departamento indicado y están asignados a al menos un proyecto.

Sección 5: Rendimiento y Optimización
Problemas comunes de rendimiento y soluciones
Consultas ineficientes: Para evitar consultas ineficientes, utilizo carga diferida o explícita en lugar de cargar datos ansiosamente.
Uso de memoria: Cuido el uso de memoria evitando objetos grandes innecesarios y controlando el ciclo de vida de los objetos, implementando patrones como Object Pooling.
Bloqueos de concurrencia: Evito bloqueos asegurando que los recursos compartidos se manejen correctamente, utilizando operaciones asíncronas y mecanismos de sincronización como lock o Semaphore.
Almacenamiento en caché: Implemento caché con MemoryCache o Distributed Cache para reducir la carga en la base de datos y mejorar el rendimiento.
Optimización de consultas lentas
Perfilado de consultas: Uso herramientas como MiniProfiler o SQL Profiler para identificar consultas lentas.
Revisión de consultas generadas: Uso ToQueryString() en EF Core para revisar la consulta SQL que se está ejecutando.
Optimización de índices: Verifico si las columnas usadas tienen los índices adecuados en la base de datos.
Paginación: Si se están obteniendo muchos datos, implemento paginación para dividir los resultados en lotes más pequeños.
Uso de proyecciones: Utilizo Select en lugar de cargar toda la entidad con Include() para traer solo los datos necesarios.