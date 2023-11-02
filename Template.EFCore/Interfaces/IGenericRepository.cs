using System.Linq.Expressions;

namespace Template.EFCore.Interfaces;

public interface IGenericRepository<T> where T : class
{
    /// <summary>
    /// Gets T by id.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>A     T? .</returns>
    T? GetById(Guid id);

    /// <summary>
    /// Gets T all.
    /// </summary>
    /// <returns>A list of TS.</returns>
    IQueryable<T> GetAll();

    /// <summary>
    /// Gets the all async.
    /// </summary>
    /// <returns>A Task.</returns>
    Task<List<T>> GetAllAsync();

    /// <summary>
    /// Gets T by id async.
    /// </summary>
    /// <param name="id">The id.</param>
    /// <returns>A Task.</returns>
    Task<T?> GetByIdAsync(Guid id);

    /// <summary>
    /// Finds T.
    /// </summary>
    /// <param name="expression">The expression.</param>
    /// <returns>A list of TS.</returns>
    IList<T> Find(Expression<Func<T, bool>> expression);

    /// <summary>
    /// Adds T.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Add(T entity);

    /// <summary>
    /// Adds T async.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns>A Task.</returns>
    Task AddAsync(T entity);

    /// <summary>
    /// Adds T range.
    /// </summary>
    /// <param name="entities">The entities.</param>
    void AddRange(IList<T> entities);

    /// <summary>
    /// Adds T range async.
    /// </summary>
    /// <param name="entities">The entities.</param>
    /// <returns>A Task.</returns>
    Task AddRangeAsync(IList<T> entities);

    /// <summary>
    /// Removes type T.
    /// </summary>
    /// <param name="entity">The entity.</param>
    void Remove(T entity);

    /// <summary>
    /// Removes type T range.
    /// </summary>
    /// <param name="entities">The entities.</param>
    void RemoveRange(IList<T> entities);

    /// <summary>
    /// Removes type T all.
    /// </summary>
    void RemoveAll();
}