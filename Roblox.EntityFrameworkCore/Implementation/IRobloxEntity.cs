namespace Roblox.EntityFrameworkCore
{
    /// <summary>
    /// New IRobloxEntity without the reliance on a DAL
    /// </summary>
    /// <typeparam name="TIndex"></typeparam>
    public interface IRobloxEntity<TIndex>
    {
        public TIndex ID { get; }
    }

    /// <inheritdoc/>
    public interface IRobloxEntity<TEntity, TIndex> : IRobloxEntity<TIndex>
    {
        public static abstract TEntity Get(TIndex id);
    }
}
