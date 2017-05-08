using Entities.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public interface IRepositoryFactory : IDisposable
    {
        void Insert<TEntity>(TEntity entity) where TEntity : class;
        void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class;
        void Update<TEntity>(TEntity entity) where TEntity : class;
        void Delete<TEntity>(object id) where TEntity : class;
        void Delete<TEntity>(TEntity entity) where TEntity : class;
        void SaveChanges();
        TEntity Find<TEntity>(params object[] keyValues) where TEntity : class;      
        IQueryable<TEntity> Queryable<TEntity>() where TEntity : class;

        IQueryable<TEntity> Select<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null) where TEntity : class;

        Task<IEnumerable<TEntity>> SelectAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null) where TEntity : class;
        
        IQueryable<TEntity> Get<TEntity>() where TEntity : class;
        IQueryable<TEntity> GetNT<TEntity>() where TEntity : class;

        //Async Operations
        Task<TEntity> GetAsync<TEntity>() where TEntity : class;
        Task<TEntity> GetEAsync<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class;
        Task<List<TEntity>> GetNTLAsync<TEntity>() where TEntity : class;
        Task<List<TEntity>> GetNTLEAsync<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class;
         
    }
    public class RepositoryFactory : IRepositoryFactory, IDisposable
    {
        private BusTicketingContext _dataContext;
        public RepositoryFactory(BusTicketingContext dataContext)
        {
            _dataContext = dataContext;
        }

        private IDbSet<TEntity> GetDBSet<TEntity>() where TEntity : class
        {
            return _dataContext.Set<TEntity>();
        }

        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            GetDBSet<TEntity>().Add(entity);
        }

        public void InsertRange<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                Insert(entity);
            }
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        { 
            var objContext = ((IObjectContextAdapter)_dataContext).ObjectContext;
            var objSet = objContext.CreateObjectSet<TEntity>();
            var entityKey = objContext.CreateEntityKey(objSet.EntitySet.Name, entity);

            Object foundEntity;
            var exists = objContext.TryGetObjectByKey(entityKey, out foundEntity);
            // Detach it here to prevent side-effects
            if (exists)
            {
                objContext.Detach(foundEntity);
            }

            GetDBSet<TEntity>().Attach(entity);
            _dataContext.Entry(entity).State = EntityState.Modified;
        }        

        public void Delete<TEntity>(object id) where TEntity : class
        {
            var entity = GetDBSet<TEntity>().Find(id);
            Delete(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            GetDBSet<TEntity>().Remove(entity);
        }
        public void SaveChanges()
        {
            _dataContext.SaveChanges();
        }
        public virtual TEntity Find<TEntity>(params object[] keyValues) where TEntity : class
        {
            return GetDBSet<TEntity>().Find(keyValues);
        }

        public IQueryable<TEntity> Queryable<TEntity>() where TEntity : class
        {
            return GetDBSet<TEntity>();
        }

        public IQueryable<TEntity> Select<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null) where TEntity : class
        {
            IQueryable<TEntity> query = GetDBSet<TEntity>();

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, include) => current.Include(include));
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (page != null && pageSize != null)
            {
                query = query.Skip((page.Value - 1) * pageSize.Value).Take(pageSize.Value);
            }
            return query;
        }        

        public async Task<IEnumerable<TEntity>> SelectAsync<TEntity>(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            List<Expression<Func<TEntity, object>>> includes = null,
            int? page = null,
            int? pageSize = null) where TEntity : class
        {
            return await Select(filter, orderBy, includes, page, pageSize).ToListAsync();
        }

        public IQueryable<TEntity> Get<TEntity>() where TEntity : class
        {
            return GetDBSet<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> GetNT<TEntity>() where TEntity : class
        {
            return GetDBSet<TEntity>().AsNoTracking().AsQueryable();
        }

        public Task<TEntity> GetAsync<TEntity>() where TEntity : class
        {
            return GetDBSet<TEntity>().FirstOrDefaultAsync();
        }

        public Task<TEntity> GetEAsync<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class
        {
            return GetDBSet<TEntity>().Where(exp).FirstOrDefaultAsync();
        }

        public Task<List<TEntity>> GetNTLAsync<TEntity>() where TEntity : class
        {
            return GetDBSet<TEntity>().AsNoTracking().ToListAsync();
        }

        public Task<List<TEntity>> GetNTLEAsync<TEntity>(Expression<Func<TEntity, bool>> exp) where TEntity : class
        {
            return GetDBSet<TEntity>().AsNoTracking().Where(exp).ToListAsync();
        }

        public void Dispose()
        {
            if (this._dataContext != null)
            {
                this._dataContext.Dispose();
            }
        }       
    }
}
