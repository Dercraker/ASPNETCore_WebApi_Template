﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Template.Domain.Entities;
using Template.EFCore.Migrations;

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
                configuration.GetConnectionString("TEMPLATE_SQL")
            //, x => x.MigrationsAssembly(typeof(TemplateContext).Assembly.FullName)
            ), ServiceLifetime.Scoped);
    }

    /// <summary>
    /// Configures the database.
    /// </summary>
    /// <param name="services">The services.</param>
    public static async Task ConfigureDatabase(this IServiceProvider services)
    {
        using (IServiceScope serviceScope = services.CreateScope())
        {
            TemplateContext? context = serviceScope.ServiceProvider.GetService<TemplateContext>();
            if (context is null)
                throw new ArgumentNullException(nameof(context));
            RoleManager<IdentityRole<Guid>>? roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole<Guid>>>();
            if (roleManager is null)
                throw new ArgumentNullException(nameof(roleManager));
            UserManager<UserApi>? userManager = serviceScope.ServiceProvider.GetService<UserManager<UserApi>>();
            if (userManager is null)
                throw new ArgumentNullException(nameof(userManager));

            if (context.Database.IsRelational())
            {
                context?.Database.Migrate();
            }
            else
            {
                context.Database.EnsureCreated();
            }

            await DBInitializer.Initialize(context, userManager, roleManager);
        }
    }

    #endregion Methods
}
