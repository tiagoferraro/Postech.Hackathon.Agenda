using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Postech.Hackathon.Agenda.Infra.Interfaces;
using Postech.Hackathon.Agenda.Infra.Repositories;
using Postech.Hackathon.Agenda.Application.Services;
using Postech.Hackathon.Agenda.Application.Interfaces;
using Postech.Hackathon.Agenda.Infra.Settings;

namespace Postech.Hackathon.Agenda.Ioc;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<DatabaseSettings>(options => configuration.GetSection("DatabaseSettings").Bind(options));
        services.Configure<RedisSettings>(options => configuration.GetSection("RedisSettings").Bind(options));
        services.AddScoped<IHorarioDisponivelRepository, HorarioDisponivelRepository>();
        services.AddScoped<IAgendamentoRepository, AgendamentoRepository>();

        // Redis
        var redisSettings = configuration.GetSection("RedisSettings").Get<RedisSettings>();
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = redisSettings?.ConnectionString;
            options.InstanceName = redisSettings?.InstanceName;
        });

        services.AddScoped<IHorarioDisponivelService, HorarioDisponivelService>();
        services.AddScoped<IAgendamentoService, AgendamentoService>();


        return services;
    }


}
