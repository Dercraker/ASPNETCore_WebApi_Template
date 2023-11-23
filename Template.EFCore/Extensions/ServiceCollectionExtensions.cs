using Microsoft.Extensions.DependencyInjection;
using Template.EFCore.Interfaces;
using Template.EFCore.Repositories;

namespace Template.EFCore.Extensions;
public static class IServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();

        return services;
    }
}
