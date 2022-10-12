using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractSet
    {
        /// <summary>
		/// 数据解析提供方
		/// </summary>
		public EsProvider EsProvider;

        /// <summary>
        /// 条件表达式对象
        /// </summary>
        internal List<LambdaExpression> WhereExpressionList { get; set; }
    }
}
