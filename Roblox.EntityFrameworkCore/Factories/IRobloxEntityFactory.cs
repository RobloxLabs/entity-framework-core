using System;
using System.Collections.Generic;

namespace Roblox.EntityFrameworkCore.Factories
{
    /// <summary>
    /// An alternative (and instantiable) way to fetch entities.
    /// Intended to be inherited and modified to your heart's content.
    /// </summary>
    public interface IRobloxEntityFactory<TEntity, TIndex>
    {
        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.GetEntityByPredicate"/>
        TEntity GetBy(Func<TEntity, bool> predicate);

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.MustGetEntityByPredicate"/>
        TEntity MustGetBy(Func<TEntity, bool> predicate);

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.MultiGetEntityByPredicate"/>
        ICollection<TEntity> MultiGetBy(Func<TEntity, bool> predicate);

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.GetEntity"/>
        TEntity Get(TIndex id);

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.MustGetEntity"/>
        TEntity MustGet(TIndex id);

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.GetOrCreateEntity"/>
        TEntity GetOrCreate(Func<TEntity> dalGetter, Func<TEntity> dalCreator);

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.GetAllEntities"/>
        ICollection<TEntity> GetAll();

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.SaveEntity"/>
        void Save(TEntity entity, Action dalInserter, Action dalUpdater);

        /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}.DeleteEntity"/>
        void Delete(TEntity entity);
    }
}
