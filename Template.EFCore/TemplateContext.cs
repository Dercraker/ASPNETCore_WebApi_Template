using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Template.Domain.Entities;
using TodoTask = Template.Domain.Entities.TodoTask;

namespace Template.EFCore;
public class TemplateContext : IdentityDbContext<UserApi, IdentityRole<Guid>, Guid>
{
    #region Props
    /// <summary>
    /// Task table definition with task class structure
    /// </summary>
    public DbSet<TodoTask> Tasks { get; set; }
    #endregion

    #region CTOR
    public TemplateContext()
    {

    }

    public TemplateContext(DbContextOptions<TemplateContext> options) : base(options)
    {
        Database.Migrate();
    }
    #endregion

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<UserApi>(u =>
        {
            u.ToTable(u => u.IsTemporal()).HasKey(u => u.Id);
            u.HasMany(u => u.Tasks).WithOne(t => t.User).HasForeignKey(t => t.IdUser).OnDelete(DeleteBehavior.NoAction);
        });

        builder.Entity<TodoTask>(t =>
        {
            t.ToTable(name: "Task", t => t.IsTemporal()).HasKey(t => t.IdTask);
        });
    }
}
