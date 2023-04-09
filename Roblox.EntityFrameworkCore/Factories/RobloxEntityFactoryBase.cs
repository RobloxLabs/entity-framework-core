using System;
using System.Collections.Generic;
using Roblox.Databases;

namespace Roblox.EntityFrameworkCore.Factories
{
    /// <summary>
    /// A base class for all RobloxEntity factories.
    /// An alternative (and instantiable) way to fetch entities.
    /// Intended to be inherited and modified to your heart's content.
    /// </summary>
    public abstract class RobloxEntityFactoryBase<TEntity, TIndex, TDatabase>
        where TEntity : RobloxEntity<TEntity, TIndex, TDatabase>, new()
        where TIndex : struct, IComparable<TIndex>
        where TDatabase : GlobalDatabase<TDatabase>, new()
    {
        protected readonly IEntityFactory<TEntity, TIndex> _Factory
            = new CacheableEntityFactory<TEntity, TIndex, TDatabase>();

        // NOTE: The "GetBy" methods are protected to force specific "GetBy[ColumnA]" methods to be defined in the entity's definition,
        // rather than calling GetBy(predicate) outside of the entity.
        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.GetEntityByPredicate"/>
        protected virtual TEntity GetBy(Predicate<TEntity> predicate)
        {
            return _Factory.GetEntityByPredicate(predicate);
        }

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.MustGetEntityByPredicate"/>
        protected virtual TEntity MustGetBy(Predicate<TEntity> predicate)
        {
            return _Factory.MustGetEntityByPredicate(predicate);
        }

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.MultiGetEntityByPredicate(Predicate{T})"/>
        protected virtual ICollection<TEntity> MultiGetBy(Predicate<TEntity> predicate)
        {
            return _Factory.MultiGetEntityByPredicate(predicate);
        }

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.MultiGetEntityByPredicate(Predicate{T}, int, int)"/>
        protected virtual ICollection<TEntity> MultiGetBy(Predicate<TEntity> predicate, int startRowIndex, int maximumRows)
        {
            return _Factory.MultiGetEntityByPredicate(
                predicate: predicate,
                startRowIndex: startRowIndex,
                maximumRows: maximumRows
            );
        }

        public virtual TEntity Get(TIndex id)
        {
            return _Factory.GetEntity(id);
        }

        public virtual TEntity MustGet(TIndex id)
        {
            return _Factory.MustGetEntity(id);
        }

        // NOTE: This is protected since it should only be called on by an entity or entity factory.
        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.GetOrCreateEntity"/>
        protected virtual TEntity GetOrCreate(Func<TEntity> dalGetter, Func<TEntity> dalCreator)
        {
            return _Factory.GetOrCreateEntity(dalGetter, dalCreator);
        }

        // NOTE: This is protected since not all entities should be able to have their entire tables fetched by default.
        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.GetAllEntities"/>
        protected virtual ICollection<TEntity> GetAll()
        {
            return _Factory.GetAllEntities();
        }

        // NOTE: This is protected so someone can't specify inserters and updaters outside an entity's definition.
        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.SaveEntity"/>
        protected virtual void Save(TEntity entity, Action dalInserter, Action dalUpdater)
        {
            _Factory.SaveEntity(entity, dalInserter, dalUpdater);
        }

        /// <summary>
        /// Saves the given entity to the database.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        public virtual void Save(TEntity entity)
        {
            _Factory.SaveEntity(
                entity,
                () =>
                {
                    entity.Created = DateTime.Now;
                    entity.Updated = entity.Created;
                },
                () =>
                {
                    entity.Updated = DateTime.Now;
                }
            );
        }

        public virtual void Delete(TEntity entity)
        {
            _Factory.DeleteEntity(entity);
        }
    }
}
