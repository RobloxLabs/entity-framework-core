using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Roblox.Databases;
using Roblox.EntityFrameworkCore.Entities;
using Roblox.EntityFrameworkCore.Factories;

namespace Roblox.EntityFrameworkCore
{
    /// <summary>
    /// A base class for all ROBLOX entities that reduces the absurd amount
    /// of boilerplate code of the old system.
    /// </summary>
    /// <remarks>
    /// Acts as a static wrapper of the <see cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}"/> class.
    /// </remarks>
    /// <typeparam name="TEntity">The entity's type.</typeparam>
    /// <typeparam name="TIndex">The type to use for the entity's index.</typeparam>
    /// <typeparam name="TDatabase">The database to use for the entity.</typeparam>
    [Serializable]
    public abstract class RobloxEntity<TEntity, TIndex, TDatabase> : IRobloxEntity<TEntity, TIndex>
        where TEntity : RobloxEntity<TEntity, TIndex, TDatabase>, new()
        where TIndex : struct, IEquatable<TIndex>
        where TDatabase : GlobalDatabase<TDatabase>, new()
    {
        #region | Entity Properties |

        public DateTime Created { get; internal set; }
        public DateTime Updated { get; internal set; }

        // Put below Created & Updated to have serialized versions of
        // the entity look less bad.
        [Key]
        public TIndex ID
        {
            get;
#if NET5_0_OR_GREATER
            internal init;
#else
            private set;
#endif
        }

        #endregion | Entity Properties |

        #region | Entity Methods |

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.Save(TEntity)"/>
        public virtual void Save()
            => _Factory.Save((TEntity)this);

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.Delete(TEntity)"/>
        public virtual void Delete()
            => _Factory.Delete((TEntity)this);

        #endregion | Entity Methods |

        #region | Data Access Methods |

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.GetBy"/>
        protected static TEntity GetBy(Predicate<TEntity> predicate)
            => _Factory.GetBy(predicate);

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.MustGetBy"/>
        protected static TEntity MustGetBy(Predicate<TEntity> predicate)
            => _Factory.MustGetBy(predicate);

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.MultiGetBy(Predicate{T})"/>
        protected static ICollection<TEntity> MultiGetBy(Predicate<TEntity> predicate)
            => _Factory.MultiGetBy(predicate);

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.MultiGetBy(Predicate{T}, int, int)"/>
        protected static ICollection<TEntity> MultiGetBy(Predicate<TEntity> predicate, int startRowIndex, int maximumRows)
            => _Factory.MultiGetBy(
                predicate: predicate,
                startRowIndex: startRowIndex,
                maximumRows: maximumRows
            );

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.Get(TIndex)"/>
        public static TEntity Get(TIndex id)
            => _Factory.Get(id);

        /// <inheritdoc cref="Get(TIndex)"/>
        public static TEntity Get(TIndex? id)
        {
            if (id.HasValue)
                return Get(id.Value);
            return null;
        }

        // TODO: public static ICollection<TEntity> MultiGet(ICollection<TIndex> ids)

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.MustGet(TIndex)"/>
        public static TEntity MustGet(TIndex id)
            => _Factory.MustGet(id);

        /// <inheritdoc cref="MustGet(TIndex)"/>
        public static TEntity MustGet(TIndex? id)
        {
            if (id.HasValue)
                return Get(id.Value);
            else
                throw new InvalidOperationException("ID of entity was null when attempting MustGet");
        }

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.GetOrCreate"/>
        protected static TEntity GetOrCreate(Func<TEntity> dalGetter, Func<TEntity> dalCreator)
            => _Factory.GetOrCreate(dalGetter, dalCreator);

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.GetAll"/>
        protected static ICollection<TEntity> GetAll()
            => _Factory.GetAll();

        /// <inheritdoc cref="RobloxEntityFactoryBase{TEntity, TIndex, TDatabase}.GetAll"/>
        protected static ICollection<TEntity> GetAll(int startRowIndex, int maximumRows)
            => _Factory.GetAll(
                startRowIndex: startRowIndex,
                maximumRows: maximumRows
            );

        #endregion | Data Access Methods |

        #region | IEquatable Members |

        public bool Equals(TEntity other)
        {
            TIndex id = this.ID;
            TIndex? num = (other != null) ? new TIndex?(other.ID) : null;
            return id.Equals(num.GetValueOrDefault()) & num != null;
        }

        #endregion | IEquatable Members |

        private static readonly InternalRobloxEntityFactory<TEntity, TIndex, TDatabase> _Factory
            = new InternalRobloxEntityFactory<TEntity, TIndex, TDatabase>();
    }
}