using API.Middlewares;
using Core.Facturacion;
using Infraestructura;
using Newtonsoft.Json.Serialization;
using Persistence.Facturacion;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Host.UseSerilog();

    builder.Services
        .AddControllers()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();
        });

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

    builder.Services.AddOpenApi();

    builder.Services.AddCore(builder.Configuration);
    builder.Services.AddPersistence();
    builder.Services.AddExternals(builder.Configuration);

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
    }

    app.UseSerilogRequestLogging();

    app.UseMiddleware<ExceptionMiddleware>();

    app.UseCors("AngularPolicy");

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(
        ex,
        "La API de Facturación se detuvo por un error."
    );

    throw;
}
finally
{
    Log.CloseAndFlush();
}