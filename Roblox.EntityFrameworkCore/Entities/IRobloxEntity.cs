using System;

namespace Roblox.EntityFrameworkCore
{
    /// <summary>
    /// New IRobloxEntity without the reliance on a DAL
    /// </summary>
    /// <typeparam name="TIndex"></typeparam>
    public interface IRobloxEntity<TIndex>
    {
        /// <summary>
        /// The timestamp to mark when the entity was created
        /// </summary>
        DateTime Created { get; }

        /// <summary>
        /// The timestamp to mark when the entity was last updated
        /// </summary>
        DateTime Updated { get; }

        /// <summary>
        /// The ID of the entity
        /// </summary>
        TIndex ID { get; }
    }

    /// <inheritdoc/>
    public interface IRobloxEntity<TEntity, TIndex> : IRobloxEntity<TIndex>
    {
        // C# 11.0
#if NET7_0_OR_GREATER
        public static abstract TEntity Get(TIndex id);
#endif
    }
}
