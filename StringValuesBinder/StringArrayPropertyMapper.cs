namespace StringValuesBinder;

using System.Reflection;

using Microsoft.Extensions.Primitives;

internal sealed class StringArrayPropertyMapper<T> : IPropertyMapper<T>
{
    private readonly string name;

    private readonly Action<T, string?[]?> setter;

    public StringArrayPropertyMapper(PropertyInfo pi)
    {
        name = pi.Name;
        setter = BindHelper.CreateSetter<T, string?[]?>(pi);
    }

    public void Map(T instance, Dictionary<string, StringValues> query)
    {
        if (query.TryGetValue(name, out var values))
        {
            setter(instance, values);
        }
    }
}
