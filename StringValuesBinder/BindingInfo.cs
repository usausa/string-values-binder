namespace StringValuesBinder;

#pragma warning disable SA1401 // Fields should be private
internal sealed class BindingInfo<T>
{
    public Func<T> Factory = default!;

    public IPropertyMapper<T>[] Mappers = default!;
}
#pragma warning restore SA1401 // Fields should be private
