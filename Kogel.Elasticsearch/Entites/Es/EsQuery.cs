using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kogel.Elasticsearch.Entites.Es.Enum;
using Newtonsoft.Json;

namespace Kogel.Elasticsearch.Entites.Es
{
    /// <summary>
    /// 对应es query
    /// </summary>
    public class EsQuery
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("bool")]
        public EsBool @Bool { get; set; }
    }

    /// <summary>
    /// 对应 es bool
    /// </summary>
    public class EsBool
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("must")]
        public List<EsBoolItem> Musts { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("should")]
        public List<EsBoolItem> Shoulds { get; set; }
    }

    /// <summary>
    /// 条件明细
    /// </summary>
    public class EsBoolItem
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonIgnore]
        public EsBoolTypeEnum BoolType { get; set; } = EsBoolTypeEnum.Term;

        /// <summary>
        /// 字段名称
        /// </summary>
        [JsonIgnore]
        public string FieldName { get; set; }

        /// <summary>
        /// 字段值（BoolType为Range时值为EsBoolRange，为bool时值为EsBool）
        /// </summary>
        [JsonIgnore]
        public object FieldValue { get; set; }
    }

    /// <summary>
    /// 范围条件
    /// </summary>
    public class EsBoolRange
    {
        /// <summary>
        /// 大于
        /// </summary>
        [JsonProperty("gt")]
        public object? Gt { get; set; }

        /// <summary>
        /// 大于等于
        /// </summary>
        [JsonProperty("gte")]
        public object? Gte { get; set; }

        /// <summary>
        /// 小于
        /// </summary>
        [JsonProperty("lt")]
        public object? Lt { get; set; }

        /// <summary>
        /// 小于等于
        /// </summary>
        [JsonProperty("lte")]
        public object? Lte { get; set; }
    }
}
