using Kogel.Elasticsearch.Entites.Es;
using Kogel.Elasticsearch.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch.Expressions
{
    /// <summary>
    /// 实现表达式的基类
    /// </summary>
    public class BaseExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// 
        /// </summary>
        internal EsBoolItem EsBoolItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public BaseExpressionVisitor()
        {
            EsBoolItem = new EsBoolItem();
        }

        /// <summary>
        /// 有+ - * /需要拼接的对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var binary = new BinaryExpressionVisitor(node);
            EsBoolItem = binary.EsBoolItem;
            return node;
        }

        /// <summary>
        /// 值对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            object nodeValue = node.ToConvertAndGetValue();
            EsBoolItem.FieldValue = nodeValue;
            return node;
        }

        /// <summary>
        /// 成员对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            //需要计算的字段值
            var expTypeName = node.Expression?.GetType().FullName ?? "";
            if (expTypeName == "System.Linq.Expressions.TypedParameterExpression" || expTypeName == "System.Linq.Expressions.PropertyExpression")
            {
                //验证是否是可空对象
                if (!node.Expression.Type.FullName.Contains("System.Nullable")) //(node.Expression.Type != typeof(Nullable))
                {
                    //是否是成员值对象
                    if (expTypeName == "System.Linq.Expressions.PropertyExpression" && node.IsConstantExpression())
                    {
                        //值
                        object nodeValue = node.ToConvertAndGetValue();
                        EsBoolItem.FieldValue = nodeValue;
                    }
                    var member = EntityCache.QueryEntity(node.Expression.Type);
                    //可能有重命名
                    string fieldName = member.EntityFieldList
                        .FirstOrDefault(x => x.PropertyInfo.Name == node.Member.Name)
                        ?.FieldName ?? throw new Exception("字段渲染失败");
                    EsBoolItem.FieldName = fieldName;
                }
                else
                {
                    //可空函数
                    Visit(node.Expression);
                    //switch (node.Member.Name)
                    //{
                    //    case "HasValue":
                    //        {
                    //        }
                    //        break;
                    //}
                }
            }
            else
            {
                //值
                object nodeValue = node.ToConvertAndGetValue();
                EsBoolItem.FieldValue = nodeValue;
            }
            return node;
        }

        /// <summary>
        /// 待执行的方法对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            Operation(node);
            return node;
        }

        /// <summary>
        /// 解析函数
        /// </summary>
        /// <param name="node"></param>
        private void Operation(MethodCallExpression node)
        {
            switch (node.Method.Name)
            {
                default:
                    EsBoolItem.FieldValue = node.ToConvertAndGetValue();
                    break;
            }
        }
    }
}
