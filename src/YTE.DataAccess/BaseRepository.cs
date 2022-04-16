using System.Linq;
using YTE.Common;
using YTE.Entities.Context;

namespace YTE.DataAccess
{
    public class BaseRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntity
    {
        private readonly YTEContext Context;
        public BaseRepository(YTEContext context)
        {
            this.Context = context;
        }

        public void Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
        }

        public IQueryable<TEntity> Get()
        {
            return Context.Set<TEntity>().AsQueryable();
        }

        public TEntity Insert(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            return entity;

        }

        public TEntity Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);

            return entity;
        }
    }
}
