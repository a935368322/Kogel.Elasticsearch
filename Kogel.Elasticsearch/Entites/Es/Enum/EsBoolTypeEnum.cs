using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch.Entites.Es.Enum
{
    /// <summary>
    /// 条件类型
    /// </summary>
    public enum EsBoolTypeEnum
    {
        /// <summary>
        /// 等于
        /// </summary>
        Term = 1,

        /// <summary>
        /// 模糊匹配
        /// </summary>
        Match = 2,

        /// <summary>
        /// 范围匹配
        /// </summary>
        Range = 3,

        /// <summary>
        /// EsBool
        /// </summary>
        Bool = 4,
    }
}
