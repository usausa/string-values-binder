namespace StringValuesBinder;

using System.Reflection;

using Microsoft.Extensions.Primitives;

using StringConvertHelper;

#pragma warning disable CA1812
internal sealed class ConvertArrayPropertyMapper<T, TElement> : IPropertyMapper<T>
{
    private readonly string name;

    private readonly Action<T, TElement[]> setter;

    public ConvertArrayPropertyMapper(PropertyInfo pi)
    {
        name = pi.Name;
        setter = BindHelper.CreateSetter<T, TElement[]>(pi);
    }

    public void Map(T instance, Dictionary<string, StringValues> query)
    {
        if (query.TryGetValue(name, out var values))
        {
            var array = new TElement[values.Count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = StringConvert.TryConvert<TElement>(values[i], out var value) ? value : default!;
            }
            setter(instance, array);
        }
    }
}
#pragma warning restore CA1812
