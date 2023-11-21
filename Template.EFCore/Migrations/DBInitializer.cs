using Microsoft.AspNetCore.Identity;
using Template.Domain.Entities;
using Template.Domain.Settings;

namespace Template.EFCore.Migrations;
public class DBInitializer
{
    public static async Task<bool> Initialize(TemplateContext context, UserManager<UserApi> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        context.Database.EnsureCreated();


        if (context.Roles.Any() || context.Users.Any() || context.Tasks.Any()) return false;


        //Adding roles
        List<string> roles = Roles.GetAllRoles();

        foreach (string role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                IdentityResult resultAddRole = await roleManager.CreateAsync(new IdentityRole<Guid>(role));
                if (!resultAddRole.Succeeded)
                    throw new ApplicationException("Adding role '" + role + "' failed with error(s): " + resultAddRole.Errors);
            }
        }

        //Adding Admin

        string? userName = Environment.GetEnvironmentVariable("Username");
        if (userName is null)
            throw new ArgumentNullException(nameof(userName));

        string? email = Environment.GetEnvironmentVariable("Email");
        if (email is null)
            throw new ArgumentNullException(nameof(email));

        string? password = Environment.GetEnvironmentVariable("Password");
        if (password is null)
            throw new ArgumentNullException(nameof(password));

        UserApi admin = new UserApi
        {
            UserName = userName,
            Email = email,
            EmailConfirmed = true,
            CreatedAt = DateTime.Now,
        };


        IdentityResult? resultAddUser = await userManager.CreateAsync(admin, password);
        if (!resultAddUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' failed with error(s): " + string.Join(", ", resultAddUser.Errors));

        IdentityResult resultAddRoleToUser = await userManager.AddToRoleAsync(admin, Roles.Admin);
        if (!resultAddRoleToUser.Succeeded)
            throw new ApplicationException("Adding user '" + admin.UserName + "' to role '" + Roles.Admin + "' failed with error(s): " + string.Join(", ", resultAddRoleToUser.Errors));
        await context.SaveChangesAsync();


        //Adding Tasks
        IEnumerable<TodoTask> tasks = Enumerable.Range(0, 10).Select(i =>
        {
            DateTime startTime = Faker.Date.Forward();

            return new TodoTask()
            {
                TaskName = Faker.Lorem.Word(),
                Description = Faker.Lorem.Paragraph(1),
                IdUser = admin.Id,
                Start = startTime,
                End = startTime.AddDays(Faker.Number.RandomNumber(0, 365)),
            };
        }
        );

        context.Tasks.AddRange(tasks);

        await context.SaveChangesAsync();

        return true;
    }
}
