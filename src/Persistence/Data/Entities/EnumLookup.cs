namespace Persistence.Data.Entities;

public class EnumLookup<T>
    where T : Enum
{
    // Constructor sense paràmetres
    public EnumLookup()
    {
    }

    // Constructor amb paràmetres per assignar valors a les propietats
    public EnumLookup(T value, string description)
    {
        Id = Convert.ToInt32(value); // Convertir el valor de l'enumeració a un enter
        Value = value; // Assignar el valor de l'enumeració
        Name = description; // Assignar la descripció
    }

    // Identificador de l'entrada
    public int Id { get; set; }

    // Valor de l'enumeració
    public T Value { get; set; }

    // Descripció de l'entrada
    public string Name { get; set; }
}