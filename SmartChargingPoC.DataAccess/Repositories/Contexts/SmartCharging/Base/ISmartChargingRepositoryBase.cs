using SmartChargingPoC.Core.Entities.Base;
using SmartChargingPoC.DataAccess.Repositories.Base;

namespace SmartChargingPoC.DataAccess.Repositories.Contexts.SmartCharging.Base
{
    public interface ISmartChargingRepositoryBase<TEntity> : IRepositoryBase<TEntity>
        where TEntity : class, IEntity
    {
    }
}
