using Microsoft.EntityFrameworkCore;
using StudentManagement.Data.Context;

namespace StudentManagement.Data.UnitOfWork;

// ─── GENERIC REPOSITORY ───────────────────────────────────────────────────────
public interface IGenericRepository<T> where T : class
{
    Task<T?> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task AddAsync(T entity);
    void Update(T entity);
    void Remove(T entity);
}

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    protected readonly ApplicationDbContext _db;
    protected readonly DbSet<T> _set;

    public GenericRepository(ApplicationDbContext db)
    {
        _db  = db;
        _set = db.Set<T>();
    }

    public async Task<T?> GetByIdAsync(int id)         => await _set.FindAsync(id);
    public async Task<IEnumerable<T>> GetAllAsync()    => await _set.ToListAsync();
    public async Task AddAsync(T entity)               => await _set.AddAsync(entity);
    public void Update(T entity)                       => _set.Update(entity);
    public void Remove(T entity)                       => _set.Remove(entity);
}

// ─── UNIT OF WORK ─────────────────────────────────────────────────────────────
public interface IUnitOfWork : IDisposable
{
    Task<int> SaveChangesAsync();
    Task BeginTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _db;
    private Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction? _tx;

    public UnitOfWork(ApplicationDbContext db) => _db = db;

    public async Task<int> SaveChangesAsync()    => await _db.SaveChangesAsync();
    public async Task BeginTransactionAsync()    => _tx = await _db.Database.BeginTransactionAsync();
    public async Task CommitAsync()              { if (_tx != null) await _tx.CommitAsync(); }
    public async Task RollbackAsync()            { if (_tx != null) await _tx.RollbackAsync(); }

    public void Dispose() { _tx?.Dispose(); _db.Dispose(); }
}
