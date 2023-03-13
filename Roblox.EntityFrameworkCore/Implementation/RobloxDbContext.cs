using Microsoft.EntityFrameworkCore;
using Roblox.Databases;

namespace Roblox.EntityFrameworkCore
{
    public class RobloxDbContext<TEntity, TDatabase> : DbContext
        where TEntity : class
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
                    options.UseMySQL(db.ConnectionString);
                    break;
            }
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var entity = modelBuilder.Entity<TEntity>();

            entity.Property("Created").HasDefaultValueSql("getdate()");
            entity.Property("Updated").HasDefaultValueSql("getdate()");
            
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