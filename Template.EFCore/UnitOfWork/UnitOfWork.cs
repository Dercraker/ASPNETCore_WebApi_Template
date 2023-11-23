using Template.EFCore.Interfaces;

namespace Template.EFCore.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    #region Properties

    private readonly TemplateContext _context;
    public ITodoTaskRepository TodoTasks { get; private set; }

    #endregion Properties

    #region Constructor

    public UnitOfWork(TemplateContext context, ITodoTaskRepository todoTasks)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        TodoTasks = todoTasks ?? throw new ArgumentNullException(nameof(todoTasks));
    }

    #endregion Constructor

    /// <inheritdoc/>
    public int Complete() => _context.SaveChanges();

    /// <inheritdoc/>
    public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();

    /// <inheritdoc/>
    public void Dispose() => _context.Dispose();

    /// <inheritdoc/>
    public async Task DisposeAsync() => await _context.DisposeAsync();
}