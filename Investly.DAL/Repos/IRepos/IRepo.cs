﻿using Investly.DAL.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos.IRepos
{
   public interface IRepo<T> where T : class
    {
        public IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null);
        public T GetById(int id);
        public void Insert(T entity);
        public void Remove(int id);
        public void RemoveRange(IEnumerable<T> entites);
        public T FirstOrDefault(Expression<Func<T, bool>> filter, string? includeProperties = null);
        public void Update(T entity);
        public void AddRange(IEnumerable<T> entites);

        public PaginatedResult<T> FindAll(
            int? take = 10,
            int? skip = 0,
            Expression<Func<T, bool>> criteria = null,
            Expression<Func<T, object>> orderBy = null,
            string orderByDirection = OrderBy.Ascending
        );

        public Task<PaginatedResult<T>> FindAllAsync(
            int? take = 10,
            int? skip = 0,
            Expression<Func<T, bool>> criteria = null,
            Expression<Func<T, object>> orderBy = null,
            string orderByDirection = OrderBy.Ascending
        );


    }
}
