using Kogel.Elasticsearch.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch
{
    public class QuerySet<T> : Query<T>, IQuerySet<T>
    {
        IEsClient _esClient;
        public QuerySet(IEsClient esClient)
        {
            _esClient = esClient;
            EsProvider = new EsProvider();
            EsProvider.Context = new AbstractDataBaseContext
            {
                Set = this
            };
            WhereExpressionList = new List<LambdaExpression>();
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQuerySet<T> Where(Expression<Func<T, bool>> predicate)
        {
            WhereExpressionList.Add(predicate);
            return this;
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        /// <param name="strWhere"></param>
        /// <returns></returns>
        public IQuerySet<T> Where(string strWhere)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 带前置条件的Where判断
        /// </summary>
        /// <param name="where"></param>
        /// <param name="truePredicate"></param>
        /// <param name="falsePredicate"></param>
        /// <returns></returns>
        public IQuerySet<T> WhereIf(bool where, Expression<Func<T, bool>> truePredicate, Expression<Func<T, bool>> falsePredicate)
        {
            if (where)
                WhereExpressionList.Add(truePredicate);
            else
                WhereExpressionList.Add(falsePredicate);
            return this;
        }
    }
}
