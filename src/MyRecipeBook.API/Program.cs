using Microsoft.OpenApi.Models;
using MyRecipeBook.API.BackgroundServices;
using MyRecipeBook.API.Filters;
using MyRecipeBook.API.Token;
using MyRecipeBook.Application;
using MyRecipeBook.Domain.Security.Tokens;
using MyRecipeBook.Infrastructure;
using MyRecipeBook.Infrastructure.Migrations;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = @"JWT Cabeçalho de autorização JWT usando o esquema Bearer.
                      Insira 'Bearer' [espaço] e, em seguida, seu token no campo de texto abaixo.
                      Exemplo: 'Bearer 123abcde'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});

var strLogLevel = builder.Configuration.GetValue<string>("Logging:LogLevel:Default");
var enumLogEventLevel = Enum.TryParse<LogEventLevel>(strLogLevel, true, out var parsedLogEventLevel)
? parsedLogEventLevel
: LogEventLevel.Information;

builder.Host.UseSerilog((context, configuration) =>
configuration.WriteTo.File("logs/log.txt",
enumLogEventLevel,
rollingInterval: RollingInterval.Day));
// .Host - para configurar o log
// .Services - Injeção de dependências
// .Configuration - Pegar algo do arquivo Json

builder.Services.AddMvc(option => option.Filters.Add(typeof(ExceptionFilter)));

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITokenProvider, HttpContextTokenValue>();

builder.Services.AddHostedService<DeleteUserServiceTimerUseCase>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MigrateDatabase();

await app.RunAsync();

return;

void MigrateDatabase()
{
    DatabaseMigration
    .Migrate(builder.Configuration,
    app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope().ServiceProvider);
}
