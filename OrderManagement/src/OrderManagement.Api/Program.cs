using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using OrderManagement.Api;
using OrderManagement.Application;
using OrderManagement.Infrastructure;
using OrderManagement.Infrastructure.Persistence;
using Scalar.AspNetCore;
using Serilog;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((cancellationTokenx, cfg) => cfg.ReadFrom.Configuration(cancellationTokenx.Configuration).WriteTo.Console());

ConfigurationManager configuration = builder.Configuration;
IServiceCollection services = builder.Services;

services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(
                new JsonStringEnumConverter());
        });

services.AddApplication()
        .AddInfrastructure(configuration);

IConfigurationSection jwt = configuration.GetSection("Jwt");

services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(o => o.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidIssuer = jwt["Issuer"],
            ValidateAudience = true,
            ValidAudience = jwt["Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!)),
            ClockSkew = TimeSpan.Zero
        }
        );

services.AddAuthorization();

services.AddOpenTelemetry()
        .ConfigureResource(r => r.AddService("OrderManagement.Api"))
                                 .WithTracing(t => t.AddAspNetCoreInstrumentation()
                                                    .AddHttpClientInstrumentation()
                                                    .AddConsoleExporter())
                                 .WithMetrics(m => m.AddAspNetCoreInstrumentation()
                                                    .AddHttpClientInstrumentation()
                                                    .AddConsoleExporter());

services.AddOpenApi(options =>
{
    options.AddDocumentTransformer(
        (document, context, cancellationToken) =>
        {
            document.Components ??= new OpenApiComponents();

            document.Components.SecuritySchemes ??=
                new Dictionary<string, IOpenApiSecurityScheme>();

            document.Components.SecuritySchemes["Bearer"] =
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Informe somente o JWT retornado por POST /auth/login."
                };

            return Task.CompletedTask;
        });

    options.AddOperationTransformer((operation, context, cancellationToken) =>
        {
            bool allowAnonymous = context.Description.ActionDescriptor.EndpointMetadata.OfType<IAllowAnonymous>().Any();

            bool requiresAuthorization = context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any();

            if (!allowAnonymous && requiresAuthorization)
            {
                operation.Security ??= [];

                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [
                        new OpenApiSecuritySchemeReference(
                            "Bearer",
                            context.Document)
                    ] = []
                });
            }

            return Task.CompletedTask;
        });
});

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseSerilogRequestLogging();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await using (AsyncServiceScope scope = app.Services.CreateAsyncScope())
{
    AppDbContext db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    await db.Database.MigrateAsync();
}

#pragma warning disable format
await app.RunAsync();
#pragma warning restore format