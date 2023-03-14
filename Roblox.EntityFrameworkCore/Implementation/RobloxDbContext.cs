using Microsoft.EntityFrameworkCore;
using Roblox.Databases;

namespace Roblox.EntityFrameworkCore
{
    public class RobloxDbContext<TEntity, TIndex, TDatabase> : DbContext
        where TEntity : class, IRobloxEntity<TIndex>
        where TDatabase : IGlobalDatabase // (has static ConnectionString property)
    {
        private IGlobalDatabase _Database;

        public DbSet<TEntity> Table { get; set; }


        public RobloxDbContext()
        {
            _Database = TDatabase.Singleton;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var db = _Database;
            switch (db.DbType)
            {
                case DatabaseType.Mssql:
                    // HACK: MAJOR SSL HACK PLEASE FIX
                    options.UseSqlServer(db.ConnectionString + ";TrustServerCertificate=True");
                    break;
                case DatabaseType.Sqlite:
                    options.UseSqlite(db.ConnectionString);
                    break;
                case DatabaseType.MySql:
                    options.UseMySql(db.ConnectionString);
                    break;
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TEntity>();

            var db = _Database;
            switch (db.DbType)
            {
                case DatabaseType.Mssql:
                    entity.Property(e => e.ID).UseIdentityColumn();
                    break;
                default:
                    /* NOTE: Generates some sub-optimal SQL (MySQL):
                      INSERT INTO `CreatorTypes` (`Created`, `Updated`, `Value`)
                      VALUES (@p0, @p1, @p2);
                      SELECT `ID`
                      FROM `CreatorTypes`
                      WHERE ROW_COUNT() = 1 AND `ID` = LAST_INSERT_ID();
                     */
                    entity.Property(e => e.ID).ValueGeneratedOnAdd();
                    break;
            }

            // Should ideally make the table name $"{nameof(TEntity)}s" if it's not already set in the entity
            var tableName = entity.Metadata.GetTableName();
            if (string.IsNullOrEmpty(tableName) || tableName == nameof(Table))
            {
                var tableNameSuffix = "s";
                tableName = entity.Metadata.ClrType.Name + tableNameSuffix;

                entity.ToTable(tableName);
            }
        }
    }
}