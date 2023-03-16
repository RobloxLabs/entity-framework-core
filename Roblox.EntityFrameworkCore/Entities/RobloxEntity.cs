using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Roblox.Databases;
using Roblox.EntityFrameworkCore.Factories;

namespace Roblox.EntityFrameworkCore
{
    public abstract class RobloxEntity<TEntity, TIndex, TDatabase> : IRobloxEntity<TEntity, TIndex>
        where TEntity : RobloxEntity<TEntity, TIndex, TDatabase>, new()
        where TIndex : struct, IComparable<TIndex>
        where TDatabase : GlobalDatabase<TDatabase>, new()
    {
        #region | Entity Properties |

        /// <summary>
        /// The timestamp to mark when the entity was created
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// The timestamp to mark when the entity was last updated
        /// </summary>
        public DateTime Updated { get; private set; }

        /// <summary>
        /// The ID of the entity
        /// </summary>
        /// <remarks>
        /// Put below Created & Updated to have serialized versions of
        /// the entity look less bad.
        /// </remarks>
        [Key]
        public TIndex ID { get; private set; }

        #endregion | Entity Properties |

        #region | Entity Methods |

        public virtual void Save()
        {
            _Factory.Save(
                (TEntity)this,
                () =>
                {
                    this.Created = DateTime.Now;
                    this.Updated = this.Created;
                },
                () =>
                {
                    this.Updated = DateTime.Now;
                }
            );
        }

        public virtual void Delete()
        {
            _Factory.Delete((TEntity)this);
        }

        #endregion | Entity Methods |

        #region | Data Access Methods |

        /// <summary>
        /// Gets the entity associated with the given ID.
        /// </summary>
        /// <param name="id">The ID to fetch the entity by</param>
        /// <returns>The entity associated with the given ID</returns>
        public static TEntity Get(TIndex id)
        {
            return _Factory.Get(id);
        }

        /// <inheritdoc cref="Get(TIndex)"/>
        public static TEntity Get(TIndex? id)
        {
            if (id.HasValue)
                return Get(id.Value);
            return null;
        }

        /// <summary>
        /// Gets the entity associated with the given ID, or throws an exception.
        /// </summary>
        /// <param name="id">The ID to fetch the entity by</param>
        /// <returns>The entity associated with the given ID</returns>
        /// <exception cref="InvalidOperationException">The entity with the given ID doesn't exist</exception>
        public static TEntity MustGet(TIndex id)
        {
            return _Factory.MustGet(id);
        }

        public static TEntity GetBy(Func<TEntity, bool> predicate)
        {
            return _Factory.GetBy(predicate);
        }

        protected static TEntity GetOrCreate(Func<TEntity> dalGetter, Func<TEntity> dalCreator)
        {
            return _Factory.GetOrCreate(dalGetter, dalCreator);
        }

        // Sorry for the inconsistent naming scheme, but we need this to not be public by default,
        // and to not conflict with EnumeratorEntity.GetAll()
        protected static ICollection<TEntity> DoGetAll()
        {
            return _Factory.GetAll();
        }

        #endregion | Data Access Methods |

        private static readonly IEntityFactory<TEntity, TIndex> _Helper
            = new CacheableEntityFactory<TEntity, TIndex, TDatabase>();

        private static readonly IRobloxEntityFactory<TEntity, TIndex> _Factory
            = new RobloxEntityFactory<TEntity, TIndex>(_Helper);
    }
}