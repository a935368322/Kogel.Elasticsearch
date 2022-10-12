using Kogel.Elasticsearch.Entites.Es;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Kogel.Elasticsearch.Expressions
{
    /// <summary>
    /// 用于解析二元表达式
    /// </summary>
    public class BinaryExpressionVisitor : WhereExpressionVisitor
    {
        public BinaryExpressionVisitor(BinaryExpression expression)
        {
            EsBoolItem = new EsBoolItem();
            Visit(expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            EsBool esBool = new EsBool();
            var leftEsBoolItem = new EsBoolItem();
            esBool.Musts = new List<EsBoolItem>() { leftEsBoolItem };
            base.EsBoolItem = leftEsBoolItem;
            Visit(node.Left);
            var expressionType = node.GetExpressionType();
            var rightEsBoolItem = new EsBoolItem();
            base.EsBoolItem = rightEsBoolItem;
            if (expressionType?.Trim() == "AND")
            {
                esBool.Musts = new List<EsBoolItem>() { rightEsBoolItem };   
                Visit(node.Right);
            }
            else if (expressionType?.Trim() == "OR")
            {
                esBool.Shoulds = new List<EsBoolItem>() { rightEsBoolItem };
                Visit(node.Right);
            }
            else
            {
                esBool.Musts = new List<EsBoolItem>() { rightEsBoolItem };
                Visit(node.Right);
            }
            return node;
        }
    }
}
