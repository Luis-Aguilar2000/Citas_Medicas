using Core;
using Infraestructura;
using Newtonsoft.Json.Serialization;
using Persistence;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Conecta Serilog con la API.
    builder.Host.UseSerilog();

    // Configura los controladores y las respuestas JSON.
    builder.Services
        .AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        });

    builder.Services.AddOpenApi();

    // Permite las peticiones desde Angular.
    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AngularPolicy", policy =>
        {
            policy
                .WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
    });

    // Registra las capas del proyecto.
    builder.Services.AddCore(builder.Configuration);
    builder.Services.AddPersistence();
    builder.Services.AddExternals(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    // Registra las peticiones que recibe esta API.
    app.UseSerilogRequestLogging();

    app.UseHttpsRedirection();

    app.UseCors("AngularPolicy");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "La API se detuvo por un error.");
}
finally
{
    Log.CloseAndFlush();
}