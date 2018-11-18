using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Mapper
{
    public class Mapper<TSource, TDestination>
    {
        Func<TSource, TDestination> mapFunction;
        internal Mapper(Func<TSource, TDestination> func) { mapFunction = func; }
        public TDestination Map(TSource source) { return mapFunction(source); }
    }

    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource));
            var constructor = Expression.New(typeof(TDestination));
            var initialization = Expression.MemberInit(constructor, GetProperties<TSource, TDestination>(sourceParam));
            var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(initialization, sourceParam);
            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        private MemberAssignment[] GetProperties<TSource, TDestination>(ParameterExpression sourceParam)
        {
            var sourceProperties = sourceParam.Type.GetProperties();
            List<MemberAssignment> resultProperties = new List<MemberAssignment>();
            foreach (var prop in sourceProperties)
            {
                var destination = typeof (TDestination).GetProperties().FirstOrDefault(p => p.Name == prop.Name);
                if (destination!=null)
                {
                    var access = Expression.MakeMemberAccess(sourceParam, prop);
                    resultProperties.Add(Expression.Bind(destination, access));
                }
            }
            return resultProperties.ToArray();
        }
    }

    public class Foo
    {
        public int Count { get; set; }

        public Foo(int count)
        {
            Count = count;
        }
    }

    public class Bar
    {
        public int Count { get; set; }
        public Bar(int count)
        {
            Count = count;
        }

        public Bar()
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var res = mapper.Map(new Foo(5));
        }
    }
}
