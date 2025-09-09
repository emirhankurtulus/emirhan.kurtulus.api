namespace emirhan.kurtulus.api.core.Attributes;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class CollectionNameAttribute(string name) : Attribute
{
    public string Name { get; } = name;
}