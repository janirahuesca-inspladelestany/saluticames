using Application.Abstractions;
using Application.Content.Repositories;
using Application.Content.Services;
using Common.Helpers.Factories;
using Contracts.DTO.Content;
using Domain.Content.Entities;
using Domain.Content.Errors;
using FluentAssertions;
using Moq;
using SharedKernel.Common;
using System.Linq.Expressions;

namespace Application.UnitTests.Tests;

public class CatalogueServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICatalogueRepository> _catalogueRepositoryMock;
    private readonly CatalogueService _sut;

    public CatalogueServiceTests()
    {
        // Configuraci� dels mocks i del servei CatalogueService per a cada test
        _catalogueRepositoryMock = new Mock<ICatalogueRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.SetupGet(u => u.CatalogueRepository).Returns(_catalogueRepositoryMock.Object);
        _sut = new CatalogueService(_unitOfWorkMock.Object);
    }

    /// <summary>
    /// Prova d'afegir nous identificadors de cims al cat�leg i comprovar que t� �xit
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CatalogueService_WhenAddingNewSummitIds_ShouldReturnSuccess()
    {
        // Arrange
        var catalogueId = Guid.NewGuid(); // Identificador del cat�leg
        var summitIds = new List<Guid> { Guid.NewGuid() }; // Llista d'identificadors de cims a afegir
        var catalogue = CatalogueFactory.Create(); // Crear un cat�leg de prova

        _catalogueRepositoryMock
            .Setup(r => r.FindByIdAsync(catalogueId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(catalogue); // Configurar el mock per retornar el cat�leg

        // Act
        var result = await _sut.AddNewSummitIdsInCatalogueAsync(catalogueId, summitIds); // Afegir els nous identificadors de cims

        // Assert
        _catalogueRepositoryMock.Verify(r => r.FindByIdAsync(catalogueId, It.IsAny<CancellationToken>()), Times.Once); // Comprovar que el m�tode FindByIdAsync es crida una vegada
        result.Should().NotBeNull(); // Comprovar que el resultat no �s null
        result.Should().BeOfType<EmptyResult<Error>>(); // Comprovar que el tipus de resultat �s el correcte
        result.IsFailure().Should().BeFalse(); // Comprovar que l'operaci� ha tingut �xit
    }

    /// <summary>
    /// Prova d'afegir un identificador de cim existent al cat�leg i comprovar que falla amb l'error SummitIdAlreadyExists
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CatalogueService_WhenAddingExistingSummitId_ShouldReturnSummitIdAlreadyExistsError()
    {
        // Arrange
        var catalogueId = Guid.NewGuid(); // Identificador del cat�leg
        var existingSummitId = Guid.NewGuid(); // Identificador de cim existent
        var summitIds = new List<Guid> { existingSummitId }; // Llista d'identificadors de cims a afegir
        var catalogue = CatalogueFactory.CreateWithSummitIds(existingSummitId); // Crear un cat�leg amb el cim existent

        _catalogueRepositoryMock
            .Setup(r => r.FindByIdAsync(catalogueId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(catalogue); // Configurar el mock per retornar el cat�leg

        // Act
        var result = await _sut.AddNewSummitIdsInCatalogueAsync(catalogueId, summitIds); // Intentar afegir l'identificador de cim existent

        // Assert
        _catalogueRepositoryMock.Verify(r => r.FindByIdAsync(catalogueId, It.IsAny<CancellationToken>()), Times.Once); // Comprovar que el m�tode FindByIdAsync es crida una vegada
        result.Should().NotBeNull(); // Comprovar que el resultat no �s null
        result.Should().BeOfType<EmptyResult<Error>>(); // Comprovar que el tipus de resultat �s el correcte
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().BeEquivalentTo(CatalogueErrors.SummitIdAlreadyExists); // Comprovar que l'error �s l'esperat
    }

    /// <summary>
    /// Prova de llistar tots els cat�legs i comprovar que es retornen correctament
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CatalogueService_WhenListingAllCatalogues_ShouldReturnAllCatalogues()
    {
        // Arrange
        var filterDto = new ListCataloguesFilterDto(); // Filtre per llistar cat�legs
        var catalogueList = new List<CatalogueAggregate>
        {
            CatalogueFactory.Create(), // Crear un cat�leg de prova
            CatalogueFactory.Create()  // Crear un altre cat�leg de prova
        };

        _catalogueRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(catalogueList); // Configurar el mock per retornar la llista de cat�legs

        // Act
        var result = await _sut.ListCatalogues(filterDto); // Llistar els cat�legs

        // Assert
        _catalogueRepositoryMock
            .Verify(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<CancellationToken>()), Times.Once); // Comprovar que el m�tode ListAsync es crida una vegada
        result.Should().NotBeNull(); // Comprovar que el resultat no �s null
        result.IsFailure().Should().BeFalse(); // Comprovar que l'operaci� ha tingut �xit
        result.Value.Should().HaveCount(catalogueList.Count); // Comprovar que el nombre de cat�legs retornats �s correcte
    }

    /// <summary>
    /// Prova de llistar cat�legs amb un nom espec�fic i comprovar que es retornen correctament
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CatalogueService_WhenListingCataloguesWithSpecificName_ShouldReturnFilteredCatalogues()
    {
        // Arrange
        var filterDto = new ListCataloguesFilterDto(Name: "El meu"); // Filtre per llistar cat�legs amb un nom espec�fic
        var catalogue = CatalogueFactory.Create(); // Crear un cat�leg de prova

        _catalogueRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CatalogueAggregate> { catalogue }); // Configurar el mock per retornar el cat�leg amb el nom espec�fic

        // Act
        var result = await _sut.ListCatalogues(filterDto); // Llistar els cat�legs

        // Assert
        _catalogueRepositoryMock
            .Verify(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<CancellationToken>()), Times.Once); // Comprovar que el m�tode ListAsync es crida una vegada
        result.Should().NotBeNull(); // Comprovar que el resultat no �s null
        result.Value.Should().ContainKey(catalogue.Id).WhoseValue.Should().BeEquivalentTo(new ListCatalogueDetailDto(catalogue.Name)); // Comprovar que el cat�leg retornat �s correcte
        result.IsFailure().Should().BeFalse(); // Comprovar que l'operaci� ha tingut �xit
    }

    /// <summary>
    /// Prova de llistar cat�legs amb un nom que no coincideix i comprovar que es retorna un resultat buit
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task CatalogueService_WhenListingCataloguesWithNonMatchingName_ShouldReturnEmptyResult()
    {
         // Arrange
        var filterDto = new ListCataloguesFilterDto(Name: "NonExisting"); // Filtre per llistar cat�legs amb un nom que no existeix

        _catalogueRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CatalogueAggregate>()); // Configurar el mock per retornar una llista buida

        // Act
        var result = await _sut.ListCatalogues(filterDto); // Llistar els cat�legs

        // Assert
        _catalogueRepositoryMock
            .Verify(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<CancellationToken>()), Times.Once); // Comprovar que el m�tode ListAsync es crida una vegada
        result.Should().NotBeNull(); // Comprovar que el resultat no �s null
        result.Value.Should().BeEmpty(); // Comprovar que el resultat �s buit
        result.IsFailure().Should().BeFalse(); // Comprovar que l'operaci� ha tingut �xit
    }
}
