using System;
using System.Collections.Generic;

using Roblox.Databases;

using Roblox.EntityFrameworkCore.Factories;

namespace Roblox.EntityFrameworkCore
{
    /// <summary>
    /// A base class for all ROBLOX entities that reduces the absurd amount
    /// of boilerplate code of the old system.
    /// </summary>
    /// <remarks>
    /// Acts as a static wrapper of the <see cref="RobloxEntityFactory{TEntity, TIndex, TDatabase}"/> class.
    /// </remarks>
    /// <typeparam name="TEntity">The entity's type.</typeparam>
    /// <typeparam name="TIndex">The type to use for the entity's index.</typeparam>
    /// <typeparam name="TDatabase">The database to use for the entity.</typeparam>
    [Serializable]
    public abstract class RobloxEntity<TEntity, TIndex, TDatabase> : RobloxDto<TEntity, TIndex>, IRobloxEntity<TEntity, TIndex>
        where TEntity : RobloxEntity<TEntity, TIndex, TDatabase>, new()
        where TIndex : struct, IEquatable<TIndex>
        where TDatabase : GlobalDatabase<TDatabase>, new()
    {
        #region | Entity Factory Members |

        /// <summary>
        /// The factory backing the entity's CRUD methods.
        /// </summary>
        private static RobloxDtoFactoryBase<TEntity, TIndex, TDatabase> _Factory
            = new RobloxEntityFactory<TEntity, TIndex, TDatabase>();

        /// <summary>
        /// Configures the entity class to use the given factory for data management.
        /// </summary>
        /// <param name="factory">The factory.</param>
        /// <exception cref="ArgumentNullException"><paramref name="factory"/> was <see cref="null"/>.</exception>
        protected static void SetEntityFactory(RobloxDtoFactoryBase<TEntity, TIndex, TDatabase> factory)
        {
            _Factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        /// <summary>
        /// Constructs an entity factory and configures the entity class to use it for data management.
        /// </summary>
        /// <typeparam name="TFactory">The entity factory type.</typeparam>
        /// <returns>The newly constructed entity factory.</returns>
        protected static TFactory SetEntityFactory<TFactory>()
            where TFactory : RobloxDtoFactoryBase<TEntity, TIndex, TDatabase>, new()
        {
            var factory = new TFactory();
            SetEntityFactory(factory);
            return factory;
        }

        #endregion | Entity Factory Members |

        #region | Entity Methods |

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.Save(TEntity)"/>
        public virtual void Save()
            => _Factory.Save((TEntity)this);

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.Delete(TEntity)"/>
        public virtual void Delete()
            => _Factory.Delete((TEntity)this);

        #endregion | Entity Methods |

        #region | Data Access Methods |

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.GetBy"/>
        protected static TEntity GetBy(Predicate<TEntity> predicate)
            => _Factory.GetBy(predicate);

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.MustGetBy"/>
        protected static TEntity MustGetBy(Predicate<TEntity> predicate)
            => _Factory.MustGetBy(predicate);

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.MultiGetBy(Predicate{T})"/>
        protected static ICollection<TEntity> MultiGetBy(Predicate<TEntity> predicate)
            => _Factory.MultiGetBy(predicate);

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.MultiGetBy(Predicate{T}, int, int)"/>
        protected static ICollection<TEntity> MultiGetBy(Predicate<TEntity> predicate, int startRowIndex, int maximumRows)
            => _Factory.MultiGetBy(
                predicate: predicate,
                startRowIndex: startRowIndex,
                maximumRows: maximumRows
            );

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.Get(TIndex)"/>
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

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.MustGet(TIndex)"/>
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

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.GetOrCreate"/>
        protected static TEntity GetOrCreate(Func<TEntity> dalGetter, Func<TEntity> dalCreator)
            => _Factory.GetOrCreate(dalGetter, dalCreator);

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.GetAll"/>
        protected static ICollection<TEntity> GetAll()
            => _Factory.GetAll();

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.GetAll"/>
        protected static ICollection<TEntity> GetAll(int startRowIndex, int maximumRows)
            => _Factory.GetAll(
                startRowIndex: startRowIndex,
                maximumRows: maximumRows
            );

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.GetCount"/>
        protected static int GetCount()
            => _Factory.GetCount();

        /// <inheritdoc cref="RobloxDtoFactoryBase{TEntity, TIndex, TDatabase}.GetCountBy"/>
        protected static int GetCountBy(Predicate<TEntity> predicate)
            => _Factory.GetCountBy(predicate);

        #endregion | Data Access Methods |
    }
}