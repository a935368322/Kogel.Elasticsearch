using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch.Test.Models
{
    /// <summary>
    /// es的产品表索引
    /// </summary>
    [Table("es_mall_product")]
    public class EsProduct
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string materialCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string partNumber { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string theLabel { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int categoryID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string unit { get; set; }
        /// <summary>
        /// 圆盘
        /// </summary>
        public string specifications { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int? isInventory { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int nmb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int brandID { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? weight { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string plantModle { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string productNo { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string seal { get; set; }
        /// <summary>
        /// 贴片钽电容-10uF-±10%-16V-CASE-B-
        /// </summary>
        public string describe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int isShelf { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int minNmb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int incNmb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int inventoryNmb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int frozenNmb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int dealNmb { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int sortCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string deleteMark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string value { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pn { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string pnUnit { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string voltage { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? minPrice { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal? maxPric { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? accuracyInt { get; set; }
    }
}
