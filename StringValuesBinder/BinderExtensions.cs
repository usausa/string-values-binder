namespace StringValuesBinder;

using System.Collections.Concurrent;

using Microsoft.Extensions.Primitives;

using StringConvertHelper;

public static class BinderExtensions
{
    private static readonly ConcurrentDictionary<Type, object> Bindings = new();

    public static T Bind<T>(this Dictionary<string, StringValues> query)
    {
        var binding = (BindingInfo<T>)Bindings.GetOrAdd(typeof(T), static x =>
        {
            var ci = x.GetConstructor(Type.EmptyTypes);
            if (ci is null)
            {
                throw new NotSupportedException($"Default constructor is required. type=[{x}]");
            }

            var factory = BindHelper.CreateFactory<T>(ci);

            var mappers = new List<IPropertyMapper<T>>();
            foreach (var pi in x.GetProperties())
            {
                if (!pi.CanWrite)
                {
                    continue;
                }

                if (pi.PropertyType == typeof(string))
                {
                    mappers.Add(new StringPropertyMapper<T>(pi));
                }
                else if (pi.PropertyType.IsArray)
                {
                    if (pi.PropertyType.GetElementType() == typeof(string))
                    {
                        mappers.Add(new StringArrayPropertyMapper<T>(pi));
                    }
                    else
                    {
                        var mapperType = typeof(ConvertArrayPropertyMapper<,>).MakeGenericType(x, pi.PropertyType.GetElementType()!);
                        mappers.Add((IPropertyMapper<T>)Activator.CreateInstance(mapperType, pi)!);
                    }
                }
                else
                {
                    var mapperType = typeof(ConvertPropertyMapper<,>).MakeGenericType(x, pi.PropertyType);
                    mappers.Add((IPropertyMapper<T>)Activator.CreateInstance(mapperType, pi)!);
                }
            }

            return new BindingInfo<T>
            {
                Factory = factory,
                Mappers = [.. mappers]
            };
        });

        var instance = binding.Factory();
        foreach (var mapper in binding.Mappers)
        {
            mapper.Map(instance, query);
        }

        return instance;
    }

    public static bool TryGetValue<T>(this Dictionary<string, StringValues> query, string key, out T result)
    {
        if (query.TryGetValue(key, out var value) &&
            StringConvert.TryConvert(value, out result))
        {
            return true;
        }

        result = default!;
        return false;
    }

    public static T GetValueOrDefault<T>(this Dictionary<string, StringValues> query, string key, T defaultValue = default!)
    {
        if (query.TryGetValue(key, out var value) &&
            StringConvert.TryConvert<T>(value, out var result))
        {
            return result;
        }

        return defaultValue;
    }

    public static T[] GetValuesOrDefault<T>(this Dictionary<string, StringValues> query, string key)
    {
        if (query.TryGetValue(key, out var values))
        {
            var array = new T[values.Count];
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = StringConvert.TryConvert<T>(values[i], out var result) ? result : default!;
            }

            return array;
        }

        return [];
    }
}
