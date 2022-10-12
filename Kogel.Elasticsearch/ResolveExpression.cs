using Kogel.Elasticsearch.Entites.Es;
using Kogel.Elasticsearch.Expressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch
{
    /// <summary>
    /// 解析者
    /// </summary>
    public class ResolveExpression
    {
        /// <summary>
        /// 
        /// </summary>
        protected readonly EsProvider _provider;

        public ResolveExpression(EsProvider provider)
        {
            _provider = provider;
        }

        /// <summary>
        /// 解析查询条件
        /// </summary>
        /// <param name="prefix"></param>
        /// <returns></returns>
        public virtual EsQuery ResolveWhereList()
        {
            EsQuery esQuery = new EsQuery
            {
                Bool = new EsBool
                {
                    Musts = new List<EsBoolItem>()
                }
            };
            //添加Linq生成的查询对象
            List<LambdaExpression> lambdaExpressionList = _provider.Context.Set.WhereExpressionList;
            for (var i = 0; i < lambdaExpressionList.Count; i++)
            {
                var whereExp = new WhereExpression(lambdaExpressionList[i]);
                esQuery.Bool.Musts.Add(whereExp.EsBoolItem);
            }
            return esQuery;
        }
    }
}
