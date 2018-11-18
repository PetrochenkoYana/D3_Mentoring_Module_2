using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Expressions
{
    interface ITransformator<TArg, TReturn>
    {
        Expression<Func<TArg, TReturn>> Transform(Expression<Func<TArg, TReturn>> lambda);
        Expression<Func<TArg, TReturn>> ReplaceParameters(Expression<Func<TArg, TReturn>> lambda, Dictionary<string, int> replaceParameters);
    }
}
