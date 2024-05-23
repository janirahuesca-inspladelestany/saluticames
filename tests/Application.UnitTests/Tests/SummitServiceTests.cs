using Application.Abstractions;
using Application.Content.Repositories;
using Application.Content.Services;
using Common.Helpers.Factories;
using Contracts.DTO.Content;
using Domain.Content.Entities;
using Domain.Content.Errors;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace Application.UnitTests.Tests;

public class SummitServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ISummitRepository> _summitRepositoryMock;
    private readonly SummitService _sut;

    public SummitServiceTests()
    {
        // Configuració dels mocks i del servei SummitService per a cada test
        _summitRepositoryMock = new Mock<ISummitRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.SetupGet(u => u.SummitRepository).Returns(_summitRepositoryMock.Object);
        _sut = new SummitService(_unitOfWorkMock.Object);
    }

    /// <summary>
    /// Prova d'afegir nous cims i comprovar que es retornen els identificadors dels cims afegits
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SummitService_WhenAddingNewSummits_ShouldReturnSummitIds()
    {
        // Arrange
        var summitDtos = new List<AddNewSummitDto>
        {
            new AddNewSummitDto("El meu summit", 242, 42.0755F, 2.4558F, false, "Pla de l'Estany")
        };
        var summitsToAdd = new List<SummitAggregate>();

        // Configuració del comportament del mock del repositori per a retornar una llista buida de cims
        _summitRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SummitAggregate>());

        // Configuració del comportament del mock del repositori per a afegir cims i registrar-los
        _summitRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SummitAggregate>(), It.IsAny<CancellationToken>()))
            .Callback<SummitAggregate, CancellationToken>((summit, cancellationToken) => summitsToAdd.Add(summit));

        // Act
        var result = await _sut.AddNewSummitsAsync(summitDtos); // S'afegeixen els nous cims

        // Assert

        // Verificació que es criden els mètodes esperats i comprovació del resultat obtingut
        _summitRepositoryMock.Verify(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _summitRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SummitAggregate>(), It.IsAny<CancellationToken>()), Times.Exactly(summitDtos.Count));
        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(summitsToAdd.Select(s => s.Id));
        result.IsFailure().Should().BeFalse();
    }

    /// <summary>
    /// Prova d'afegir un cim que ja existeix i comprova que es retorna l'error SummitAlreadyExists
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SummitService_WhenAddingExistingSummit_ShouldReturnSummitAlreadyExistsError()
    {
        // Arrange
        var summitDtos = new List<AddNewSummitDto>
        {
            new AddNewSummitDto("El meu summit", 242, 42.0755F, 2.4558F, false, "Pla de l'Estany")
        };
        var existingSummit = SummitFactory.Create();

        // Configuració del comportament del mock del repositori per a retornar una llista amb un cim existent
        _summitRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SummitAggregate> { existingSummit });

        // Act
        var result = await _sut.AddNewSummitsAsync(summitDtos); // S'intenta afegir un cim que ja existeix

        // Assert

        // Verificació que es criden els mètodes esperats i comprovació del resultat obtingut
        _summitRepositoryMock.Verify(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _summitRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SummitAggregate>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Should().NotBeNull();
        result.Error.Should().Be(SummitErrors.SummitAlreadyExists);
        result.IsFailure().Should().BeTrue();
    }

    /// <summary>
    /// Prova d'afegir un cim amb una regió no vàlida i comprova que es retorna l'error SummitRegionNotAvailable
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task SummitService_WhenAddingSummitWithInvalidRegion_ShouldReturnSummitRegionNotAvailableError()
    {
        // Arrange
        var summitDtos = new List<AddNewSummitDto>
        {
            new AddNewSummitDto("El meu summit", 242, 42.0755F, 2.4558F, false, "InvalidRegion")
        };

        // Configuració del comportament del mock del repositori per a retornar una llista buida de cims
        _summitRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SummitAggregate>());

        // Act
        var result = await _sut.AddNewSummitsAsync(summitDtos); // S'intenta afegir un cim amb una regió no vàlida

        // Verificació que es criden els mètodes esperats i comprovació del resultat obtingut
        // Assert
        _summitRepositoryMock.Verify(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<CancellationToken>()), Times.Once);
        _summitRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SummitAggregate>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Should().NotBeNull();
        result.Error.Should().Be(SummitErrors.SummitRegionNotAvailable);
        result.IsFailure().Should().BeTrue();
    }
}
