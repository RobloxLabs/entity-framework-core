using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Roblox.Databases;

namespace Roblox.EntityFrameworkCore.Entities
{
    /// <summary>
    /// An entity that tries to serve the purpose of a data-bound enum.
    /// </summary>
    public abstract class EnumeratorEntity<TEntity, TIndex, TDatabase> : RobloxEntity<TEntity, TIndex, TDatabase>, IEnumeratorEntity<TIndex>
        where TEntity : EnumeratorEntity<TEntity, TIndex, TDatabase>, new()
        where TIndex : struct, IEquatable<TIndex>
        where TDatabase : GlobalDatabase<TDatabase>, new()
    {
        #region | Properties |

        /// <summary>
        /// The value assigned to the enum.
        /// </summary>
        [Required]
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
            TEntity result = null;

            if (!string.IsNullOrWhiteSpace(value))
            {
                result = GetBy(
                    (entity) => entity.Value == value
                );
            }

            return result;
        }

        /// <summary>
        /// Gets or creates an entity by Value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>A new or the pre-existing entity with the given Value</returns>
        public static TEntity GetOrCreate(string value)
        {
            return GetOrCreate(
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
            if (string.IsNullOrWhiteSpace(value)) throw new ArgumentNullException(nameof(value));

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
        public static new ICollection<TEntity> GetAll()
        {
            // Dumb hack
            return RobloxEntity<TEntity, TIndex, TDatabase>.GetAll();
        }

        #endregion | CRUD Methods |
    }
}
