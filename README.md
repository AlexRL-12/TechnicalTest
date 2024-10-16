# Employee Management API

## Descripci�n

Este proyecto es una API para la gesti�n de empleados que permite registrar, actualizar, eliminar y listar empleados. Tambi�n implementa autenticaci�n JWT y autorizaci�n basada en roles (`Admin` y `User`). El proyecto sigue los principios SOLID y utiliza los patrones de dise�o **Strategy** para el c�lculo de bonos y **Repository** para la separaci�n de la l�gica de acceso a datos.

## Requisitos

- .NET 6 SDK
- MySQL (o cualquier otra base de datos compatible)
- Visual Studio, Visual Studio Code, o cualquier editor compatible con .NET
- Postman o similar para probar los endpoints de la API

## Configuraci�n

### Paso 1: Clonar el repositorio

Clona este repositorio en tu m�quina local:

```bash
git clone https://github.com/AlexRL-12/TechnicalTest.git
Paso 2: Configurar la conexi�n a la base de datos
Abre el archivo appsettings.json y actualiza la cadena de conexi�n a tu base de datos MySQL:

Crea una base de datos en tu mysql llamada employee1 en este caso
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=employee1;User=root;Password=tu_contrase�a;"
}

Paso 3: Crear la base de datos
Si no tienes la base de datos creada, ejecuta las migraciones de Entity Framework Core para crear las tablas necesarias:


dotnet ef database update

Paso 4: Ejecutar el proyecto
Para ejecutar la aplicaci�n, usa el siguiente comando:


dotnet run
Esto iniciar� el servidor en el puerto 5001 para HTTPS. Puedes cambiar este puerto en el archivo launchSettings.json si es necesario.

Paso 5: Probar la API
Puedes utilizar herramientas como Postman para probar los siguientes endpoints.

Endpoints
Autenticaci�n
Iniciar sesi�n (Login)

POST /api/auth/login
Cuerpo de la solicitud:

{
  "username": "NombreDeUsuario",
  "password": "Contrase�a"
}

Respuesta:

{
  "token": "JWT_TOKEN_AQU�"
}


Gesti�n de Empleados
Nota: Necesitar�s un token JWT para acceder a estos endpoints (se obtiene al iniciar sesion)

Obtener todos los empleados

GET /api/employees
Requiere roles: Admin o User
Obtener un empleado por ID

GET /api/employees/{id}
Requiere roles: Admin o User
Agregar un nuevo empleado

POST /api/employees
Requiere roles: Admin
Cuerpo de la solicitud:

{
  "name": "Nuevo Empleado",
  "currentPosition": 1,
  "salary": 50000,
  "position": "Desarrollador",
  "departmentId": 1
}
Actualizar un empleado existente

PUT /api/employees/{id}
Requiere roles: Admin
Eliminar un empleado

DELETE /api/employees/{id}
Requiere roles: Admin
Arquitectura
El proyecto sigue una arquitectura desacoplada donde se aplican los principios SOLID:

SRP (Responsabilidad �nica): Cada clase tiene una �nica responsabilidad.
Patr�n Repository: Para abstraer el acceso a los datos y permitir cambios futuros sin afectar el servicio.
Patr�n Strategy: Para el c�lculo de bonos basado en la posici�n del empleado.
Seguridad
Este proyecto utiliza JWT (JSON Web Tokens) para autenticaci�n y autorizaci�n:

Admin: Tiene permisos completos para agregar, actualizar y eliminar empleados.
User: Solo tiene permiso para listar y consultar empleados.
Middleware
Un middleware personalizado registra la duraci�n de cada solicitud HTTP:


public class RequestLoggingMiddleware
{
    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = new Stopwatch();
        stopwatch.Start();

        await _next(context);

        stopwatch.Stop();
        var logMessage = $"[{context.Request.Method}] {context.Request.Path} responded in {stopwatch.ElapsedMilliseconds} ms";
        Console.WriteLine(logMessage);  
    }
}

Problemas comunes de rendimiento y optimizaci�n
Consultas lentas: Se evita cargando solo los datos necesarios con AsNoTracking() y Include().
Herramientas de profiling: Puedes utilizar MiniProfiler o Application Insights para identificar consultas lentas y mejorar el rendimiento.

Contribuciones

Si deseas contribuir, por favor abre un pull request o issue.