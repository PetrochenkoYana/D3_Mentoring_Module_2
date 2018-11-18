using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Sample03
{
    public class ExpressionToFTSRequestTranslator : ExpressionVisitor
    {
        StringBuilder resultString;

        public string Translate(Expression exp)
        {
            resultString = new StringBuilder();
            Visit(exp);

            return resultString.ToString();
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }

            if (node.Method.DeclaringType == typeof(String)
               && node.Method.Name == "StartsWith")
            {
                var argument = (ConstantExpression)node.Arguments[0];
                var expression = Expression.Constant(argument.Value + "*");
                Visit(node.Object);
                Visit(expression);
                return node;
            }

            if (node.Method.DeclaringType == typeof(String)
             && node.Method.Name == "EndsWith")
            {
                var argument = (ConstantExpression)node.Arguments[0];
                var expression = Expression.Constant("*" + argument.Value);
                Visit(node.Object);
                Visit(expression);
                return node;
            }


            if (node.Method.DeclaringType == typeof(String)
             && node.Method.Name == "Contains")
            {
                var argument = (ConstantExpression)node.Arguments[0];
                var expression = Expression.Constant("*" + argument.Value +"*");
                Visit(node.Object);
                Visit(expression);
                return node;
            }
            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    //if (!(node.Left.NodeType == ExpressionType.MemberAccess))
                    //	throw new NotSupportedException(string.Format("Left operand should be property or field", node.NodeType));

                    //if (!(node.Right.NodeType == ExpressionType.Constant))
                    //	throw new NotSupportedException(string.Format("Right operand should be constant", node.NodeType));

                    Visit(node.Left);
                    Visit(node.Right);
                    break;

                default:
                    throw new NotSupportedException(string.Format("Operation {0} is not supported", node.NodeType));
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            if (resultString.Length != 0)
            {
                resultString.Insert(0, node.Member.Name + ":");
            }
            else
            {
                resultString.Append(node.Member.Name).Append(":");
            }

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            resultString.Append("(").Append(node.Value).Append(")");

            return node;
        }
    }
}
