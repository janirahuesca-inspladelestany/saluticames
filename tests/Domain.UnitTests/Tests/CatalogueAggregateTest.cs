using Domain.Content.Entities;
using Domain.Content.Errors;
using Domain.UnitTests.Helpers.Factories;
using FluentAssertions;

namespace Domain.UnitTests.Tests;

public class CatalogueAggregateTest
{
    [Fact]
    public void Create_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        string name = "El meu catalogue";
        Guid id = Guid.NewGuid();

        // Act
        var result = Catalogue.Create(name, id);

        // Assert
        result.IsSuccess().Should().BeTrue();
        var catalogue = result.Value!;
        catalogue.Id.Should().Be(id);
        catalogue.Name.Should().Be(name);
    }

    [Fact]
    public void RegisterSummitIds_WhenSummitIdsAreValid_ThenSuccess()
    {
        // Arrange
        var validSummitIds = new Guid[] { Guid.NewGuid(), Guid.NewGuid() };
        var sut = CatalogueFactory.Create();

        // Act
        var result = sut.RegisterSummitIds(validSummitIds);

        // Assert
        result.IsSuccess().Should().BeTrue();
    }

    [Fact]
    public void RegisterSummitIds_WhenAnySummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty;
        var validSummitId = Guid.NewGuid();
        var mixedSummitIds = new List<Guid> { validSummitId, invalidSummitId };
        var sut = CatalogueFactory.Create();

        // Act
        var result = sut.RegisterSummitIds(mixedSummitIds);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid);
    }

    [Fact]
    public void RegisterSummitIds_WhenSummitIdsIsNull_ThenFailureSummitIdNotValid()
    {
        // Arrange
        IEnumerable<Guid>? nullSummitIds = null;
        var sut = CatalogueFactory.Create();

        // Act
        var result = sut.RegisterSummitIds(nullSummitIds);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid);
    }

    [Fact]
    public void RegisterSummitIds_WhenInternalErrorOccurs_ThenFailureAndOriginalListRemainsIntact()
    {
        // Arrange
        var sut = CatalogueFactory.Create();
        var validSummitId = Guid.NewGuid();
        var invalidSummitId = Guid.Empty;
        var mixedSummitIds = new List<Guid> { validSummitId, invalidSummitId };
        var originalSummitIds = sut.SummitIds.ToList();

        // Act
        var result = sut.RegisterSummitIds(mixedSummitIds);

        // Assert
        result.IsFailure().Should().BeTrue();
        sut.SummitIds.Should().BeEquivalentTo(originalSummitIds);
    }

    [Fact]
    public void RegisterSummitId_WhenSummitIdIsValidAndNotRegistered_ThenSuccess()
    {
        // Arrange
        var validSummitId = Guid.NewGuid();
        var sut = CatalogueFactory.Create();

        // Act
        var result = sut.RegisterSummitId(validSummitId);

        // Assert
        result.IsSuccess().Should().BeTrue();
    }

    [Fact]
    public void RegisterSummitId_WhenSummitIdIsAlreadyRegistered_ThenFailureSummitIdAlreadyExists()
    {
        // Arrange
        var alreadyRegisteredSummitId = Guid.NewGuid();
        var sut = CatalogueFactory.CreateWithSummitIds(alreadyRegisteredSummitId);

        // Act
        var result = sut.RegisterSummitId(alreadyRegisteredSummitId);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(CatalogueErrors.SummitIdAlreadyExists);
    }

    [Fact]
    public void RegisterSummitId_WhenSummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty;
        var sut = CatalogueFactory.Create();

        // Act
        var result = sut.RegisterSummitId(invalidSummitId);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid);
    }

    [Fact]
    public void RemoveSummitIds_WhenSummitIdsAreValidAndRegistered_ThenSuccess()
    {
        // Arrange
        var registeredSummitIds = new Guid[] { Guid.NewGuid(), Guid.NewGuid() };
        var sut = CatalogueFactory.CreateWithSummitIds(registeredSummitIds);

        // Act
        var result = sut.RemoveSummitIds(registeredSummitIds);

        // Assert
        result.IsSuccess().Should().BeTrue();
    }

    [Fact]
    public void RemoveSummitIds_WhenAnySummitIdIsNotRegistered_ThenFailureSummitIdNotRegistered()
    {
        // Arrange
        var notRegisteredSummitId = Guid.NewGuid();
        var registeredSummitId = Guid.NewGuid();
        var mixedSummitIds = new List<Guid> { registeredSummitId, notRegisteredSummitId };
        var sut = CatalogueFactory.CreateWithSummitIds(registeredSummitId);

        // Act
        var result = sut.RemoveSummitIds(mixedSummitIds);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(CatalogueErrors.SummitIdNotRegistered);
    }

    [Fact]
    public void RemoveSummitIds_WhenAnySummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty;
        var validSummitId = Guid.NewGuid();
        var mixedSummitIds = new List<Guid> { validSummitId, invalidSummitId };
        var sut = CatalogueFactory.Create();

        // Act
        var result = sut.RemoveSummitIds(mixedSummitIds);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid);
    }

    [Fact]
    public void RemoveSummitIds_WhenSummitIdsIsNull_ThenFailureSummitIdNotValid()
    {
        // Arrange
        IEnumerable<Guid>? nullSummitIds = null;
        var sut = CatalogueFactory.Create();

        // Act
        var result = sut.RemoveSummitIds(nullSummitIds);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid);
    }

    [Fact]
    public void RemoveSummitIds_WhenInternalErrorOccurs_ThenFailureAndOriginalListRemainsIntact()
    {
        // Arrange
        var registeredSummitId = Guid.NewGuid();
        var notRegisteredSummitId = Guid.NewGuid();
        var mixedSummitIds = new List<Guid> { registeredSummitId, notRegisteredSummitId };
        var sut = CatalogueFactory.CreateWithSummitIds(registeredSummitId);
        var originalSummitIds = sut._catalogueSummit;

        // Act
        var result = sut.RemoveSummitIds(mixedSummitIds);

        // Assert
        result.IsFailure().Should().BeTrue();
        sut._catalogueSummit.Should().BeEquivalentTo(originalSummitIds);
    }

    [Fact]
    public void RemoveSummitId_WhenSummitIdIsValidAndRegistered_ThenSuccess()
    {
        // Arrange
        var validSummitId = Guid.NewGuid();
        var sut = CatalogueFactory.CreateWithSummitIds(validSummitId);

        // Act
        var result = sut.RemoveSummitId(validSummitId);

        // Assert
        result.IsSuccess().Should().BeTrue();
    }

    [Fact]
    public void RemoveSummitId_WhenSummitIdIsNotRegistered_ThenFailureSummitIdNotRegistered()
    {
        // Arrange
        var notRegisteredSummitId = Guid.NewGuid();
        var sut = CatalogueFactory.Create();

        // Act
        var result = sut.RemoveSummitId(notRegisteredSummitId);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(CatalogueErrors.SummitIdNotRegistered);
    }

    [Fact]
    public void RemoveSummitId_WhenSummitIdIsInvalid_ThenFailureSummitIdNotValid()
    {
        // Arrange
        var invalidSummitId = Guid.Empty;
        var sut = CatalogueFactory.Create();

        // Act
        var result = sut.RemoveSummitId(invalidSummitId);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(CatalogueErrors.SummitIdNotValid);
    }

}
