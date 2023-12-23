namespace StringValuesBinder;

using Microsoft.Extensions.Primitives;

internal interface IPropertyMapper<in T>
{
    void Map(T instance, Dictionary<string, StringValues> query);
}
