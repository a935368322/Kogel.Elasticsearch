using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch.Interfaces
{
    /// <summary>
    /// es连接
    /// </summary>
    public interface IEsClient
    {
        /// <summary>
        /// 查询设置
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        IQuerySet<T> QuerySet<T>();

       
    }
}
