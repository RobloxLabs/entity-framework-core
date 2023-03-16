using System;
using System.Collections.Generic;

namespace Roblox.EntityFrameworkCore.Factories
{
    /// <summary>
    /// An instantiable replacement for EntityHelper.
    /// Essentially a flexible Data Access Layer with room
    /// for interjection. You can do caching, auditing, logging, etc. here
    /// </summary>
    public interface IEntityFactory<TEntity, TIndex>
    {
        TEntity GetEntityByPredicate(Func<TEntity, bool> predicate);

        TEntity MustGetEntityByPredicate(Func<TEntity, bool> predicate);

        ICollection<TEntity> MultiGetEntityByPredicate(Func<TEntity, bool> predicate);

        /// <summary>
        /// Gets the entity associated with the given ID.
        /// </summary>
        /// <param name="id">The ID to fetch the entity by</param>
        /// <returns>The entity associated with the given ID</returns>
        TEntity GetEntity(TIndex id);

        /// <summary>
        /// Gets the entity associated with the given ID, or throws an exception.
        /// </summary>
        /// <param name="id">The ID to fetch the entity by</param>
        /// <returns>The entity associated with the given ID</returns>
        /// <exception cref="InvalidOperationException">The entity with the given ID doesn't exist</exception>
        TEntity MustGetEntity(TIndex id);

        TEntity GetOrCreateEntity(Func<TEntity> dalGetter, Func<TEntity> dalCreator);

        ICollection<TEntity> GetAllEntities();

        void SaveEntity(TEntity entity, Action dalInserter, Action dalUpdater);

        void DeleteEntity(TEntity entity);
    }
}
