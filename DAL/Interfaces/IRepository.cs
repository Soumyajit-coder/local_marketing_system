using Microsoft.EntityFrameworkCore.Storage;
using System.Linq.Expressions;

namespace localMarketingSystem.DAL.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAllByCondition(Expression<Func<T, bool>> condition);
        Task<ICollection<T>> GetAllByConditionAsync(Expression<Func<T, bool>> condition);

        IQueryable<T> GetAll();
        Task<ICollection<T>> GetAllAsync();

        Task<ICollection<TResult>> GetSelectedColumnAsync<TResult>(Expression<Func<T, TResult>> selectExpression);
        Task<ICollection<TResult>> GetSelectedColumnByConditionAsync<TResult>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TResult>> selectExpression);
        public Task<TResult> GetSingleSelectedColumnByConditionAsync<TResult>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TResult>> selectExpression);
        Task<string> GetSingleSelectedColumnByConditionAsync(Expression<Func<T, bool>> filterExpression, Expression<Func<T, string>> selectExpression);
        Task<Dictionary<TKey, List<TResult>>> GetSelectedColumnGroupByConditionAsync<TKey, TResult>(Expression<Func<T, bool>> filterExpression, Expression<Func<T, TKey>> groupByKeySelector, Expression<Func<T, TResult>> selectExpression);
        T GetSingle(Expression<Func<T, bool>> condition);

        Task<T> GetSingleAysnc(Expression<Func<T, bool>> condition);
        Task<T> GetDetailsAsync(Expression<Func<T, bool>> filter, bool useNoTracking = false);
        public int CountWithCondition(Expression<Func<T, bool>> condition);
        public int Count();
        public decimal TotalWithCondition(Expression<Func<T, decimal>> selector, Expression<Func<T, bool>> condition);
        bool Add(T entity);
        bool Update(T entity);
        bool Delete(T entity);

        void SaveChangesManaged();
        public IExecutionStrategy GetExecutionStrategy();

        public Task<object> ExecuteQuery(string sqlQuery, object parameters);
    }
}
