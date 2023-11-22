namespace application_pkg;

public static class ConfigureService
{
    public static IServiceCollection InjectCached(this IServiceCollection services, string stringConnection)
    {

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = stringConnection;
        });

        services.AddScoped(typeof(ICachedService<>), typeof(CachedService<>));

        return services;
    }
}