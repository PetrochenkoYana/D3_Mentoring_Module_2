using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Expressions
{
    class Program
    {
        static void Main(string[] args)
        {
            Func<int, int> func = (a) => a + 1;
            Expression<Func<int, int>> expression = (a) => a + a + 1 + (Int32.Parse("5") - 1 + (int)1.0) - 1 + a;
            var incrementTransformator = new TransformatorIncrement<int,int>();
            var transformedExpression = incrementTransformator.Transform(expression);
            var result = transformedExpression.Compile().Invoke(6);
            var afterreplacing = incrementTransformator.ReplaceParameters(expression, new Dictionary<string, int>() { { "a", 5 } });
            var result2 = afterreplacing.Compile().Invoke(6);
            Console.ReadKey();
        }
    }


}
