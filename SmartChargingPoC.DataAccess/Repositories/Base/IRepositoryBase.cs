using SmartChargingPoC.Core.Entities.Base;
using System.Linq.Expressions;

namespace SmartChargingPoC.DataAccess.Repositories.Base
{
    public interface IRepositoryBase<TEntity>
        where TEntity : class, IEntity
    {
        IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>>? filter = null);
        TEntity GetIncludeBy(int id, params Expression<Func<TEntity, object>>[] includeExpressions);
        bool Any(int id);
        TEntity GetById(int id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
    }
}