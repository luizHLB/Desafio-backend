using Microsoft.OpenApi.Models;
using Product.API.Workers;
using Product.Data.Contexts;
using Product.Data.DependencyInjection;
using Product.Data.Util;
using Product.Service.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var config = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json", optional: false)
               .AddEnvironmentVariables()
               .Build();
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddRepositories(config);
builder.Services.AddApplication(config);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Product - API",
        Description = "Documentação padrão de apoio para desenvolvimento e integração."
    });

    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Jwt Authorization Token. Example: \"bearer {token}\"",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
});

//builder.Services.AddHostedService<NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    InitDB.RunMigration(services.GetService<ProductContext>(), config);
}

app.Run();
