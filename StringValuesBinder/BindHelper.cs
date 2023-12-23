namespace StringValuesBinder;

using System.Linq.Expressions;
using System.Reflection;

internal static class BindHelper
{
    public static Func<TTarget> CreateFactory<TTarget>(ConstructorInfo ci)
    {
        return Expression.Lambda<Func<TTarget>>(Expression.New(ci)).Compile();
    }

    public static Action<TTarget, TMember> CreateSetter<TTarget, TMember>(PropertyInfo pi)
    {
        var parameterExpression = Expression.Parameter(typeof(TTarget));
        var parameterExpression2 = Expression.Parameter(typeof(TMember));
        var propertyExpression = Expression.Property(parameterExpression, pi);
        return Expression.Lambda<Action<TTarget, TMember>>(
            Expression.Assign(propertyExpression, parameterExpression2),
            parameterExpression,
            parameterExpression2).Compile();
    }
}
