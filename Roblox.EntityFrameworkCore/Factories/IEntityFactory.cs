using System;
using System.Collections.Generic;

namespace Roblox.EntityFrameworkCore.Factories
{
    /// <summary>
    /// An instantiable replacement for EntityHelper.
    /// Essentially a flexible Data Access Layer with room
    /// for interjection/customization. You can do caching, auditing, logging, etc. here
    /// </summary>
    public interface IEntityFactory<TEntity, TIndex>
    {
        /// <summary>
        /// Gets an entity by the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The entity.</returns>
        TEntity GetEntityByPredicate(Predicate<TEntity> predicate);

        /// <summary>
        /// Gets an entity by the given predicate or throws an exception.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The entity.</returns>
        /// <exception cref="InvalidOperationException">Unable to get entity.</exception>
        TEntity MustGetEntityByPredicate(Predicate<TEntity> predicate);

        /// <summary>
        /// Gets a collection of entities by the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A collection of entities.</returns>
        ICollection<TEntity> MultiGetEntityByPredicate(Predicate<TEntity> predicate);

        /// <summary>
        /// Gets a collection of entities by the given predicate.
        /// Also comes with paging support!
        /// </summary>
        /// <remarks>
        /// NOTE: Does not work with 64-bit IDs
        /// </remarks>
        /// <param name="predicate">The predicate.</param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns>A collection of entities.</returns>
        ICollection<TEntity> MultiGetEntityByPredicate(Predicate<TEntity> predicate, int startRowIndex, int maximumRows);

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

        /// <summary>
        /// Gets or creates the entity using <paramref name="dalGetter"/> and <paramref name="dalCreator"/>.
        /// </summary>
        /// <param name="dalGetter">The method to call to get the entity.</param>
        /// <param name="dalCreator">The method to call to create the entity.</param>
        /// <returns><paramref name="dalGetter"/> result if not null or <paramref name="dalCreator"/> result if not null respectively.</returns>
        TEntity GetOrCreateEntity(Func<TEntity> dalGetter, Func<TEntity> dalCreator);

        /// <summary>
        /// Gets all known <see cref="TEntity"/>s in the table.
        /// </summary>
        /// <returns>An <see cref="ICollection{TEntity}"/> of <see cref="TEntity"/>s</returns>
        ICollection<TEntity> GetAllEntities();

        /// <summary>
        /// Inserts or updates the given entity in the database.
        /// </summary>
        /// <param name="entity">The entity to save to the database.</param>
        /// <param name="dalInserter">The method to call before inserting the entity.</param>
        /// <param name="dalUpdater">The method to call before updating the entity.</param>
        void SaveEntity(TEntity entity, Action dalInserter, Action dalUpdater);

        /// <summary>
        /// Deletes the given entity from the database.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        void DeleteEntity(TEntity entity);
    }
}
