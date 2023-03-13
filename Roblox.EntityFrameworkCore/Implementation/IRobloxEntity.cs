namespace Roblox.EntityFrameworkCore
{
    /// <summary>
    /// New IRobloxEntity without the reliance on a DAL
    /// </summary>
    /// <typeparam name="TIndex"></typeparam>
    public interface IRobloxEntity<TEntity, TIndex>
    {
        public TIndex ID { get; }

        public static abstract TEntity Get(TIndex id);
    }
}
