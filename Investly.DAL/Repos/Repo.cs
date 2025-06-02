using Investly.DAL.Entities;
using Investly.DAL.Helper;
using Investly.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
   public class Repo<T> : IRepo<T> where T : class
    {
        private readonly AppDbContext _db;
        private readonly DbSet<T> dbSet;
        public Repo(AppDbContext db)
        {
            _db = db;
           this.dbSet = _db.Set<T>();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? properties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (properties != null)
            {
                foreach (var prop in properties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            return query.ToList();
        }

        public void Remove(int id)
        {
            T entity = dbSet.Find(id);
            dbSet.Remove(entity);

        }

        public void RemoveRange(IEnumerable<T> entites)
        {
            dbSet.RemoveRange(entites);

        }
        public T GetById(int id)
        {
            T entity = dbSet.Find(id);
            return entity;
        }

        public void Insert(T entity)
        {
            dbSet.Add(entity);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var prop in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(prop);
                }
            }
            return query.FirstOrDefault();
        }
        public void Update(T entity)
        {
            dbSet.Update(entity);
        }
        public void AddRange(IEnumerable<T> entites)
        {
            dbSet.AddRange(entites);
        }

        ////////////////


        // Synchronous version
        public PaginatedResult<T> FindAll(
            int? take = 10,
            int? skip = 0,
            Expression<Func<T, bool>> criteria = null,
            Expression<Func<T, object>> orderBy = null,
            string orderByDirection = OrderBy.Ascending
        )
        {
            IQueryable<T> query = dbSet.AsQueryable();

            // Get total count before applying filters
            var totalItemsInTable = query.Count();

            // Apply filtering criteria
            if (criteria != null)
                query = query.Where(criteria);

            // Get filtered count
            var totalFilteredItems = query.Count();

            // Apply ordering before pagination
            if (orderBy != null)
            {
                query = orderByDirection == OrderBy.Ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }

            // Apply pagination
            if (skip.HasValue && skip.Value > 0)
                query = query.Skip(skip.Value);

            if (take.HasValue && take.Value > 0)
                query = query.Take(take.Value);

            // Execute query
            var items = query.ToList();

            // Calculate pagination metadata
            var pageSize = take.GetValueOrDefault(10);
            var totalPages = (int)Math.Ceiling(totalFilteredItems / (double)pageSize);
            var currentPage = (skip.GetValueOrDefault(0) / pageSize) + 1;

            return new PaginatedResult<T>
            {
                Items = items,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalFilteredItems = totalFilteredItems,
                TotalItemsInTable = totalItemsInTable
            };
        }

        // Asynchronous version
        public async Task<PaginatedResult<T>> FindAllAsync(
            int? take = 10,
            int? skip = 0,
            Expression<Func<T, bool>> criteria = null,
            Expression<Func<T, object>> orderBy = null,
            string orderByDirection = OrderBy.Ascending
        )
        {
            IQueryable<T> query = dbSet.AsQueryable();

            // Get total count before applying filters (async)
            var totalItemsInTable = await query.CountAsync();

            // Apply filtering criteria
            if (criteria != null)
                query = query.Where(criteria);

            // Get filtered count (async)
            var totalFilteredItems = await query.CountAsync();

            // Apply ordering before pagination
            if (orderBy != null)
            {
                query = orderByDirection == OrderBy.Ascending
                    ? query.OrderBy(orderBy)
                    : query.OrderByDescending(orderBy);
            }

            // Apply pagination
            if (skip.HasValue && skip.Value > 0)
                query = query.Skip(skip.Value);

            if (take.HasValue && take.Value > 0)
                query = query.Take(take.Value);

            // Execute query (async)
            var items = await query.ToListAsync();

            // Calculate pagination metadata
            var pageSize = take.GetValueOrDefault(10);
            var totalPages = (int)Math.Ceiling(totalFilteredItems / (double)pageSize);
            var currentPage = (skip.GetValueOrDefault(0) / pageSize) + 1;

            return new PaginatedResult<T>
            {
                Items = items,
                CurrentPage = currentPage,
                PageSize = pageSize,
                TotalPages = totalPages,
                TotalFilteredItems = totalFilteredItems,
                TotalItemsInTable = totalItemsInTable
            };
        }


    }
}
