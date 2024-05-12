using Content.UnitTests.Helpers.Factories;
using Domain.Content.Entities;
using Domain.Content.Enums;
using Domain.Content.Errors;
using FluentAssertions;

namespace Content.UnitTests.Domain;

public class SummitAggregateTest
{
    [Fact]
    public void Create_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        string name = "Montjuic";
        int altitude = 173;
        float latitude = 41.3642f;
        float longitude = 2.1533f;
        bool isEssential = true;
        Region region = Region.Barcelones;
        Guid id = Guid.NewGuid();

        // Act
        var result = Summit.Create(name, altitude, latitude, longitude, isEssential, region, id);

        // Assert
        result.IsSuccess().Should().BeTrue();
        var summit = result.Value!;
        summit.Id.Should().Be(id);
        summit.Name.Should().Be(name);
        summit.Altitude.Should().Be(altitude);
        summit.Latitude.Should().Be(latitude);
        summit.Longitude.Should().Be(longitude);
        summit.IsEssential.Should().Be(isEssential);
        summit.Region.Should().Be(region);
    }

    [Fact]
    public void SetName_WhenNameIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create();
        string newName = "New Name";

        // Act
        var result = summit.SetName(newName);

        // Assert
        result.IsSuccess().Should().BeTrue();
        summit.Name.Should().Be(newName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(3151)]
    public void SetAltitude_WhenAltitudeIsInvalid_ThenFailureSummitInvalidAltitude(int invalidAltitude)
    {
        // Arrange
        var summit = SummitFactory.Create();

        // Act
        var result = summit.SetAltitude(invalidAltitude);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(SummitErrors.SummitInvalidAltitude);
    }

    [Fact]
    public void SetAltitude_WhenAltitudeIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create();
        var newAltitude = 242;

        // Act
        var result = summit.SetAltitude(newAltitude);

        // Assert
        result.IsSuccess().Should().BeTrue();
        summit.Altitude.Should().Be(newAltitude);
    }

    [Fact]
    public void SetLatitude_WhenLatitudeIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create();
        float newLatitude = 41.3642f;

        // Act
        var result = summit.SetLatitude(newLatitude);

        // Assert
        result.IsSuccess().Should().BeTrue();
        summit.Latitude.Should().Be(newLatitude);
    }

    [Fact]
    public void SetLongitude_WhenLongitudeIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create();
        float newLongitude = 2.1533f;

        // Act
        var result = summit.SetLongitude(newLongitude);

        // Assert
        result.IsSuccess().Should().BeTrue();
        summit.Longitude.Should().Be(newLongitude);
    }

    [Fact]
    public void SetIsEssential_WhenIsEssentialIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create();
        bool newIsEssential = true;

        // Act
        var result = summit.SetIsEssential(newIsEssential);

        // Assert
        result.IsSuccess().Should().BeTrue();
        summit.IsEssential.Should().Be(newIsEssential);
    }

    [Fact]
    public void SetRegion_WhenRegionIsNone_ThenFailureSummitRegionNotAvailable()
    {
        // Arrange
        var summit = SummitFactory.Create();

        // Act
        var result = summit.SetRegion(Region.None);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(SummitErrors.SummitRegionNotAvailable);
    }

    [Fact]
    public void SetRegion_WhenRegionIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create();
        var newRegion = Region.Garrotxa;

        // Act
        var result = summit.SetRegion(newRegion);

        // Assert
        result.IsSuccess().Should().BeTrue();
        summit.Region.Should().Be(newRegion);
    }

    [Theory]
    [InlineData(1000, DifficultyLevel.Easy)]
    [InlineData(1500, DifficultyLevel.Moderate)]
    [InlineData(2500, DifficultyLevel.Difficult)]
    public void SetAltitude_WhenAltitudeIsValid_ThenSuccessAndDifficultyLevelCalculatedCorrectly(int altitude, DifficultyLevel expectedDifficulty)
    {
        // Arrange
        var summit = SummitFactory.Create();

        // Act
        var result = summit.SetAltitude(altitude);

        // Assert
        result.IsSuccess().Should().BeTrue();
        summit.DifficultyLevel.Should().Be(expectedDifficulty);
    }
}
