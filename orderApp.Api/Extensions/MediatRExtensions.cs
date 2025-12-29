using OrderApp.Application.Mappings;

namespace OrderApp.Api.Extensions;

public static class MediatRExtensions
{
    public static IServiceCollection AddMediatRServices(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(MediatRExtensions).Assembly);
        });

        return services;
    }
}