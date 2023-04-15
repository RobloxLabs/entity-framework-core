using System;
using System.Collections.Generic;
using Roblox.Databases;
using Roblox.EntityFrameworkCore.Factories;

namespace Roblox.EntityFrameworkCore.Entities
{
    /// <summary>
    /// A version of RobloxEntityFactory that's intended to only be used by RobloxEntity.
    /// Aka <see cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}"/> but all methods are public.
    /// </summary>
    /// <remarks>
    /// The following is the problem this was created to solve:<br/>
    /// We need to have an EntityHelper-type class, as well as an EntityFactory class for the RobloxEntity base class to use.
    /// Finally, we need a clean and intentionally restricted version of the EntityFactory class to act as a
    /// base class for custom RobloxEntityFactories.
    /// So far, C# has been making this hell...
    /// Also, it might be possible to define internal public versions of these methods inside the original base class.
    /// </remarks>
    internal sealed class InternalRobloxEntityFactory<TEntity, TIndex, TDatabase> : RobloxEntityFactoryBase<TEntity, TIndex, TDatabase>
        where TEntity : RobloxEntity<TEntity, TIndex, TDatabase>, new()
        where TIndex : struct, IComparable<TIndex>
        where TDatabase : GlobalDatabase<TDatabase>, new()
    {
        public InternalRobloxEntityFactory()
        {

        }

        public InternalRobloxEntityFactory(IEntityFactory<TEntity, TIndex> factory)
        {
            _Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.GetBy"/>
        public new TEntity GetBy(Predicate<TEntity> predicate)
        {
            return base.GetBy(predicate);
        }

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.MustGetBy"/>
        public new TEntity MustGetBy(Predicate<TEntity> predicate)
        {
            return base.MustGetBy(predicate);
        }

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.MultiGetBy(Predicate{T})"/>
        public new ICollection<TEntity> MultiGetBy(Predicate<TEntity> predicate)
        {
            return base.MultiGetBy(predicate);
        }

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.MultiGetBy(Predicate{T}, int, int"/>
        public new ICollection<TEntity> MultiGetBy(Predicate<TEntity> predicate, int startRowIndex, int maximumRows)
        {
            return base.MultiGetBy(
                predicate: predicate,
                startRowIndex: startRowIndex,
                maximumRows: maximumRows
            );
        }

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.GetOrCreate"/>
        public new TEntity GetOrCreate(Func<TEntity> dalGetter, Func<TEntity> dalCreator)
        {
            return base.GetOrCreate(dalGetter, dalCreator);
        }

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.GetAll"/>
        public new ICollection<TEntity> GetAll()
        {
            return base.GetAll();
        }

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.Save(TEntity, Action, Action)"/>
        public new void Save(TEntity entity, Action dalInserter, Action dalUpdater)
        {
            base.Save(entity, dalInserter, dalUpdater);
        }
    }
}
