using System;
using System.Collections.Generic;

namespace Roblox.EntityFrameworkCore.Factories
{
    /// <summary>
    /// A base class for RobloxEntity factories
    /// </summary>
    public class RobloxEntityFactory<TEntity, TIndex> : IRobloxEntityFactory<TEntity, TIndex>
    {
        protected IEntityFactory<TEntity, TIndex> _Factory;

        public RobloxEntityFactory(IEntityFactory<TEntity, TIndex> genericEntityFactory)
        {
            _Factory = genericEntityFactory;
        }

        public virtual TEntity GetBy(Func<TEntity, bool> predicate)
        {
            return _Factory.GetEntityByPredicate(predicate);
        }

        public virtual TEntity MustGetBy(Func<TEntity, bool> predicate)
        {
            return _Factory.MustGetEntityByPredicate(predicate);
        }

        public virtual ICollection<TEntity> MultiGetBy(Func<TEntity, bool> predicate)
        {
            return _Factory.MultiGetEntityByPredicate(predicate);
        }

        public virtual TEntity Get(TIndex id)
        {
            return _Factory.GetEntity(id);
        }

        public virtual TEntity MustGet(TIndex id)
        {
            return _Factory.MustGetEntity(id);
        }

        public virtual TEntity GetOrCreate(Func<TEntity> dalGetter, Func<TEntity> dalCreator)
        {
            return _Factory.GetOrCreateEntity(dalGetter, dalCreator);
        }

        public virtual ICollection<TEntity> GetAll()
        {
            return _Factory.GetAllEntities();
        }

        public virtual void Save(TEntity entity, Action dalInserter, Action dalUpdater)
        {
            _Factory.SaveEntity(entity, dalInserter, dalUpdater);
        }

        public virtual void Delete(TEntity entity)
        {
            _Factory.DeleteEntity(entity);
        }
    }
}
