using SharedKernel.Abstractions;

namespace Domain.Content.Events;

/* 
 * Classe que representa un esdeveniment de domini específic: la creació d'un cim dins del domini de contingut. 
 * Aquest esdeveniment pot ser utilitzat per notificar altres parts del sistema sobre la creació d'un nou cim, permetent-los reaccionar a aquest esdeveniment segons sigui necessari.
 * Finalment no s'ha realitzat cap implamentació.
*/
public sealed record SummitCreatedDomainEvent(Guid SummitId) : IDomainEvent;