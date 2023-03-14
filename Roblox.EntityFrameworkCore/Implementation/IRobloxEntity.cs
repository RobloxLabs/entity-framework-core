namespace Roblox.EntityFrameworkCore
{
    /// <summary>
    /// New IRobloxEntity without the reliance on a DAL
    /// </summary>
    /// <typeparam name="TIndex"></typeparam>
    public interface IRobloxEntity<TIndex>
    {
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
