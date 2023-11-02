using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Template.EFCore.IOC;
public static class ServiceCollectionExtensions
{
    #region Methods

    /// <summary>
    /// Defining the connection string for TemplateContext
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddTemplateContext(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddDbContext<TemplateContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("TEMPLATE_SQL"),
                x => x.MigrationsAssembly(typeof(TemplateContext).Assembly.FullName)));
    }

    /// <summary>
    /// Configures the database.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void ConfigureDatabase(this IServiceProvider services)
    {
        using (IServiceScope serviceScope = services.CreateScope())
        {
            TemplateContext? context = serviceScope.ServiceProvider.GetService<TemplateContext>();
            if (context != null)
            {
                if (context.Database.IsRelational())
                {
                    context?.Database.Migrate();
                }
                else
                {
                    context.Database.EnsureCreated();
                }
            }
        }
    }

    #endregion Methods
}
