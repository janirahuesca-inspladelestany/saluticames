using Common.Helpers.Factories;
using Domain.Content.Entities;
using Domain.Content.Errors;
using FluentAssertions;

namespace Domain.UnitTests.Tests;

public class CatalogueAggregateTests
{
    /// <summary>
    ///  // Prova de la creaci� d'un cat�leg amb par�metres v�lids
    /// </summary>
    [Fact]
    public void Create_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        string name = "El meu catalogue"; // Nom del cat�leg
        Guid id = Guid.NewGuid(); // Identificador �nic

        // Act
        var result = CatalogueAggregate.Create(name, id); // Crear el cat�leg

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que la creaci� ha tingut �xit
        var catalogue = result.Value!; // Obtenir el cat�leg creat
        catalogue.Id.Should().Be(id); // Comprovar que l'ID coincideix
        catalogue.Name.Should().Be(name); // Comprovar que el nom coincideix
    }

    /// <summary>
    /// Prova de registre d'identificadors v�lids de "summit"
    /// </summary>
    [Fact]
    public void RegisterSummitIds_WhenSummitIdsAreValid_ThenSuccess()
    {
        // Arrange
        var validSummitIds = new Guid[] { Guid.NewGuid(), Guid.NewGuid() }; // Identificadors v�lids
        var sut = CatalogueFactory.Create(); // Crear el cat�leg de prova

        // Act
        var result = sut.RegisterSummitIds(validSummitIds); // Registrar els identificadors

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operaci� ha tingut �xit
    }


    /// <summary>
    /// Prova de registre d'identificadors quan hi ha un identificador no v�lid
    /// </summary>
    [Fact]
    public void RegisterSummitIds_WhenAnySummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty; // Identificador no v�lid
        var validSummitId = Guid.NewGuid(); // Identificador v�lid
        var mixedSummitIds = new List<Guid> { validSummitId, invalidSummitId }; // Llista mixta
        var sut = CatalogueFactory.Create(); // Crear el cat�leg de prova

        // Act
        var result = sut.RegisterSummitIds(mixedSummitIds); // Intentar registrar els identificadors

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de registre d'identificadors quan la llista �s nul�la
    /// </summary>
    [Fact]
    public void RegisterSummitIds_WhenSummitIdsIsNull_ThenFailureSummitIdNotValid()
    {
        // Arrange
        IEnumerable<Guid>? nullSummitIds = null; // Identificadors nuls
        var sut = CatalogueFactory.Create(); // Crear el cat�leg de prova

        // Act
        var result = sut.RegisterSummitIds(nullSummitIds); // Intentar registrar identificadors nuls

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de registre d'identificadors quan hi ha un error intern
    /// </summary>
    [Fact]
    public void RegisterSummitIds_WhenInternalErrorOccurs_ThenFailureAndOriginalListRemainsIntact()
    {
        // Arrange
        var sut = CatalogueFactory.Create(); // Crear el cat�leg de prova
        var validSummitId = Guid.NewGuid(); // Identificador v�lid
        var invalidSummitId = Guid.Empty; // Identificador no v�lid
        var mixedSummitIds = new List<Guid> { validSummitId, invalidSummitId }; // Llista mixta
        var originalSummitIds = sut.SummitIds.ToList(); // Llista original d'identificadors

        // Act
        var result = sut.RegisterSummitIds(mixedSummitIds); // Intentar registrar els identificadors

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        sut.SummitIds.Should().BeEquivalentTo(originalSummitIds); // Comprovar que la llista original no ha canviat
    }

    /// <summary>
    /// // Prova de registre d'un identificador v�lid i no registrat
    /// </summary>
    [Fact]
    public void RegisterSummitId_WhenSummitIdIsValidAndNotRegistered_ThenSuccess()
    {
        // Arrange
        var validSummitId = Guid.NewGuid(); // Identificador v�lid
        var sut = CatalogueFactory.Create(); // Crear el cat�leg de prova

        // Act
        var result = sut.RegisterSummitId(validSummitId); // Registrar l'identificador

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operaci� ha tingut �xit
    }

    /// <summary>
    /// rova de registre d'un identificador ja registrat
    /// </summary>
    [Fact]
    public void RegisterSummitId_WhenSummitIdIsAlreadyRegistered_ThenFailureSummitIdAlreadyExists()
    {
        // Arrange
        var alreadyRegisteredSummitId = Guid.NewGuid(); // Identificador ja registrat
        var sut = CatalogueFactory.CreateWithSummitIds(alreadyRegisteredSummitId); // Crear el cat�leg de prova amb l'identificador ja registrat

        // Act
        var result = sut.RegisterSummitId(alreadyRegisteredSummitId); // Intentar registrar l'identificador de nou

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdAlreadyExists); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de registre d'un identificador no v�lid
    /// </summary>
    [Fact]
    public void RegisterSummitId_WhenSummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty; // Identificador no v�lid
        var sut = CatalogueFactory.Create(); // Crear el cat�leg de prova

        // Act
        var result = sut.RegisterSummitId(invalidSummitId); // Intentar registrar l'identificador no v�lid

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de l'eliminaci� d'identificadors v�lids i registrats
    /// </summary>
    [Fact]
    public void RemoveSummitIds_WhenSummitIdsAreValidAndRegistered_ThenSuccess()
    {
        // Arrange
        var registeredSummitIds = new Guid[] { Guid.NewGuid(), Guid.NewGuid() }; // Identificadors registrats
        var sut = CatalogueFactory.CreateWithSummitIds(registeredSummitIds); // Crear el cat�leg de prova amb identificadors registrats

        // Act
        var result = sut.RemoveSummitIds(registeredSummitIds); // Eliminar els identificadors

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operaci� ha tingut �xit
    }

    /// <summary>
    /// Prova de l'eliminaci� d'identificadors quan hi ha un identificador no registrat
    /// </summary>
    [Fact]
    public void RemoveSummitIds_WhenAnySummitIdIsNotRegistered_ThenFailureSummitIdNotRegistered()
    {
        // Arrange
        var notRegisteredSummitId = Guid.NewGuid(); // Identificador no registrat
        var registeredSummitId = Guid.NewGuid(); // Identificador registrat
        var mixedSummitIds = new List<Guid> { registeredSummitId, notRegisteredSummitId }; // Llista mixta
        var sut = CatalogueFactory.CreateWithSummitIds(registeredSummitId); // Crear el cat�leg de prova amb identificador registrat

        // Act
        var result = sut.RemoveSummitIds(mixedSummitIds); // Intentar eliminar els identificadors

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotRegistered); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de l'eliminaci� d'identificadors quan hi ha un identificador no v�lid
    /// </summary>
    [Fact]
    public void RemoveSummitIds_WhenAnySummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty; // Identificador no v�lid
        var validSummitId = Guid.NewGuid(); // Identificador v�lid
        var mixedSummitIds = new List<Guid> { validSummitId, invalidSummitId }; // Llista mixta
        var sut = CatalogueFactory.Create(); // Crear el cat�leg de prova

        // Act
        var result = sut.RemoveSummitIds(mixedSummitIds); // Intentar eliminar els identificadors

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de l'eliminaci� d'identificadors quan la llista �s nul�la
    /// </summary>
    [Fact]
    public void RemoveSummitIds_WhenSummitIdsIsNull_ThenFailureSummitIdNotValid()
    {
        // Arrange
        IEnumerable<Guid>? nullSummitIds = null; // Identificadors nul�ls
        var sut = CatalogueFactory.Create(); // Crear el cat�leg de prova

        // Act
        var result = sut.RemoveSummitIds(nullSummitIds); // Intentar eliminar identificadors nul�ls

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de l'eliminaci� d'identificadors quan hi ha un error intern
    /// </summary>
    [Fact]
    public void RemoveSummitIds_WhenInternalErrorOccurs_ThenFailureAndOriginalListRemainsIntact()
    {
        // Arrange
        var registeredSummitId = Guid.NewGuid(); // Identificador registrat
        var notRegisteredSummitId = Guid.NewGuid(); // Identificador no registrat
        var mixedSummitIds = new List<Guid> { registeredSummitId, notRegisteredSummitId }; // Llista mixta
        var sut = CatalogueFactory.CreateWithSummitIds(registeredSummitId); // Crear el cat�leg de prova amb identificador registrat
        var originalSummitIds = sut._catalogueSummit; // Llista original d'identificadors

        // Act
        var result = sut.RemoveSummitIds(mixedSummitIds); // Intentar eliminar els identificadors

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        sut._catalogueSummit.Should().BeEquivalentTo(originalSummitIds); // Comprovar que la llista original no ha canviat
    }

    /// <summary>
    /// Prova de l'eliminaci� d'un identificador v�lid i registrat
    /// </summary>
    [Fact]
    public void RemoveSummitId_WhenSummitIdIsValidAndRegistered_ThenSuccess()
    {
        // Arrange
        var validSummitId = Guid.NewGuid(); // Identificador v�lid
        var sut = CatalogueFactory.CreateWithSummitIds(validSummitId); // Crear el cat�leg de prova amb identificador registrat

        // Act
        var result = sut.RemoveSummitId(validSummitId); // Eliminar l'identificador

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operaci� ha tingut �xit
    }

    /// <summary>
    /// Prova de l'eliminaci� d'un identificador no registrat
    /// </summary>
    [Fact]
    public void RemoveSummitId_WhenSummitIdIsNotRegistered_ThenFailureSummitIdNotRegistered()
    {
        // Arrange
        var notRegisteredSummitId = Guid.NewGuid(); // Identificador no registrat
        var sut = CatalogueFactory.Create(); // Crear el cat�leg de prova

        // Act
        var result = sut.RemoveSummitId(notRegisteredSummitId); // Intentar eliminar l'identificador no registrat

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotRegistered); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de l'eliminaci� d'un identificador no v�lid
    /// </summary>
    [Fact]
    public void RemoveSummitId_WhenSummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty; // Identificador no v�lid
        var sut = CatalogueFactory.Create(); // Crear el cat�leg de prova

        // Act
        var result = sut.RemoveSummitId(invalidSummitId); // Intentar eliminar l'identificador no v�lid

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

}
