using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;
using Template.API.AutoMapperProfiles;
using Template.API.Validator.TodoTask;
using Template.API.Validator.User;
using Template.Domain.Dto.TodoTask;
using Template.Domain.Dto.User;
using Template.Domain.Entities;
using Template.Domain.Settings;
using Template.EFCore;
using Template.EFCore.Extensions;
using Template.Platform.Extensions;
using Template.Provider.Extensions;
using Task = System.Threading.Tasks.Task;

namespace Template.API.Extensions;

public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Configures the swagger.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            string? API_NAME = Assembly.GetExecutingAssembly().GetName().Name;
            string xmlPath = $"{AppContext.BaseDirectory}{API_NAME}.xml";

            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Version = "v1",
                Title = API_NAME,
                Description = "Template API",
            });
            c.IncludeXmlComments(xmlPath);
        });
    }

    /// <summary>
    /// Configures the cors.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void ConfigureCors(this IServiceCollection services, ConfigurationManager configuration)
    {
        List<string> originsAllowed = configuration.GetSection("CallsOrigins").Get<List<string>>()!;
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(builder =>
            {
                builder
                       .WithOrigins(originsAllowed.ToArray())
                       .WithMethods("PUT", "DELETE", "GET", "OPTIONS", "POST")
                       .AllowAnyHeader()
                       .Build();
            });
        });
    }

    /// <summary>
    /// Adds the auto mapper configuration.
    /// </summary>
    /// <param name="services">The services.</param>
    public static void AddAutoMapperConfiguration(this IServiceCollection services) => services.AddAutoMapper(typeof(UserProfiles),
                                                                                                              typeof(TodoTaskProfiles));

    /// <summary>
    /// Adds the services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration">The configuration.</param>
    public static void AddServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.AddPlatforms()
            .AddProviders()
            .AddValidators()
            .AddRepositories();

        services.AddSingleton(configuration.GetSection("JWTSettings").Get<JWTSettings>());

        services.AddControllers();
    }

    /// <summary>
    /// Adds the validators.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <returns>An IServiceCollection.</returns>
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        #region UserValidator
        services.AddScoped<IValidator<RegisterUserDto>, RegisterUserValidator>();
        services.AddScoped<IValidator<LoginUserDto>, LoginUserValidator>();
        services.AddScoped<IValidator<ForgotPasswordDto>, ForgotPasswordValidator>();
        services.AddScoped<IValidator<ResetPasswordDto>, ResetPasswordValidator>();
        #endregion

        #region TodoTaskValidator
        services.AddScoped<IValidator<CreateTodoTaskDto>, CreateTodoTaskValidator>();
        services.AddScoped<IValidator<UpdateTodoTaskDto>, UpdateTodoTaskValidator>();

        #endregion

        return services;
    }


    public static void AddJWT(this IServiceCollection services, ConfigurationManager configuration)
    {
        JWTSettings? jwtSettings = configuration.GetSection("JWTSettings").Get<JWTSettings>();
        services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireAudience = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
                ValidateLifetime = true,
                RoleClaimType = "Roles",
                NameClaimType = "UserName",

            };
        });
        services.AddAuthorization(options =>
        {
            options.DefaultPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .AddAuthenticationSchemes("Bearer")
                .Build();
        });
    }
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<UserApi, IdentityRole<Guid>>(options =>
        {
            options.SignIn.RequireConfirmedAccount = false;

            options.ClaimsIdentity.UserIdClaimType = "Username";
            options.ClaimsIdentity.EmailClaimType = "Email";

            //Password requirement
            options.Password.RequireDigit = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
            options.Password.RequiredUniqueChars = 4; //Determine le nombre de caract�re unnique minimum requis


            //Lockout if passwd fails 5 times then count blocks 60 min
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(60);
            options.Lockout.AllowedForNewUsers = true;

            //User
            options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
            options.User.RequireUniqueEmail = true;
        })
        .AddDefaultTokenProviders()
        .AddRoles<IdentityRole<Guid>>()
        .AddEntityFrameworkStores<TemplateContext>();


        services.ConfigureApplicationCookie(options =>
        {
            options.Events.OnRedirectToLogin = context =>
            {
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };
        });

    }
}
