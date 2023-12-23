namespace StringValuesBinder;

using System.Reflection;

using Microsoft.Extensions.Primitives;

using StringConvertHelper;

#pragma warning disable CA1812
internal sealed class ConvertPropertyMapper<T, TProperty> : IPropertyMapper<T>
{
    private readonly string name;

    private readonly Action<T, TProperty> setter;

    public ConvertPropertyMapper(PropertyInfo pi)
    {
        name = pi.Name;
        setter = BindHelper.CreateSetter<T, TProperty>(pi);
    }

    public void Map(T instance, Dictionary<string, StringValues> query)
    {
        if (query.TryGetValue(name, out var values) &&
            StringConvert.TryConvert<TProperty>(values, out var result))
        {
            setter(instance, result);
        }
    }
}
#pragma warning restore CA1812
