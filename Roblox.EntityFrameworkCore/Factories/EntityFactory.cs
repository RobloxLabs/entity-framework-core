using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Roblox.Databases;

namespace Roblox.EntityFrameworkCore.Factories
{
    /// <inheritdoc cref="IEntityFactory{TEntity, TIndex}"/>
    public class EntityFactory<TEntity, TIndex, TDatabase> : IEntityFactory<TEntity, TIndex>
        where TEntity : class, IRobloxEntity<TIndex>
        where TIndex : struct, IEquatable<TIndex>
        where TDatabase : GlobalDatabase<TDatabase>, new()
    {
        #region | Data Access Methods |

        protected virtual void CreateEntity(TEntity entity)
        {
            using (var db = GetDbContext())
            {
                db.Table.Add(entity);
                db.SaveChanges();
            }
        }

        protected virtual T QueryData<T>(Func<DbSet<TEntity>, T> queryFunc)
        {
            T result = default;

            using (var db = GetDbContext())
            {
                result = queryFunc(db.Table);
            }

            return result;
        }

        protected virtual void UpdateEntity(TEntity entity)
        {
            using (var db = GetDbContext())
            {
                db.Table.Update(entity);
                db.SaveChanges();
            }
        }

        public virtual void DeleteEntity(TEntity entity)
        {
            using (var db = GetDbContext())
            {
                db.Table.Remove(entity);
                db.SaveChanges();
            }
        }

        public virtual TEntity GetEntityByPredicate(Predicate<TEntity> predicate)
        {
            var pred = new Func<TEntity, bool>(predicate);
            return QueryData(
                (table) => table.FirstOrDefault(pred)
            );
        }

        public virtual TEntity MustGetEntityByPredicate(Predicate<TEntity> predicate)
        {
            return GetEntityByPredicate(predicate) ?? throw new InvalidOperationException($"Failed to load {typeof(TEntity).Name}.");
        }

        public virtual ICollection<TEntity> MultiGetEntityByPredicate(Predicate<TEntity> predicate)
        {
            var pred = new Func<TEntity, bool>(predicate);
            return QueryData(
                (table) => table.Where(pred)
                .ToList()
            );
        }

        public virtual ICollection<TEntity> MultiGetEntityByPredicate(Predicate<TEntity> predicate, int startRowIndex, int maximumRows)
        {
            var pred = new Func<TEntity, bool>(predicate);
            return QueryData(
                (table) => table.Where(pred)
                .Skip(startRowIndex)
                .Take(maximumRows)
                .ToList()
            );
        }

        public virtual TEntity GetEntity(TIndex id)
        {
            return GetEntityByPredicate(entity => entity.ID.Equals(id));
        }

        public virtual TEntity MustGetEntity(TIndex id)
        {
            return MustGetEntityByPredicate(entity => entity.ID.Equals(id));
        }

        public virtual ICollection<TEntity> GetAllEntities()
        {
            return QueryData((table) => table.ToList());
        }

        public virtual ICollection<TEntity> GetAllEntities(int startRowIndex, int maximumRows)
        {
            return QueryData(
                (table) => table
                .Skip(startRowIndex)
                .Take(maximumRows)
                .ToList()
            );
        }

        public virtual int GetEntityCount()
        {
            return QueryData(
                (table) => table.Count()
            );
        }

        public virtual int GetEntityCountByPredicate(Predicate<TEntity> predicate)
        {
            var pred = new Func<TEntity, bool>(predicate);
            return QueryData(
                (table) => table.Where(pred)
                .Count()
            );
        }

        public virtual TEntity GetOrCreateEntity(Func<TEntity> dalGetter, Func<TEntity> dalCreator)
        {
            var result = dalGetter();

            if (result == null)
            {
                result = dalCreator();
            }

            return result;
        }

        public virtual void SaveEntity(TEntity entity, Action dalInserter, Action dalUpdater)
        {
            if (entity.ID.Equals(default))
            {
                dalInserter();
                CreateEntity(entity);
            }
            else
            {
                dalUpdater();
                UpdateEntity(entity);
            }
        }

        #endregion | Data Access Methods |

        protected static RobloxDbContext<TEntity, TIndex, TDatabase> GetDbContext()
            => new RobloxDbContext<TEntity, TIndex, TDatabase>();
    }
}


