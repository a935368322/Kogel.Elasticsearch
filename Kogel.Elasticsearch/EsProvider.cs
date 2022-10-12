using Kogel.Elasticsearch.Entites.Es;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch
{
    /// <summary>
    /// es解析提供方
    /// </summary>
    public class EsProvider
    {
        /// <summary>
        /// es的查询对象
        /// </summary>
        public EsSearch Search { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public AbstractDataBaseContext Context { get; set; }

        /// <summary>
        /// 解析者
        /// </summary>
        private readonly ResolveExpression _resolveExpression;

        public EsProvider()
        {
            Search = new EsSearch();
            _resolveExpression = new ResolveExpression(this);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual EsProvider FormatToList<T>()
        {
            Search.Query = _resolveExpression.ResolveWhereList();
            return this;
        }
    }
}
