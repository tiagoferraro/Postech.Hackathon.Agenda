using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System.Reflection;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.AspNetCore.Authorization;

namespace Postech.Hackathon.Agenda.Ioc;

public static class SwaggerConfiguration
{
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(static options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Postech Hackathon - Agendamento API",
                Version = "v1",
                Description = "API de autorização para o Hackathon da Postech",
                Contact = new OpenApiContact
                {
                    Name = "Equipe Postech",
                    Email = "contato@postech.com"
                }
            });

            // Configuração para autenticação JWT no Swagger
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header usando o esquema Bearer. Exemplo: \"Authorization: Bearer {token}\"",
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
                            }
                        },
                        Array.Empty<string>()
                    }
                });

            // Adiciona operação filter para identificar endpoints com [Authorize]
            options.OperationFilter<SecurityRequirementsOperationFilter>();

            // Inclui comentários XML se disponíveis
            var xmlFile = $"{Assembly.GetEntryAssembly()?.GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    public static WebApplication MapOpenApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c => 
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Postech Hackathon Agendamento API v1");
            c.RoutePrefix = string.Empty;
            c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        });

        return app;
    }
}

// Filtro para identificar endpoints com [Authorize]
public class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        // Verifica se o endpoint tem o atributo [Authorize]
        var authAttributes = context.MethodInfo.DeclaringType!.GetCustomAttributes(true)
            .Union(context.MethodInfo.GetCustomAttributes(true))
            .OfType<AuthorizeAttribute>();

        if (authAttributes.Any())
        {
            // Adiciona o requisito de segurança para endpoints com [Authorize]
            operation.Security =
            [
                new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            Array.Empty<string>()
                        }
                    }
            ];

            // Obtém as roles do atributo [Authorize]
            var roles = authAttributes
                .SelectMany(a => a.Roles?.Split(',') ?? Array.Empty<string>())
                .Where(r => !string.IsNullOrEmpty(r))
                .Distinct()
                .ToList();

            var rolesDescription = roles.Any() 
                ? $"Roles permitidas: {string.Join(", ", roles)}" 
                : "Requer autenticação";

            // Adiciona um ícone de cadeado e as roles na descrição
            operation.Description = string.IsNullOrEmpty(operation.Description)
                ? $"🔒 Endpoint protegido - {rolesDescription}"
                : $"🔒 Endpoint protegido - {rolesDescription}\n{operation.Description}";
        }
        else
        {
            // Adiciona indicação para endpoints sem [Authorize]
            operation.Description = string.IsNullOrEmpty(operation.Description)
                ? "🔓 Endpoint público - Não requer autenticação"
                : $"🔓 Endpoint público - Não requer autenticação\n{operation.Description}";
        }
    }
}

