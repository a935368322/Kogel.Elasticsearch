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
    /// 用于解析条件表达式
    /// </summary>
    public class WhereExpressionVisitor : BaseExpressionVisitor
    {
        /// <summary>
        /// 
        /// </summary>
        internal new EsBoolItem EsBoolItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public WhereExpressionVisitor()
        {
            EsBoolItem = new EsBoolItem();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            string callName = node.Method.DeclaringType?.FullName ?? "";
            if (callName.Contains("Kogel.Elasticsearch"))
            {
                base.VisitMethodCall(node);
                EsBoolItem.BoolType = Entites.Es.Enum.EsBoolTypeEnum.Bool;
                EsBoolItem.FieldValue = base.EsBoolItem;
            }
            else
            {
                Operation(node);
            }
            return node;
        }

        /// <summary>
        /// 处理判断字符
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitUnary(UnaryExpression node)
        {
            throw new Exception("无法处理带!的判断字符");
            //return node;
        }

        /// <summary>
        /// 重写成员对象，得到字段名称
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            var expType = node.Expression?.GetType();
            var expTypeName = expType?.FullName ?? "";
            if (expTypeName == "System.Linq.Expressions.TypedParameterExpression" || expTypeName == "System.Linq.Expressions.PropertyExpression")
            {
                //验证是否是可空对象
                if (!node.Expression?.Type?.FullName?.Contains("System.Nullable") ?? false) //(node.Expression.Type != typeof(Nullable))
                {
                    //是否是成员值对象
                    if (expTypeName == "System.Linq.Expressions.PropertyExpression" && node.IsConstantExpression())
                    {
                        object nodeValue = node.ToConvertAndGetValue();
                        EsBoolItem.FieldValue = nodeValue;
                        return node;
                    }
                    var member = EntityCache.QueryEntity(node.Expression.Type);
                    EsBoolItem.FieldName = member.EntityFieldList.FirstOrDefault(x => x.PropertyInfo.Name == node.Member.Name)
                        ?.FieldName ?? throw new Exception("字段渲染失败");
                }
                else
                {
                    //可空函数
                    Visit(node.Expression);
                    switch (node.Member.Name)
                    {
                        case "HasValue":
                            {
                                //this.SpliceField.Append(" IS NOT NULL");
                            }
                            break;
                    }
                }
            }
            else
            {
                object nodeValue = node.ToConvertAndGetValue();
                EsBoolItem.FieldValue = nodeValue;
            }
            return node;
        }

        /// <summary>
        /// 重写值对象
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            EsBoolItem.FieldValue = node.ToConvertAndGetValue();
            //if (!string.IsNullOrEmpty(EsBoolItem.FieldName))
            //{
            //    EsBoolItem.FieldValue = node.ToConvertAndGetValue();
            //}
            //else
            //{
            //    var nodeValue = node.ToConvertAndGetValue();
            //    //switch (nodeValue)
            //    //{
            //    //    case true:
            //    //        SpliceField.Append("1=1");
            //    //        break;
            //    //    case false:
            //    //        SpliceField.Append("1!=1");
            //    //        break;
            //    //    default:
            //    //        SpliceField.Append(ParamName);
            //    //        Param.Add(ParamName, nodeValue);
            //    //        break;
            //    //}
            //}
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
                case "Contains":
                    {
                        if (node.Object != null && !(node.Object.Type.FullName.Contains("System.Collections.Generic")))
                        {
                            Visit(node.Object);
                            var value = node.Arguments[0].ToConvertAndGetValue();
                            EsBoolItem.BoolType = Entites.Es.Enum.EsBoolTypeEnum.Match;
                            EsBoolItem.FieldValue = value;
                        }
                        else
                        {
                            if (node.Object != null)
                            {
                                Visit(node.Arguments[0]);
                                EsBoolItem.FieldValue = base.EsBoolItem;
                                Visit(node.Object);
                            }
                            else
                            {
                                Visit(node.Arguments[1]);
                                EsBoolItem.FieldValue = base.EsBoolItem;
                            }
                        }
                    }
                    break;
                case "Equals":
                    {
                        if (node.Object != null)
                        {
                            Visit(node.Object);
                            //this.SpliceField.Append($" = ");
                            Visit(node.Arguments[0]);
                        }
                        else
                        {
                            Visit(node.Arguments[0]);
                            //this.SpliceField.Append($" = ");
                            Visit(node.Arguments[1]);
                        }
                    }
                    break;
                case "Between":
                    {
                        if (node.Object != null)
                        {
                            Visit(node.Object);
                            //SpliceField.Append(" BETWEEN ");
                            Visit(node.Arguments[0]);
                            //SpliceField.Append(" AND ");
                            Visit(node.Arguments[1]);
                        }
                        else
                        {
                            Visit(node.Arguments[0]);
                            //SpliceField.Append(" BETWEEN ");
                            Visit(node.Arguments[1]);
                            //SpliceField.Append(" AND ");
                            Visit(node.Arguments[2]);
                        }
                    }
                    break;
                default:
                    {
                        if (node.Object != null)
                            Visit(node.Object);
                        else
                            Visit(node.Arguments);
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 解析查询条件
    /// </summary>
    public sealed class WhereExpression : WhereExpressionVisitor
    {
        /// <summary>
        /// 
        /// </summary>
        public EsBoolItem EsBoolItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        public WhereExpression(LambdaExpression expression) : base()
        {
            Visit(expression);
            EsBoolItem = base.EsBoolItem;
        }

        /// <summary>
        /// 解析二元表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            var binary = new BinaryExpressionVisitor(node);
            EsBoolItem = binary.EsBoolItem;
            return node;
        }
    }
}
