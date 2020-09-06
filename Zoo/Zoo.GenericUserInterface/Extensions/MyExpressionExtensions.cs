using System;
using System.Linq.Expressions;

namespace Zoo.GenericUserInterface.Extensions
{
    internal static class MyExpressionExtensions
    {
        internal static string GetMemberName<TModel, TProp>(Expression<Func<TModel, TProp>> expression)
        {
            return (expression.Body as MemberExpression).Member.Name;
        }
    }
}