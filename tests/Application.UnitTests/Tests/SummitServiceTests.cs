using Application.Abstractions;
using Application.Content.Repositories;
using Application.Content.Services;
using Contracts.DTO.Content;
using Domain.Content.Entities;
using Domain.Content.Errors;
using FluentAssertions;
using Moq;
using SharedKernel.UnitTests.Helpers.Factories;
using System.Linq.Expressions;

namespace Application.UnitTests.Tests;

public class SummitServiceTests
{
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ISummitRepository> _summitRepositoryMock;
    private readonly SummitService _sut;

    public SummitServiceTests()
    {
        _summitRepositoryMock = new Mock<ISummitRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _unitOfWorkMock.SetupGet(u => u.SummitRepository).Returns(_summitRepositoryMock.Object);
        _sut = new SummitService(_unitOfWorkMock.Object);
    }

    [Fact]
    public async Task SummitService_WhenAddingNewSummits_ShouldReturnSummitIds()
    {
        // Arrange
        var summitDtos = new List<AddNewSummitDto>
        {
            new AddNewSummitDto("El meu summit", 242, 42.0755F, 2.4558F, false, "Pla de l'Estany")
        };
        var summitsToAdd = new List<SummitAggregate>();

        _summitRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SummitAggregate>());

        _summitRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<SummitAggregate>(), It.IsAny<CancellationToken>()))
            .Callback<SummitAggregate, CancellationToken>((summit, cancellationToken) => summitsToAdd.Add(summit));

        // Act
        var result = await _sut.AddNewSummitsAsync(summitDtos);

        // Assert
        _summitRepositoryMock.Verify(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _summitRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SummitAggregate>(), It.IsAny<CancellationToken>()), Times.Exactly(summitDtos.Count));
        result.Should().NotBeNull();
        result.Value.Should().BeEquivalentTo(summitsToAdd.Select(s => s.Id));
        result.IsFailure().Should().BeFalse();
    }

    [Fact]
    public async Task SummitService_WhenAddingExistingSummit_ShouldReturnSummitAlreadyExistsError()
    {
        // Arrange
        var summitDtos = new List<AddNewSummitDto>
        {
            new AddNewSummitDto("El meu summit", 242, 42.0755F, 2.4558F, false, "Pla de l'Estany")
        };
        var existingSummit = SummitFactory.Create();

        _summitRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SummitAggregate> { existingSummit });

        // Act
        var result = await _sut.AddNewSummitsAsync(summitDtos);

        // Assert
        _summitRepositoryMock.Verify(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _summitRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SummitAggregate>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Should().NotBeNull();
        result.Error.Should().Be(SummitErrors.SummitAlreadyExists);
        result.IsFailure().Should().BeTrue();
    }

    [Fact]
    public async Task SummitService_WhenAddingSummitWithInvalidRegion_ShouldReturnSummitRegionNotAvailableError()
    {
        // Arrange
        var summitDtos = new List<AddNewSummitDto>
        {
            new AddNewSummitDto("El meu summit", 242, 42.0755F, 2.4558F, false, "InvalidRegion")
        };

        _summitRepositoryMock
            .Setup(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SummitAggregate>());

        // Act
        var result = await _sut.AddNewSummitsAsync(summitDtos);

        // Assert
        _summitRepositoryMock.Verify(r => r.ListAsync(It.IsAny<Expression<Func<SummitAggregate, bool>>>(), It.IsAny<Func<IQueryable<SummitAggregate>, IOrderedQueryable<SummitAggregate>>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        _summitRepositoryMock.Verify(r => r.AddAsync(It.IsAny<SummitAggregate>(), It.IsAny<CancellationToken>()), Times.Never);
        result.Should().NotBeNull();
        result.Error.Should().Be(SummitErrors.SummitRegionNotAvailable);
        result.IsFailure().Should().BeTrue();
    }
}
