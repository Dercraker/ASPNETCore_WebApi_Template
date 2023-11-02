using Microsoft.Extensions.DependencyInjection;
using Template.Platform.Interfaces;

namespace Template.Platform.Extensions;
public static class IServiceCollectionExtensions
{
    /// <summary>
    /// Adds the platforms.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>An IServiceCollection.</returns>
    public static IServiceCollection AddPlatforms(this IServiceCollection services)
    {
        services.AddScoped<IUserPlatform, UserPlatform>();
        services.AddScoped<ITodoTaskPlatform, TodoTaskPlatform>();

        return services;
    }
}
