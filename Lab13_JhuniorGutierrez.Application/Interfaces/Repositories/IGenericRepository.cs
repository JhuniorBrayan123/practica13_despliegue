namespace Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(params object[] keyValues);
    Task<IReadOnlyList<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}
