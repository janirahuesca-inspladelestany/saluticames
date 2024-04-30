namespace Persistence.Data;

public class EnumLookup<T>
    where T : Enum
{
    public EnumLookup()
    {
    }

    public EnumLookup(T value, string description)
    {
        Id = Convert.ToInt32(value);
        Value = value;
        Name = description;
    }

    public int Id { get; set; }
    public T Value { get; set; }
    public string Name { get; set; }
}