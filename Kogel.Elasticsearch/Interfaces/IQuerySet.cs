using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch.Interfaces
{
    /// <summary>
    /// 读取设置
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IQuerySet<T> : IQuery<T>
    {
        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        IQuerySet<T> Where(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        IQuerySet<T> Where(string strWhere);

        /// <summary>
        /// 带前置条件的Where判断
        /// </summary>
        /// <typeparam name="TWhere"></typeparam>
        /// <param name="where"></param>
        /// <param name="truePredicate"></param>
        /// <param name="falsePredicate"></param>
        /// <returns></returns>
        IQuerySet<T> WhereIf(bool where, Expression<Func<T, bool>> truePredicate, Expression<Func<T, bool>> falsePredicate);
    }
}
