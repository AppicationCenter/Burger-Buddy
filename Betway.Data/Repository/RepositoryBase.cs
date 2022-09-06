using Betway.Data.Contexts;
using Betway.Data.IRepository;
using Betway.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Betway.Data.Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T>, IDisposable where T : ModelBase
    {
        private readonly BurgerBuddyContext context;
        private readonly ILogger logger;
        private readonly DbSet<T> tableSet;
        private string errorMessage = string.Empty;

        public RepositoryBase(BurgerBuddyContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;
            this.tableSet = context.Set<T>();
        }

        public BurgerBuddyContext Context
        {
            get
            {
                return this.context;
            }
        }

        public ILogger Logger
        {
            get
            {
                return this.logger;
            }
        }

        public T Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(Delete)} entity must not be null");
            }
            try
            {
                this.tableSet.Remove(entity);

                return entity;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                this.logger.LogError(this.errorMessage);
                throw new Exception(errorMessage, dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be deleted: {ex.Message}");
            }
        }

        public void Dispose()
        {
            if (this.context != null)
                this.context.Dispose();
        }

        public virtual async Task<T> Get(int id)
        {
            var entity = await this.tableSet.FindAsync(id);

            return entity!;
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync()
        {
            try
            {
                var entities = await this.tableSet.ToListAsync();
                return entities;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                this.logger.LogError(this.errorMessage);
                throw new Exception(errorMessage, dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual async Task<IEnumerable<T>> GetAsync(Expression<Func<T, bool>>? where = null,
            Expression<Func<T, bool>>? predicate = null, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null)
        {
            IQueryable<T> query = this.tableSet.AsNoTracking();
            try
            {
                if (where != null)
                {
                    query = query.Where(where);
                }
                if (predicate != null)
                {
                    query = query.Include(predicate);
                }

                if (orderBy != null)
                {
                    return await orderBy(query).ToListAsync();
                }

                return await query.ToListAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                this.logger.LogError(this.errorMessage);
                throw new Exception(errorMessage, dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"Couldn't retrieve entities: {ex.Message}");
            }
        }

        public virtual async Task<IEnumerable<T>> GetAsync(int pageNumber, int pageSize)
        {
            await Task.CompletedTask;
            return this.tableSet.AsNoTracking().Skip(pageNumber * pageSize).Take(pageSize);
        }

        public virtual async Task<T> InsertAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(InsertAsync)} entity must not be null");
            }
            try
            {
                //var entityToAdd = await this.Get(entity.Id);
                //if(entityToAdd != null) 
                //{
                //    entity = await this.UpdateAsync(entity);
                //    return entity;
                //}

                await this.tableSet.AddAsync(entity);

                return entity;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                this.logger.LogError(this.errorMessage);
                throw new Exception(errorMessage, dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
                //throw new Exception($"{nameof(entity)} could not be saved: {ex.Message}");
            }
        }

        public virtual async Task<List<T>> InsertRangeAsync(List<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException($"{nameof(InsertAsync)} entity must not be null");
            }
            try
            {
                await this.tableSet.AddRangeAsync(entities);

                return entities;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                this.logger.LogError(this.errorMessage);
                throw new Exception(errorMessage, dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"List of entities could not be saved: {ex.Message}");
            }
        }

        public virtual async Task<T> UpdateAsync(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException($"{nameof(UpdateAsync)} entity must not be null");
            }
            try
            {
                //T? dbEntity = await this.tableSet.FindAsync(entity);
                T? dbEntity = await this.Get(entity.Id);
                if (dbEntity != null)
                    this.context.Entry(dbEntity).CurrentValues.SetValues(entity);
                //this.tableSet.Update(entity);

                return dbEntity!;
            }
            catch (DbEntityValidationException dbEx)
            {
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        errorMessage += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;
                this.logger.LogError(this.errorMessage);
                throw new Exception(errorMessage, dbEx);
            }
            catch (Exception ex)
            {
                throw new Exception($"{nameof(entity)} could not be updated: {ex.Message}");
            }
        }
    }
}
