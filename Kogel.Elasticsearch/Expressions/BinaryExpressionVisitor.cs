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
        EsBool _esBool;
        public BinaryExpressionVisitor(BinaryExpression expression)
        {
            EsBoolItem = new EsBoolItem();
            _esBool = new EsBool
            {
                Musts = new List<EsBoolItem>(),
                Shoulds = new List<EsBoolItem>()
            };
            Visit(expression);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);
            var expressionType = node.GetExpressionType()?.Trim();

            if (expressionType == "AND")
            {


                Visit(node.Right);
            }
            else if (expressionType == "OR")
            {
                Visit(node.Right);
            }
            else
            {
                if (expressionType == "=")
                {
                    EsBoolItem.BoolType = Entites.Es.Enum.EsBoolTypeEnum.Term;
                }
                Visit(node.Right);

            }
            return node;
        }
    }
}
