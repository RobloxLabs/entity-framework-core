using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using Roblox.Databases;

namespace Roblox.EntityFrameworkCore
{
    public abstract class RobloxEntity<TEntity, TIndex, TDatabase> : IRobloxEntity<TEntity, TIndex>
        where TEntity : RobloxEntity<TEntity, TIndex, TDatabase>, new()
        where TIndex : struct, IComparable<TIndex>
        where TDatabase : IGlobalDatabase
    {
        #region | Entity Properties |

        /// <summary>
        /// The ID of the entity
        /// </summary>
        [Key]
        public TIndex ID { get; private set; }

        /// <summary>
        /// The timestamp to mark when the entity was created
        /// </summary>
        public DateTime Created { get; private set; }

        /// <summary>
        /// The timestamp to mark when the entity was last updated
        /// </summary>
        public DateTime Updated { get; private set; }

        #endregion | Entity Properties |

        #region | Repository Methods |

        protected static T DoQuery<T>(Func<DbSet<TEntity>, T> queryFunc)
        {
            T result = default(T);

            using (var db = GetDbContext())
            {
                result = queryFunc(db.Table);
            }

            return result;
        }

        protected static TEntity DoGetBy(Func<TEntity, bool> query)
        {
            return DoQuery(
                (table) => table.FirstOrDefault(query)
            );
        }

        protected static TEntity DoMustGetBy(Func<TEntity, bool> query)
        {
            return DoGetBy(query) ?? throw new InvalidOperationException($"Failed to load {typeof(TEntity).Name}.");
        }

        protected static ICollection<TEntity> DoMultiGetBy(Func<TEntity, bool> query)
        {
            return DoQuery(
                (table) => table.Where(query)
            ).ToList();
        }

        private static TEntity DoGetById(TIndex id)
        {
            return DoGetBy(entity => entity.ID.Equals(id));
        }

        private static TEntity DoMustGetById(TIndex id)
        {
            return DoMustGetBy(entity => entity.ID.Equals(id));
        }

        protected static TEntity DoGetOrCreate(Func<TEntity> dalGetter, Func<TEntity> dalCreator)
        {
            var result = dalGetter();

            if (result == null)
            {
                result = dalCreator();
            }

            return result;
        }

        protected static ICollection<TEntity> DoGetAll()
        {
            return DoQuery((table) => table.ToList());
        }

        #endregion | Repository Methods |

        #region | DAL Methods |

        private void DoInsert()
        {
            using (var db = GetDbContext())
            {
                db.Table.Add((TEntity)this);
                db.SaveChanges();
            }
        }

        private void DoUpdate()
        {
            using (var db = GetDbContext())
            {
                db.Table.Update((TEntity)this);
                db.SaveChanges();
            }
        }

        private void DoDelete()
        {
            using (var db = GetDbContext())
            {
                db.Table.Remove((TEntity)this);
                db.SaveChanges();
            }
        }

        #endregion | DAL Methods |

        #region | EntityHelper Methods |

        protected void SaveEntity(Action dalInserter, Action dalUpdater)
        {
            if (this.ID.CompareTo(default(TIndex)) == 0)
            {
                dalInserter();
                DoInsert();
            }
            else
            {
                dalUpdater();
                DoUpdate();
            }
        }

        #endregion | EntityHelper Methods |

        #region | BIZ Methods |

        /// <summary>
        /// Gets the entity associated with the given ID.
        /// </summary>
        /// <param name="id">The ID to fetch the entity by</param>
        /// <returns>The entity associated with the given ID</returns>
        public static TEntity Get(TIndex id)
        {
            return DoGetById(id);
        }

        /// <summary>
        /// Gets the entity associated with the given ID, or throws an exception.
        /// </summary>
        /// <param name="id">The ID to fetch the entity by</param>
        /// <returns>The entity associated with the given ID</returns>
        /// <exception cref="InvalidOperationException">The entity with the given ID doesn't exist</exception>
        public static TEntity MustGet(TIndex id)
        {
            return DoMustGetById(id);
        }

        public virtual void Save()
        {
            this.SaveEntity(
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
            DoDelete();
        }

        #endregion | BIZ Methods |

        protected static RobloxDbContext<TEntity, TIndex, TDatabase> GetDbContext() => new();
    }
}