using Common.Helpers.Factories;
using Domain.Content.Entities;
using Domain.Content.Enums;
using Domain.Content.Errors;
using FluentAssertions;

namespace Domain.UnitTests.Tests;

public class SummitAggregateTests
{
    /// <summary>
    /// Prova la creació d'un cim amb paràmetres vàlids
    /// </summary>
    [Fact]
    public void Create_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        string name = "Montjuic"; // Nom del cim
        int altitude = 173; // Altitud del cim
        float latitude = 41.3642f; // Latitud del cim
        float longitude = 2.1533f; // Longitud del cim
        bool isEssential = true; // Indicador de si el cim és essencial
        Region region = Region.Barcelones; // Regió del cim
        Guid id = Guid.NewGuid(); // Identificador únic del cim

        // Act
        var result = SummitAggregate.Create(name, altitude, latitude, longitude, isEssential, region, id); // Crear el cim

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que la creació ha tingut èxit
        var summit = result.Value!;
        summit.Id.Should().Be(id);
        summit.Name.Should().Be(name);
        summit.Altitude.Should().Be(altitude);
        summit.Latitude.Should().Be(latitude);
        summit.Longitude.Should().Be(longitude);
        summit.IsEssential.Should().Be(isEssential);
        summit.Region.Should().Be(region);
    }

    /// <summary>
    /// Prova d'establir un nou nom vàlid per al cim
    /// </summary>
    [Fact]
    public void SetName_WhenNameIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create(); // Crear un cim de prova
        string newName = "New Name"; // Nou nom del cim

        // Act
        var result = summit.SetName(newName); // Establir el nou nom

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        summit.Name.Should().Be(newName);
    }

    /// <summary>
    /// Prova d'establir una altitud no vàlida per al cim
    /// </summary>
    /// <param name="invalidAltitude"></param>
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(3151)]
    public void SetAltitude_WhenAltitudeIsInvalid_ThenFailureSummitInvalidAltitude(int invalidAltitude)
    {
        // Arrange
        var summit = SummitFactory.Create(); // Crear un cim de prova

        // Act
        var result = summit.SetAltitude(invalidAltitude); // Intentar establir l'altitud no vàlida

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(SummitErrors.SummitInvalidAltitude); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'establir una altitud vàlida per al cim
    /// </summary>
    [Fact]
    public void SetAltitude_WhenAltitudeIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create(); // Crear un cim de prova
        var newAltitude = 242; // Nova altitud

        // Act
        var result = summit.SetAltitude(newAltitude); // Establir la nova altitud

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        summit.Altitude.Should().Be(newAltitude);
    }

    /// <summary>
    /// Prova d'establir una latitud vàlida per al cim
    /// </summary>
    [Fact]
    public void SetLatitude_WhenLatitudeIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create(); // Crear un cim de prova
        float newLatitude = 41.3642f; // Nova latitud

        // Act
        var result = summit.SetLatitude(newLatitude); // Establir la nova latitud

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        summit.Latitude.Should().Be(newLatitude);
    }

    /// <summary>
    /// Prova d'establir una longitud vàlida per al cim
    /// </summary>
    [Fact]
    public void SetLongitude_WhenLongitudeIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create(); // Crear un cim de prova
        float newLongitude = 2.1533f; // Nova longitud

        // Act
        var result = summit.SetLongitude(newLongitude); // Establir la nova longitud

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        summit.Longitude.Should().Be(newLongitude);
    }

    /// <summary>
    /// Prova d'establir si el cim és essencial
    /// </summary>
    [Fact]
    public void SetIsEssential_WhenIsEssentialIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create(); // Crear un cim de prova
        bool newIsEssential = true; // Nou valor per a si el cim és essencial

        // Act
        var result = summit.SetIsEssential(newIsEssential); // Establir el nou valor

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        summit.IsEssential.Should().Be(newIsEssential);
    }

    /// <summary>
    /// Prova d'establir una regió no vàlida (None) per al cim
    /// </summary>
    [Fact]
    public void SetRegion_WhenRegionIsNone_ThenFailureSummitRegionNotAvailable()
    {
        // Arrange
        var summit = SummitFactory.Create(); // Crear un cim de prova

        // Act
        var result = summit.SetRegion(Region.None); // Intentar establir la regió com None

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(SummitErrors.SummitRegionNotAvailable); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'establir una regió vàlida per al cim
    /// </summary>
    [Fact]
    public void SetRegion_WhenRegionIsValid_ThenSuccess()
    {
        // Arrange
        var summit = SummitFactory.Create(); // Crear un cim de prova
        var newRegion = Region.Garrotxa; // Nova regió

        // Act
        var result = summit.SetRegion(newRegion); // Establir la nova regió

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        summit.Region.Should().Be(newRegion);
    }

    /// <summary>
    /// Prova d'establir una altitud vàlida i calcular correctament el nivell de dificultat
    /// </summary>
    /// <param name="altitude"></param>
    /// <param name="expectedDifficulty"></param>
    [Theory]
    [InlineData(1000, DifficultyLevel.Easy)]
    [InlineData(1500, DifficultyLevel.Moderate)]
    [InlineData(2500, DifficultyLevel.Difficult)]
    public void SetAltitude_WhenAltitudeIsValid_ThenSuccessAndDifficultyLevelCalculatedCorrectly(int altitude, DifficultyLevel expectedDifficulty)
    {
        // Arrange
        var summit = SummitFactory.Create(); // Crear un cim de prova

        // Act
        var result = summit.SetAltitude(altitude); // Establir la nova altitud

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        summit.DifficultyLevel.Should().Be(expectedDifficulty); // Comprovar que el nivell de dificultat és correcte
    }
}
