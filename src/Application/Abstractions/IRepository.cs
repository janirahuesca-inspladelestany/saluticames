using SharedKernel.Abstractions; 

namespace Application.Abstractions
{
    // Interfície IRepository que declara operacions genèriques per a la manipulació de dades
    public interface IRepository<T, TId>
        where T : AggregateRoot<TId> // Requeriment de tipus genèric que T sigui un AggregateRoot amb un tipus d'identificador TId
    {
        // No hi ha mètodes definits en aquesta interfície ja que és una interfície genèrica que s'espera que les implementacions específiques proporcionin mètodes per a la manipulació de dades
    }
}
