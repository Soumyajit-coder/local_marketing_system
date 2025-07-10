using localMarketingSystem.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace localMarketingSystem.DAL
{
    public class Repository<T, Tcontext> : IRepository<T> where T : class where Tcontext : DbContext
    {
        protected readonly Tcontext HappyWorldTradingDBContext = null;

        public Repository(Tcontext context)
        {
            this.HappyWorldTradingDBContext = context;
        }


        public bool IsTransactionRunning()
        {
            return this.HappyWorldTradingDBContext.Database.CurrentTransaction == null ? false : true;
        }
        private IDbContextTransaction BeginTran()
        {
            return this.HappyWorldTradingDBContext.Database.BeginTransaction();
        }



        public IExecutionStrategy GetExecutionStrategy()
        {
            return this.HappyWorldTradingDBContext.Database.CreateExecutionStrategy();
        }


        public IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> result = this.HappyWorldTradingDBContext.Set<T>();
            if (condition != null)
            {
                result = result.Where(condition);
            }

            return result;
        }

        public async Task<ICollection<T>> GetAllByConditionAsync(Expression<Func<T, bool>> condition)
        {
            IQueryable<T> result = this.HappyWorldTradingDBContext.Set<T>();
            if (condition != null)
            {
                result = result.Where(condition);
            }

            return await result.ToListAsync();
        }

        public IQueryable<T> GetAll()
        {
            IQueryable<T> result = this.HappyWorldTradingDBContext.Set<T>();
            return result;
        }

        public async Task<ICollection<T>> GetAllAsync()
        {
            IQueryable<T> result = this.HappyWorldTradingDBContext.Set<T>();
            return await result.ToListAsync();
        }

        public async Task<ICollection<TResult>> GetSelectedColumnAsync<TResult>(Expression<Func<T, TResult>> selectExpression)
        {
            IQueryable<TResult> result = this.HappyWorldTradingDBContext.Set<T>().Select(selectExpression);
            return await result.ToListAsync();
        }

        public async Task<ICollection<TResult>> GetSelectedColumnByConditionAsync<TResult>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TResult>> selectExpression)
        {
            IQueryable<TResult> result = this.HappyWorldTradingDBContext.Set<T>()
                                    .Where(filterExpression)
                                    .Select(selectExpression);

            return await result.ToListAsync();
        }
        public async Task<Dictionary<TKey, List<TResult>>> GetSelectedColumnGroupByConditionAsync<TKey, TResult>(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, TKey>> groupByKeySelector,
            Expression<Func<T, TResult>> selectExpression)
        {
            var data = await this.HappyWorldTradingDBContext.Set<T>()
            .Where(filterExpression)
            .ToListAsync();
            var groupedResult = data
                .GroupBy(groupByKeySelector.Compile())
                .ToDictionary(group => group.Key, group => group.Select(selectExpression.Compile()).ToList());

            return groupedResult;

        }
        public async Task<TResult> GetSingleSelectedColumnByConditionAsync<TResult>(
       Expression<Func<T, bool>> filterExpression,
       Expression<Func<T, TResult>> selectExpression)
        {
            TResult result = await this.HappyWorldTradingDBContext.Set<T>()
                                        .Where(filterExpression)
                                        .Select(selectExpression)
                                        .FirstOrDefaultAsync();

            return result;
        }
        public async Task<string> GetSingleSelectedColumnByConditionAsync(
            Expression<Func<T, bool>> filterExpression,
            Expression<Func<T, string>> selectExpression)
        {
            string result = await this.HappyWorldTradingDBContext.Set<T>()
                                            .Where(filterExpression)
                                            .Select(selectExpression)
                                            .FirstOrDefaultAsync();

            return result;
        }
        public T GetSingle(Expression<Func<T, bool>> condition)
        {
            return this.HappyWorldTradingDBContext.Set<T>().Where(condition).FirstOrDefault();
        }
        public async Task<T> GetSingleAysnc(Expression<Func<T, bool>> condition)
        {
            var retValue = await this.HappyWorldTradingDBContext.Set<T>().Where(condition).SingleOrDefaultAsync();

            return retValue;
        }
        public async Task<T> GetDetailsAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false)
        {
            if (useNoTracking)
            {
                var getValue = await this.HappyWorldTradingDBContext.Set<T>().AsNoTracking().Where(filter).FirstOrDefaultAsync();
                return getValue;
            }
            else
            {
                var getValue = await this.HappyWorldTradingDBContext.Set<T>().Where(filter).FirstOrDefaultAsync();
                return getValue;
            }
        }

        public async Task<object> ExecuteQuery(string sqlQuery, object parameters)
        {
            return await this.HappyWorldTradingDBContext.Database.ExecuteSqlRawAsync(sqlQuery, parameters);
        }
        public int CountWithCondition(Expression<Func<T, bool>> condition)
        {
            return this.HappyWorldTradingDBContext.Set<T>().Count(condition);
        }
        public int Count()
        {
            return this.HappyWorldTradingDBContext.Set<T>().Count();
        }
        public decimal TotalWithCondition(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> condition)
        {
            IQueryable<T> query = this.HappyWorldTradingDBContext.Set<T>();

            if (condition != null)
            {
                query = query.Where(condition);
            }

            return query.Sum(selector);
        }
        public bool Add(T entity)
        {
            this.HappyWorldTradingDBContext.Set<T>().Add(entity);
            return true;
        }
        public void AddRange(IEnumerable<T> entities)
        {
            this.HappyWorldTradingDBContext.Set<T>().AddRange(entities);
        }

        public bool Update(T entity)
        {
            this.HappyWorldTradingDBContext.Entry(entity).State = EntityState.Modified;
            return true;
        }

        public bool Delete(T entity)
        {
            this.HappyWorldTradingDBContext.Set<T>().Remove(entity);
            return true;
        }


        public void SaveChangesManaged()
        {
            this.HappyWorldTradingDBContext.SaveChanges();
        }

    }
}
