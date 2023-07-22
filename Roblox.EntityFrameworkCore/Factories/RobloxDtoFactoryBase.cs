using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.EntityFrameworkCore;

using Roblox.Databases;

namespace Roblox.EntityFrameworkCore.Factories
{
    /// <summary>
    /// A base class for all RobloxDto factories.
    /// An instantiable way to fetch data transfer objects.
    /// Intended to be inherited and modified to your heart's content.
    /// </summary>
    public abstract class RobloxDtoFactoryBase<TDto, TIndex, TDatabase> : IRobloxDtoFactory<TDto, TIndex>
        where TDto : RobloxDto<TDto, TIndex>
        where TIndex : struct, IEquatable<TIndex>
        where TDatabase : GlobalDatabase<TDatabase>, new()
    {
        protected static RobloxDbContext<TDto, TIndex, TDatabase> GetDbContext()
            => new RobloxDbContext<TDto, TIndex, TDatabase>();

        #region | Core CRUD Methods |

        protected virtual void Create(TDto dto)
        {
            using (var db = GetDbContext())
            {
                db.Table.Add(dto);
                db.SaveChanges();
            }
        }

        protected virtual T QueryData<T>(Func<DbSet<TDto>, T> queryFunc)
        {
            T result = default;

            using (var db = GetDbContext())
            {
                result = queryFunc(db.Table);
            }

            return result;
        }

        protected virtual void Update(TDto dto)
        {
            using (var db = GetDbContext())
            {
                db.Table.Update(dto);
                db.SaveChanges();
            }
        }

        public virtual void Delete(TDto dto)
        {
            using (var db = GetDbContext())
            {
                db.Table.Remove(dto);
                db.SaveChanges();
            }
        }

        #endregion | Core CRUD Methods |

        #region | Protected CRUD Methods |

        /// <summary>
        /// Gets a data transfer object by the given predicate.
        /// </summary>
        /// <remarks>
        /// The "GetBy" methods are protected to force specific "GetBy[ColumnA]" methods to be defined in the entity's definition,
        /// rather than calling GetBy(predicate) outside of the entity.
        /// </remarks>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The data transfer object.</returns>
        internal protected virtual TDto GetBy(Predicate<TDto> predicate)
        {
            var pred = new Func<TDto, bool>(predicate);
            return QueryData(
                (table) => table.FirstOrDefault(pred)
            );
        }

        /// <summary>
        /// Gets a DTO by the given predicate or throws an exception.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The populated DTO.</returns>
        /// <exception cref="InvalidOperationException">Unable to get DTO.</exception>
        internal protected virtual TDto MustGetBy(Predicate<TDto> predicate)
        {
            return GetBy(predicate) ?? throw new InvalidOperationException($"Failed to load {typeof(TDto).Name}.");
        }

        /// <summary>
        /// Gets a collection of DTOs by the given predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>A collection of DTOs.</returns>
        internal protected virtual ICollection<TDto> MultiGetBy(Predicate<TDto> predicate)
        {
            var pred = new Func<TDto, bool>(predicate);
            return QueryData(
                (table) => table.Where(pred)
                .ToList()
            );
        }

        /// <summary>
        /// Gets a collection of DTOs by the given predicate.
        /// Also comes with paging support!
        /// </summary>
        /// <remarks>
        /// NOTE: Does not work with 64-bit IDs
        /// </remarks>
        /// <param name="predicate">The predicate.</param>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns>A collection of DTOs.</returns>
        internal protected virtual ICollection<TDto> MultiGetBy(Predicate<TDto> predicate, int startRowIndex, int maximumRows)
        {
            var pred = new Func<TDto, bool>(predicate);
            return QueryData(
                (table) => table.Where(pred)
                .Skip(startRowIndex)
                .Take(maximumRows)
                .ToList()
            );
        }

        /// <summary>
        /// Gets or creates the DTO using <paramref name="dtoGetter"/> and <paramref name="dtoCreator"/>.
        /// </summary>
        /// <remarks>
        /// This is protected since it should only be called on by an entity or entity factory.
        /// </remarks>
        /// <param name="dtoGetter">The method to call to get the DTO.</param>
        /// <param name="dtoCreator">The method to call to create the DTO.</param>
        /// <returns><paramref name="dtoGetter"/> result if not null or <paramref name="dtoCreator"/> result if not null respectively.</returns>
        internal protected virtual TDto GetOrCreate(Func<TDto> dtoGetter, Func<TDto> dtoCreator)
            => dtoGetter() ?? dtoCreator();

        /// <summary>
        /// Gets all known <see cref="TDto"/>s in the table.
        /// </summary>
        /// <remarks>
        /// This is protected since not all entities should be able to have their entire tables fetched by default.
        /// </remarks>
        /// <returns>An <see cref="ICollection{TDto}"/> of <see cref="TDto"/>s</returns>
        internal protected virtual ICollection<TDto> GetAll()
            => QueryData((table) => table.ToList());

        /// <summary>
        /// Gets all known <see cref="TDto"/>s in the table.
        /// Also comes with paging support!
        /// </summary>
        /// <remarks>
        /// NOTE: Does not work with 64-bit IDs
        /// This is protected since not all entities should be able to have their entire tables fetched by default.
        /// </remarks>
        /// <param name="startRowIndex"></param>
        /// <param name="maximumRows"></param>
        /// <returns>An <see cref="ICollection{TDto}"/> of <see cref="TDto"/>s</returns>
        internal protected virtual ICollection<TDto> GetAll(int startRowIndex, int maximumRows)
        {
            return QueryData(
                (table) => table
                .Skip(startRowIndex)
                .Take(maximumRows)
                .ToList()
            );
        }

        /// <summary>
        /// Gets a total count of DTOs in a table.
        /// </summary>
        /// <remarks>
        /// This is protected since not all entities should be able to have their entire tables counted by default.
        /// </remarks>
        /// <returns>The total number of DTOs.</returns>
        internal protected virtual int GetCount()
            => QueryData((table) => table.Count());

        /// <summary>
        /// Gets a count of DTOs in a table based on a predicate.
        /// </summary>
        /// <remarks>
        /// This is protected since not all entities should be able to have their entire tables counted by default.
        /// </remarks>
        /// <param name="predicate">The predicate.</param>
        /// <returns>The total number of DTOs.</returns>
        internal protected virtual int GetCountBy(Predicate<TDto> predicate)
        {
            var pred = new Func<TDto, bool>(predicate);
            return QueryData(
                (table) => table.Where(pred)
                .Count()
            );
        }

        /// <summary>
        /// Inserts or updates the given DTO in the database.
        /// </summary>
        /// <remarks>
        /// This is protected so someone can't specify inserters and updaters outside an DTO's definition.
        /// </remarks>
        /// <param name="dto">The DTO to save to the database.</param>
        /// <param name="dtoInserter">The method to call before inserting the DTO.</param>
        /// <param name="dtoUpdater">The method to call before updating the DTO.</param>
        internal protected virtual void Save(TDto dto, Action dtoInserter, Action dtoUpdater)
        {
            if (dto.ID.Equals(default))
            {
                dtoInserter();
                Create(dto);
            }
            else
            {
                dtoUpdater();
                Update(dto);
            }
        }

        #endregion | Protected CRUD Methods |

        #region | Public CRUD Methods |

        public virtual TDto Get(TIndex id)
            => GetBy(dto => dto.ID.Equals(id));

        public virtual TDto MustGet(TIndex id)
            => MustGetBy(dto => dto.ID.Equals(id));

        public virtual void Save(TDto dto)
        {
            Save(
                dto,
                () =>
                {
                    dto.Created = DateTime.Now;
                    dto.Updated = dto.Created;
                },
                () =>
                {
                    dto.Updated = DateTime.Now;
                }
            );
        }

        #endregion | Public CRUD Methods |
    }
}
