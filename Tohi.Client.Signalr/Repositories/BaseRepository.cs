using Microsoft.EntityFrameworkCore;

namespace Tohi.Client.Signalr.Repositories
{
    public interface IRepo<TEntity> where TEntity : class
    {
        /// <summary>
        /// Truy vấn dữ liệu database
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> UseQueries();
    }
    public class BaseRepository<TEntity> : IRepo<TEntity> where TEntity : class
    {
        private readonly DbContext _context;
        private DbSet<TEntity> _dbset;

        public BaseRepository(DbContext context)
        {
            _context = context;
            _dbset = context.Set<TEntity>();
        }

        public IQueryable<TEntity> UseQueries()
        {
            return _dbset.AsQueryable();
        }
    }
}
