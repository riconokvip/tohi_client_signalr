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

        /// <summary>
        /// Cập nhật dữ liệu vào database
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(TEntity entity);
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

        public async Task Update(TEntity entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
