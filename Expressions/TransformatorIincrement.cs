using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expressions
{
    class TransformatorIncrement<TArg, TReturn> : ExpressionVisitor, ITransformator<TArg, TReturn>
    {
        public Dictionary<string, int> ReplacedParameters = null;
        public Expression<Func<TArg, TReturn>> Transform(Expression<Func<TArg, TReturn>> lambda)
        {
            return VisitAndConvert(lambda, "");
        }

        public Expression<Func<TArg, TReturn>> ReplaceParameters(Expression<Func<TArg, TReturn>> lambda, Dictionary<string, int> replaceParameters)
        {
            ReplacedParameters = replaceParameters;
            return VisitAndConvert(lambda,"");
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            object parameter = null;
            if (ReplacedParameters != null)
                parameter = ReplacedParameters[node.Name];
            return parameter != null ? Expression.Constant(parameter) : base.VisitParameter(node);
        }
        protected override Expression VisitLambda<T>(Expression<T> node) => Expression.Lambda(Visit(node.Body), node.Parameters);
        protected override Expression VisitBinary(BinaryExpression nod)
        {
            var node = nod;
            if (node.NodeType == ExpressionType.Add)
            {
                if ((node.Right.NodeType == ExpressionType.Constant && (int)((ConstantExpression)node.Right).Value == 1))
                {
                    var res = base.Visit(node.Left);
                    return Expression.Increment(res);
                }
            }

            if (node.NodeType == ExpressionType.Subtract)
            {
                if (node.Right.NodeType == ExpressionType.Constant && node.Right.Type == typeof(int) &&
                    (int)((ConstantExpression)node.Right).Value == 1)
                {
                    var res = base.Visit(node.Left);
                    return Expression.Decrement(res);
                }
            }

            return base.VisitBinary(node);
        }
    }
}
