using Application.Abstractions;
using Application.Content.Repositories;
using Application.Content.Services;
using Contracts.DTO.Content;
using Domain.Content.Entities;
using Domain.Content.Errors;
using FluentAssertions;
using Moq;
using SharedKernel.Common;
using SharedKernel.UnitTests.Helpers.Factories;
using System.Linq.Expressions;
using Error = SharedKernel.Common.Error;

namespace Application.UnitTests.Tests;

public class CatalogueServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ICatalogueRepository> _catalogueRepositoryMock;
    private readonly CatalogueService _sut;

    public CatalogueServiceTests()
    {
        _catalogueRepositoryMock = new Mock<ICatalogueRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.SetupGet(u => u.CatalogueRepository).Returns(_catalogueRepositoryMock.Object);
        _sut = new CatalogueService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task CatalogueService_WhenAddingNewSummitIds_ShouldReturnSuccess()
    {
        // Arrange
        var catalogueId = Guid.NewGuid();
        var summitIds = new List<Guid> { Guid.NewGuid() };
        var catalogue = CatalogueFactory.Create();

        _catalogueRepositoryMock
            .Setup(r => r.FindByIdAsync(catalogueId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(catalogue);

        // Act
        var result = await _sut.AddNewSummitIdsInCatalogueAsync(catalogueId, summitIds);

        // Assert
        _catalogueRepositoryMock.Verify(r => r.FindByIdAsync(catalogueId, It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.Should().BeOfType<EmptyResult<Error>>();
        result.IsFailure().Should().BeFalse();
    }

    [Fact]
    public async Task CatalogueService_WhenAddingExistingSummitId_ShouldReturnSummitIdAlreadyExistsError()
    {
        // Arrange
        var catalogueId = Guid.NewGuid();
        var existingSummitId = Guid.NewGuid();
        var summitIds = new List<Guid> { existingSummitId };
        var catalogue = CatalogueFactory.CreateWithSummitIds(existingSummitId);

        _catalogueRepositoryMock
            .Setup(r => r.FindByIdAsync(catalogueId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(catalogue);

        // Act
        var result = await _sut.AddNewSummitIdsInCatalogueAsync(catalogueId, summitIds);

        // Assert
        _catalogueRepositoryMock.Verify(r => r.FindByIdAsync(catalogueId, It.IsAny<CancellationToken>()), Times.Once);
        result.Should().NotBeNull();
        result.Should().BeOfType<EmptyResult<Error>>();
        result.IsFailure().Should().BeTrue();
        result.Error.Should().BeEquivalentTo(CatalogueErrors.SummitIdAlreadyExists);
    }

    [Fact]
    public async Task CatalogueService_WhenListingAllCatalogues_ShouldReturnAllCatalogues()
    {
        // Arrange
        var filterDto = new ListCataloguesFilterDto();
        var catalogueList = new List<CatalogueAggregate>
        {
            CatalogueFactory.Create(),
            CatalogueFactory.Create()
        };

        _catalogueRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(catalogueList);

        // Act
        var result = await _sut.ListCatalogues(filterDto);

        // Assert
        _catalogueRepositoryMock
            .Verify(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);

        result.Should().NotBeNull();
        result.IsFailure().Should().BeFalse();
        result.Value.Should().HaveCount(catalogueList.Count);
    }

    [Fact]
    public async Task CatalogueService_WhenListingCataloguesWithSpecificName_ShouldReturnFilteredCatalogues()
    {
        // Arrange
        var filterDto = new ListCataloguesFilterDto(Name: "El meu");
        var catalogue = CatalogueFactory.Create();

        _catalogueRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CatalogueAggregate> { catalogue });

        // Act
        var result = await _sut.ListCatalogues(filterDto);

        // Assert
        _catalogueRepositoryMock
            .Verify(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        
        result.Should().NotBeNull();
        result.Value.Should().ContainKey(catalogue.Id).WhoseValue.Should().BeEquivalentTo(new ListCatalogueDetailDto(catalogue.Name));
        result.IsFailure().Should().BeFalse();
    }

    [Fact]
    public async Task CatalogueService_WhenListingCataloguesWithNonMatchingName_ShouldReturnEmptyResult()
    {
        // Arrange
        var filterDto = new ListCataloguesFilterDto(Name: "NonExisting");

        _catalogueRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<CatalogueAggregate>());

        // Act
        var result = await _sut.ListCatalogues(filterDto);

        // Assert
        _catalogueRepositoryMock
            .Verify(r => r.ListAsync(It.IsAny<Expression<Func<CatalogueAggregate, bool>>>(), It.IsAny<Func<IQueryable<CatalogueAggregate>, IOrderedQueryable<CatalogueAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        
        result.Should().NotBeNull();
        result.Value.Should().BeEmpty();
        result.IsFailure().Should().BeFalse();
    }

}
