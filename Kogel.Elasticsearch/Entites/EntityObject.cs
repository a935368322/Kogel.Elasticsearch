using Kogel.Elasticsearch.Helper;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch.Entites
{
    /// <summary>
    /// 
    /// </summary>
    public class EntityObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public EntityObject(Type type)
        {
            //反射表名称
            this.Name = type.Name;
            //获取是否有Display特性
            var typeAttribute = type.GetCustomAttributess(true).FirstOrDefault(x => x.GetType().Equals(typeof(TableAttribute)));
            if (typeAttribute != null)
            {
                var tableProperty = typeAttribute as TableAttribute;
                //是否有重命名
                var rename = tableProperty?.Name;
                if (!string.IsNullOrEmpty(rename))
                {
                    this.Name = rename;
                }
            }
            this.Type = type;
            //反射实体类属性
            this.Properties = type.GetProperties();
            List<PropertyInfo> PropertyInfoList = new List<PropertyInfo>();
            //字段列表
            this.EntityFieldList = new List<EntityField>();
            //反射实体类字段
            foreach (var item in this.Properties)
            {
                //当前字段属性设置
                var fieldAttribute = item.GetCustomAttributes(true).FirstOrDefault(x => x.GetType().Equals(typeof(JsonProperty)));
                if (fieldAttribute != null)
                {
                    JsonProperty? fieldProperty = fieldAttribute as JsonProperty;
                    //获取是否是表关系隐射字段
                    if (fieldProperty != null)
                    {
                        PropertyInfoList.Add(item);
                        //设置详细属性
                        EntityFieldList.Add(new EntityField()
                        {
                            FieldName = fieldProperty.PropertyName,
                            PropertyInfo = item
                        });
                    }
                }
                else
                {
                    PropertyInfoList.Add(item);
                    //设置详细属性
                    EntityFieldList.Add(new EntityField()
                    {
                        FieldName = item.Name,
                        PropertyInfo = item
                    });
                }
            }
            this.Properties = PropertyInfoList.ToArray();
        }

        /// <summary>
        /// 类名(表名称)
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public Type Type { get; set; }

        /// <summary>
        /// 命名空间
        /// </summary>
        public string AssemblyString { get; set; }

        /// <summary>
        /// 类反射的属性实例
        /// </summary>
        public PropertyInfo[] Properties { get; set; }

        /// <summary>
        /// 字段列表
        /// </summary>
        public List<EntityField> EntityFieldList { get; set; }
    }

    /// <summary>
    /// 实体字段
    /// </summary>
    public class EntityField
    {
        /// <summary>
        /// 
        /// </summary>
        public PropertyInfo PropertyInfo { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string FieldName { get; set; }
    }
}
