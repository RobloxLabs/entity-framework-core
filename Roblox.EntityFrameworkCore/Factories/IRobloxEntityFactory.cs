namespace Roblox.EntityFrameworkCore.Factories
{
    public interface IRobloxEntityFactory<TEntity, TIndex>
    {
        TEntity Get(TIndex id);

        TEntity MustGet(TIndex id);

        /// <summary>
        /// Saves the given entity to the database.
        /// </summary>
        /// <param name="entity">The entity to save.</param>
        void Save(TEntity entity);

        void Delete(TEntity entity);
    }
}
