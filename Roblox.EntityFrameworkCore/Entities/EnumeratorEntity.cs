using System;
using System.Collections.Generic;
using Roblox.Databases;

namespace Roblox.EntityFrameworkCore.Entities
{
    /// <summary>
    /// An entity that tries to serve the purpose of a data-bound enum.
    /// </summary>
    public class EnumeratorEntity<TEntity, TIndex, TDatabase> : RobloxEntity<TEntity, TIndex, TDatabase>
        where TEntity : EnumeratorEntity<TEntity, TIndex, TDatabase>, new()
        where TIndex : struct, IComparable<TIndex>
        where TDatabase : IGlobalDatabase
    {
        #region | Properties |

        /// <summary>
        /// The value assigned to the enum.
        /// </summary>
        public string Value { get; set; }

        #endregion | Properties |

        #region | CRUD Methods |

        /// <summary>
        /// Gets the entity by Value.
        /// </summary>
        /// <param name="value">The Value associated with the desired entity</param>
        /// <returns>The entity with the given Value</returns>
        public static TEntity Get(string value)
        {
            return DoGetBy(
                (entity) => entity.Value == value
            );
        }

        /// <summary>
        /// Gets or creates an entity by Value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A new or the pre-existing entity with the given Value</returns>
        public static TEntity GetOrCreate(string value)
        {
            return DoGetOrCreate(
                () => Get(value),
                () => Create(value)
            );
        }

        /// <summary>
        /// Creates a new entity.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The created entity</returns>
        public static TEntity Create(string value)
        {
            var result = new TEntity
            {
                Value = value
            };
            result.Save();

            return result;
        }

        /// <summary>
        /// Gets all entities in the table.
        /// </summary>
        /// <returns>An <see cref="ICollection{TEntity}"/> of entities</returns>
        public static ICollection<TEntity> GetAll()
        {
            return DoGetAll();
        }

        #endregion | CRUD Methods |
    }
}
