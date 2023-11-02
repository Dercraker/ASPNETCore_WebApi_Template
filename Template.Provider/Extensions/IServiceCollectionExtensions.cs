using Microsoft.Extensions.DependencyInjection;
using Template.Provider.Interfaces;

namespace Template.Provider.Extensions;
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the platforms.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>An IServiceCollection.</returns>
    public static IServiceCollection AddProviders(this IServiceCollection services)
    {
        services.AddScoped<ITodoTaskProvider, TodoTaskProvider>();

        return services;
    }
}