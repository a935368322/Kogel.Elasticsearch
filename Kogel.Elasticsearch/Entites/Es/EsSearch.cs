using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch.Entites.Es
{
    /// <summary>
    /// es查询对象
    /// </summary>
    public class EsSearch
    {
        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("query")]
        public EsQuery Query { get; set; }
    }
}
