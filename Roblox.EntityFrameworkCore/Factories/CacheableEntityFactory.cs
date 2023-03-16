using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Roblox.Databases;

namespace Roblox.EntityFrameworkCore.Factories
{
    /// <summary>
    /// An entity factory that references queries against a cache.
    /// </summary>
    public class CacheableEntityFactory<TEntity, TIndex, TDatabase> : GenericEntityFactory<TEntity, TIndex, TDatabase>
        where TEntity : class, IRobloxEntity<TIndex>
        where TIndex : struct, IComparable<TIndex>
        where TDatabase : GlobalDatabase<TDatabase>, new()
    {
        #region | Data Access Methods |

        protected override void CreateEntity(TEntity entity)
        {
            base.CreateEntity(entity);
        }

        protected override T ReadEntity<T>(Func<DbSet<TEntity>, T> queryFunc)
        {
            return base.ReadEntity(queryFunc);
        }

        protected override void UpdateEntity(TEntity entity)
        {
            base.UpdateEntity(entity);
        }

        public override void DeleteEntity(TEntity entity)
        {
            base.DeleteEntity(entity);
        }

        public override TEntity GetEntityByPredicate(Func<TEntity, bool> predicate)
        {
            return base.GetEntityByPredicate(predicate);
        }

        public override TEntity MustGetEntityByPredicate(Func<TEntity, bool> predicate)
        {
            return base.MustGetEntityByPredicate(predicate);
        }

        public override ICollection<TEntity> MultiGetEntityByPredicate(Func<TEntity, bool> predicate)
        {
            return base.MultiGetEntityByPredicate(predicate);
        }

        public override TEntity GetEntity(TIndex id)
        {
            return base.GetEntity(id);
        }

        public override TEntity MustGetEntity(TIndex id)
        {
            return base.MustGetEntity(id);
        }

        public override ICollection<TEntity> GetAllEntities()
        {
            return base.GetAllEntities();
        }

        public override TEntity GetOrCreateEntity(Func<TEntity> dalGetter, Func<TEntity> dalCreator)
        {
            var result = dalGetter();

            if (result == null)
            {
                result = dalCreator();
            }

            return result;
        }

        public override void SaveEntity(TEntity entity, Action dalInserter, Action dalUpdater)
        {
            if (entity.ID.CompareTo(default) == 0)
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
    }
}


