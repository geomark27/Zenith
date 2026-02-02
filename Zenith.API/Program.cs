using Microsoft.EntityFrameworkCore;
using Zenith.Infrastructure.Data;
using DotNetEnv;

// Cargar variables del .env (si existe)
Env.Load();

var builder = WebApplication.CreateBuilder(args);

// Determinar connection string: primero variables de entorno, luego appsettings
string connectionString;

var dbServer = Environment.GetEnvironmentVariable("DB_SERVER");
if (!string.IsNullOrEmpty(dbServer))
{
    // Usar variables de entorno (para Docker/producción)
    var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "ZenithDB";
    var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "sa";
    var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "YourStrong@Password123";
    connectionString = $"Server={dbServer};Database={dbName};User Id={dbUser};Password={dbPassword};TrustServerCertificate=True;MultipleActiveResultSets=true;";
}
else
{
    // Usar appsettings (para desarrollo local)
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
}

// DbContext
builder.Services.AddDbContext<ZenithDbContext>(options =>
    options.UseSqlServer(
        connectionString,
        b => b.MigrationsAssembly("Zenith.Infrastructure")
    ));

// Rutas en minúsculas
builder.Services.AddRouting(options => options.LowercaseUrls = true);

// Services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();