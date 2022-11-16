using SmartChargingPoC.Core.Entities.Base;
using SmartChargingPoC.DataAccess.Contexts;
using SmartChargingPoC.DataAccess.Repositories.Base;

namespace SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Base
{
    public class SmartChargingRepositoryBase<TEntity> : RepositoryBase<TEntity, SmartChargingContext>, ISmartChargingRepositoryBase<TEntity>
        where TEntity : class, IEntity
    {
        public SmartChargingRepositoryBase(SmartChargingContext context)
            : base(context)
        {
        }
    }
}
