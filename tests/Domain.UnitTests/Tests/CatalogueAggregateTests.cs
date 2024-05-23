using Common.Helpers.Factories;
using Domain.Content.Entities;
using Domain.Content.Errors;
using FluentAssertions;

namespace Domain.UnitTests.Tests;

public class CatalogueAggregateTests
{
    /// <summary>
    ///  // Prova de la creació d'un catàleg amb paràmetres vàlids
    /// </summary>
    [Fact]
    public void Create_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        string name = "El meu catalogue"; // Nom del catàleg
        Guid id = Guid.NewGuid(); // Identificador únic

        // Act
        var result = CatalogueAggregate.Create(name, id); // Crear el catàleg

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que la creació ha tingut èxit
        var catalogue = result.Value!; // Obtenir el catàleg creat
        catalogue.Id.Should().Be(id); // Comprovar que l'ID coincideix
        catalogue.Name.Should().Be(name); // Comprovar que el nom coincideix
    }

    /// <summary>
    /// Prova de registre d'identificadors vàlids de "summit"
    /// </summary>
    [Fact]
    public void RegisterSummitIds_WhenSummitIdsAreValid_ThenSuccess()
    {
        // Arrange
        var validSummitIds = new Guid[] { Guid.NewGuid(), Guid.NewGuid() }; // Identificadors vàlids
        var sut = CatalogueFactory.Create(); // Crear el catàleg de prova

        // Act
        var result = sut.RegisterSummitIds(validSummitIds); // Registrar els identificadors

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
    }


    /// <summary>
    /// Prova de registre d'identificadors quan hi ha un identificador no vàlid
    /// </summary>
    [Fact]
    public void RegisterSummitIds_WhenAnySummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty; // Identificador no vàlid
        var validSummitId = Guid.NewGuid(); // Identificador vàlid
        var mixedSummitIds = new List<Guid> { validSummitId, invalidSummitId }; // Llista mixta
        var sut = CatalogueFactory.Create(); // Crear el catàleg de prova

        // Act
        var result = sut.RegisterSummitIds(mixedSummitIds); // Intentar registrar els identificadors

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de registre d'identificadors quan la llista és nul·la
    /// </summary>
    [Fact]
    public void RegisterSummitIds_WhenSummitIdsIsNull_ThenFailureSummitIdNotValid()
    {
        // Arrange
        IEnumerable<Guid>? nullSummitIds = null; // Identificadors nuls
        var sut = CatalogueFactory.Create(); // Crear el catàleg de prova

        // Act
        var result = sut.RegisterSummitIds(nullSummitIds); // Intentar registrar identificadors nuls

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de registre d'identificadors quan hi ha un error intern
    /// </summary>
    [Fact]
    public void RegisterSummitIds_WhenInternalErrorOccurs_ThenFailureAndOriginalListRemainsIntact()
    {
        // Arrange
        var sut = CatalogueFactory.Create(); // Crear el catàleg de prova
        var validSummitId = Guid.NewGuid(); // Identificador vàlid
        var invalidSummitId = Guid.Empty; // Identificador no vàlid
        var mixedSummitIds = new List<Guid> { validSummitId, invalidSummitId }; // Llista mixta
        var originalSummitIds = sut.SummitIds.ToList(); // Llista original d'identificadors

        // Act
        var result = sut.RegisterSummitIds(mixedSummitIds); // Intentar registrar els identificadors

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        sut.SummitIds.Should().BeEquivalentTo(originalSummitIds); // Comprovar que la llista original no ha canviat
    }

    /// <summary>
    /// // Prova de registre d'un identificador vàlid i no registrat
    /// </summary>
    [Fact]
    public void RegisterSummitId_WhenSummitIdIsValidAndNotRegistered_ThenSuccess()
    {
        // Arrange
        var validSummitId = Guid.NewGuid(); // Identificador vàlid
        var sut = CatalogueFactory.Create(); // Crear el catàleg de prova

        // Act
        var result = sut.RegisterSummitId(validSummitId); // Registrar l'identificador

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
    }

    /// <summary>
    /// rova de registre d'un identificador ja registrat
    /// </summary>
    [Fact]
    public void RegisterSummitId_WhenSummitIdIsAlreadyRegistered_ThenFailureSummitIdAlreadyExists()
    {
        // Arrange
        var alreadyRegisteredSummitId = Guid.NewGuid(); // Identificador ja registrat
        var sut = CatalogueFactory.CreateWithSummitIds(alreadyRegisteredSummitId); // Crear el catàleg de prova amb l'identificador ja registrat

        // Act
        var result = sut.RegisterSummitId(alreadyRegisteredSummitId); // Intentar registrar l'identificador de nou

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdAlreadyExists); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de registre d'un identificador no vàlid
    /// </summary>
    [Fact]
    public void RegisterSummitId_WhenSummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty; // Identificador no vàlid
        var sut = CatalogueFactory.Create(); // Crear el catàleg de prova

        // Act
        var result = sut.RegisterSummitId(invalidSummitId); // Intentar registrar l'identificador no vàlid

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de l'eliminació d'identificadors vàlids i registrats
    /// </summary>
    [Fact]
    public void RemoveSummitIds_WhenSummitIdsAreValidAndRegistered_ThenSuccess()
    {
        // Arrange
        var registeredSummitIds = new Guid[] { Guid.NewGuid(), Guid.NewGuid() }; // Identificadors registrats
        var sut = CatalogueFactory.CreateWithSummitIds(registeredSummitIds); // Crear el catàleg de prova amb identificadors registrats

        // Act
        var result = sut.RemoveSummitIds(registeredSummitIds); // Eliminar els identificadors

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
    }

    /// <summary>
    /// Prova de l'eliminació d'identificadors quan hi ha un identificador no registrat
    /// </summary>
    [Fact]
    public void RemoveSummitIds_WhenAnySummitIdIsNotRegistered_ThenFailureSummitIdNotRegistered()
    {
        // Arrange
        var notRegisteredSummitId = Guid.NewGuid(); // Identificador no registrat
        var registeredSummitId = Guid.NewGuid(); // Identificador registrat
        var mixedSummitIds = new List<Guid> { registeredSummitId, notRegisteredSummitId }; // Llista mixta
        var sut = CatalogueFactory.CreateWithSummitIds(registeredSummitId); // Crear el catàleg de prova amb identificador registrat

        // Act
        var result = sut.RemoveSummitIds(mixedSummitIds); // Intentar eliminar els identificadors

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotRegistered); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de l'eliminació d'identificadors quan hi ha un identificador no vàlid
    /// </summary>
    [Fact]
    public void RemoveSummitIds_WhenAnySummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty; // Identificador no vàlid
        var validSummitId = Guid.NewGuid(); // Identificador vàlid
        var mixedSummitIds = new List<Guid> { validSummitId, invalidSummitId }; // Llista mixta
        var sut = CatalogueFactory.Create(); // Crear el catàleg de prova

        // Act
        var result = sut.RemoveSummitIds(mixedSummitIds); // Intentar eliminar els identificadors

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de l'eliminació d'identificadors quan la llista és nul·la
    /// </summary>
    [Fact]
    public void RemoveSummitIds_WhenSummitIdsIsNull_ThenFailureSummitIdNotValid()
    {
        // Arrange
        IEnumerable<Guid>? nullSummitIds = null; // Identificadors nul·ls
        var sut = CatalogueFactory.Create(); // Crear el catàleg de prova

        // Act
        var result = sut.RemoveSummitIds(nullSummitIds); // Intentar eliminar identificadors nul·ls

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de l'eliminació d'identificadors quan hi ha un error intern
    /// </summary>
    [Fact]
    public void RemoveSummitIds_WhenInternalErrorOccurs_ThenFailureAndOriginalListRemainsIntact()
    {
        // Arrange
        var registeredSummitId = Guid.NewGuid(); // Identificador registrat
        var notRegisteredSummitId = Guid.NewGuid(); // Identificador no registrat
        var mixedSummitIds = new List<Guid> { registeredSummitId, notRegisteredSummitId }; // Llista mixta
        var sut = CatalogueFactory.CreateWithSummitIds(registeredSummitId); // Crear el catàleg de prova amb identificador registrat
        var originalSummitIds = sut._catalogueSummit; // Llista original d'identificadors

        // Act
        var result = sut.RemoveSummitIds(mixedSummitIds); // Intentar eliminar els identificadors

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        sut._catalogueSummit.Should().BeEquivalentTo(originalSummitIds); // Comprovar que la llista original no ha canviat
    }

    /// <summary>
    /// Prova de l'eliminació d'un identificador vàlid i registrat
    /// </summary>
    [Fact]
    public void RemoveSummitId_WhenSummitIdIsValidAndRegistered_ThenSuccess()
    {
        // Arrange
        var validSummitId = Guid.NewGuid(); // Identificador vàlid
        var sut = CatalogueFactory.CreateWithSummitIds(validSummitId); // Crear el catàleg de prova amb identificador registrat

        // Act
        var result = sut.RemoveSummitId(validSummitId); // Eliminar l'identificador

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
    }

    /// <summary>
    /// Prova de l'eliminació d'un identificador no registrat
    /// </summary>
    [Fact]
    public void RemoveSummitId_WhenSummitIdIsNotRegistered_ThenFailureSummitIdNotRegistered()
    {
        // Arrange
        var notRegisteredSummitId = Guid.NewGuid(); // Identificador no registrat
        var sut = CatalogueFactory.Create(); // Crear el catàleg de prova

        // Act
        var result = sut.RemoveSummitId(notRegisteredSummitId); // Intentar eliminar l'identificador no registrat

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotRegistered); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova de l'eliminació d'un identificador no vàlid
    /// </summary>
    [Fact]
    public void RemoveSummitId_WhenSummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty; // Identificador no vàlid
        var sut = CatalogueFactory.Create(); // Crear el catàleg de prova

        // Act
        var result = sut.RemoveSummitId(invalidSummitId); // Intentar eliminar l'identificador no vàlid

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid); // Comprovar l'error esperat
    }

}
