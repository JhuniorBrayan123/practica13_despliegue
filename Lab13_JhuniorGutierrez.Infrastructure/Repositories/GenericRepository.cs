using Lab13_JhuniorGutierrez.Application.Interfaces.Repositories;
using Lab13_JhuniorGutierrez.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Lab13_JhuniorGutierrez.Infrastructure.Repositories;

public class GenericRepository<T>(Lab13_JhuniorGutierrezDbContext context) : IGenericRepository<T> where T : class
{
    protected readonly Lab13_JhuniorGutierrezDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(params object[] keyValues) => await DbSet.FindAsync(keyValues);

    public async Task<IReadOnlyList<T>> GetAllAsync() => await DbSet.AsNoTracking().ToListAsync();

    public async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

    public void Update(T entity) => DbSet.Update(entity);

    public void Remove(T entity) => DbSet.Remove(entity);
}
