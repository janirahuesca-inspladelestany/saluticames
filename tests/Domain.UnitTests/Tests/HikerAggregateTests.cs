using Common.Helpers.Factories;
using Domain.Challenge.Entities;
using Domain.Challenge.Errors;
using Domain.Challenge.Rules;
using FluentAssertions;

namespace Domain.UnitTests.Tests;

public class HikerAggregateTests
{
    /// <summary>
    /// Prova de la creació d'un excursionista amb paràmetres vàlids
    /// </summary>
    [Fact]
    public void Create_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        string id = "12345678P"; // Identificador vàlid
        string name = "Kilian"; // Nom vàlid
        string surname = "Gordet"; // Cognom vàlid

        // Act
        var result = HikerAggregate.Create(id, name, surname); // Crear l'excursionista

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que la creació ha tingut èxit
        var hiker = result.Value!;
        hiker.Id.Should().Be(id);
        hiker.Name.Should().Be(name);
        hiker.Surname.Should().Be(surname);
    }

    /// <summary>
    /// Prova d'afegir un diari vàlid a un excursionista
    /// </summary>
    [Fact]
    public void AddDiary_WhenDiaryIsValid_ThenSuccess()
    {
        // Arrange
        var diary = DiaryFactory.Create(); // Crear un diari de prova
        var sut = HikerFactory.Create(); // Crear l'excursionista de prova

        // Act
        var addDiaryResult = sut.AddDiary(diary); // Afegir el diari

        // Assert
        addDiaryResult.IsSuccess().Should().BeTrue(); // Comprovar que l'addició ha tingut èxit
    }

    /// <summary>
    /// Prova d'afegir un diari nul a un excursionista
    /// </summary>
    [Fact]
    public void AddDiary_WhenDiaryIsNull_ThenFailureDiaryNotFound()
    {
        // Arrange
        DiaryEntity diary = null; // Diari nul
        var sut = HikerFactory.Create(); // Crear l'excursionista de prova

        // Act
        var result = sut.AddDiary(diary); // Intentar afegir el diari nul

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(ChallengeErrors.DiaryNotFound); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'afegir un diari que ja existeix a un excursionista
    /// </summary>
    [Fact]
    public void AddDiary_WhenDiaryAlreadyExists_ThenFailureDiaryAlreadyExists()
    {
        // Arrange
        var diary = DiaryFactory.Create(); // Crear un diari de prova
        var sut = HikerFactory.Create(); // Crear l'excursionista de prova

        // Act
        var addDiaryResult1 = sut.AddDiary(diary); // Afegir el diari la primera vegada
        var addDiaryResult2 = sut.AddDiary(diary); // Intentar afegir el diari una segona vegada

        // Assert
        addDiaryResult1.IsSuccess().Should().BeTrue(); // Comprovar que la primera addició ha tingut èxit
        addDiaryResult2.IsFailure().Should().BeTrue(); // Comprovar que la segona addició ha fallat
        addDiaryResult2.Error.Should().Be(ChallengeErrors.DiaryAlreadyExists); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'afegir ascensions a un diari nul
    /// </summary>
    [Fact]
    public void AddClimbsToDiary_WhenDiaryIsNull_ThenFailureDiaryNotFound()
    {
        // Arrange
        DiaryEntity diary = null; // Diari nul
        var climbs = new List<ClimbEntity> { ClimbFactory.Create() }; // Crear una llista d'ascensions de prova
        var sut = HikerFactory.Create(); // Crear l'excursionista de prova

        // Act
        var result = sut.AddClimbsToDiary(diary, climbs); // Intentar afegir les ascensions

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(ChallengeErrors.DiaryNotFound); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'afegir una ascensió que ja existeix en un diari
    /// </summary>
    [Fact]
    public void AddClimbsToDiary_WhenClimbAlreadyExistsInDiary_ThenFailureClimbInvalidDuplicated()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una ascensió de prova
        var diary = DiaryFactory.CreateWithClimbs(climb); // Crear un diari amb l'ascensió de prova
        var newClimbs = new List<ClimbEntity> { climb }; // Crear una llista amb l'ascensió duplicada
        var sut = HikerFactory.CreateWithDiary(diary); // Crear l'excursionista de prova amb el diari

        // Act
        var result = sut.AddClimbsToDiary(diary, newClimbs); // Intentar afegir l'ascensió duplicada

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidDuplicated); // Comprovar l'error esperat
        diary.Climbs.Should().ContainSingle(); // Comprovar que el diari només conté una ascensió
    }

    /// <summary>
    /// Prova d'afegir ascensions vàlides que no existeixen en un diari
    /// </summary>
    [Fact]
    public void AddClimbsToDiary_WhenClimbsAreValidAndDoNotExistInDiary_ThenSuccess()
    {
        // Arrange
        var diary = DiaryFactory.Create(); // Crear un diari de prova
        var originalClimbs = diary.Climbs; // Obtenir les ascensions originals del diari
        var climbs = new List<ClimbEntity> { ClimbFactory.Create(), ClimbFactory.Create() }; // Crear una llista de noves ascensions
        var sut = HikerFactory.CreateWithDiary(diary); // Crear l'excursionista de prova amb el diari

        // Act
        var result = sut.AddClimbsToDiary(diary, climbs); // Afegir les noves ascensions

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        diary.Climbs.Should().HaveCount(2); // Comprovar que el diari conté dues ascensions
        diary.Climbs.Should().BeEquivalentTo(originalClimbs); // Comprovar que les noves ascensions són correctes
    }

    /// <summary>
    /// Prova d'afegir una ascensió a un diari nul
    /// </summary>
    [Fact]
    public void AddClimbToDiary_WhenDiaryIsNull_ThenFailureDiaryNotFound()
    {
        // Arrange
        DiaryEntity diary = null; // Diari nul
        var climb = ClimbFactory.Create(); // Crear una ascensió de prova
        var sut = HikerFactory.Create(); // Crear l'excursionista de prova

        // Act
        var result = sut.AddClimbToDiary(diary, climb); // Intentar afegir l'ascensió

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(ChallengeErrors.DiaryNotFound); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'afegir una ascensió que ja existeix en un diari
    /// </summary>
    [Fact]
    public void AddClimbToDiary_WhenClimbAlreadyExistsInDiary_ThenFailureClimbInvalidDuplicated()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una ascensió de prova
        var diary = DiaryFactory.CreateWithClimbs(climb); // Crear un diari amb l'ascensió de prova
        var sut = HikerFactory.CreateWithDiary(diary); // Crear l'excursionista de prova amb el diari

        // Act
        var result = sut.AddClimbToDiary(diary, climb); // Intentar afegir l'ascensió duplicada

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidDuplicated); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'afegir una ascensió quan l'excursionista ha arribat al màxim d'ascensions per dia
    /// </summary>
    [Fact]
    public void AddClimbToDiary_WhenHikerReachedMaxClimbsPerDay_ThenFailureClimbInvalidReachedMaxLimits()
    {
        // Arrange
        var climbs = Enumerable
            .Range(0, Constants.MAX_CLIMBS_PER_DAY) // Generar el nombre màxim d'ascensions
            .Select(_ => ClimbFactory.Create())
            .ToArray();
        var newClimb = ClimbFactory.Create(); // Crear una nova ascensió de prova
        var diary = DiaryFactory.CreateWithClimbs(climbs); // Crear un diari amb el màxim d'ascensions
        var sut = HikerFactory.CreateWithDiary(diary); // Crear l'excursionista de prova amb el diari

        // Act
        var result = sut.AddClimbToDiary(diary, newClimb); // Intentar afegir una ascensió més

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidReachedMaxLimit); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'afegir una ascensió vàlida que no existeix en un diari
    /// </summary>
    [Fact]
    public void AddClimbToDiary_WhenClimbIsValidAndDoesNotExistInDiary_ThenAddsClimbSuccessfully()
    {
        // Arrange
        var hiker = HikerFactory.Create(); // Crear l'excursionista de prova
        var diary = DiaryFactory.Create(); // Crear un diari de prova
        var climb = ClimbFactory.Create(); // Crear una ascensió de prova

        // Act
        var result = hiker.AddClimbToDiary(diary, climb); // Afegir l'ascensió al diari

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        diary.Climbs.Should().Contain(climb); // Comprovar que l'ascensió s'ha afegit correctament
    }

    /// <summary>
    /// Prova de la creació d'una ascensió amb paràmetres vàlids
    /// </summary>
    [Fact]
    public void ClimbCreate_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        Guid summitId = Guid.NewGuid(); // Identificador de cim vàlid
        DateTime ascensionDate = DateTime.UtcNow.AddDays(-1); // Data d'ascensió vàlida

        // Act
        var result = ClimbEntity.Create(summitId, ascensionDate); // Crear l'ascensió

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que la creació ha tingut èxit
        var climb = result.Value!;
        climb.SummitId.Should().Be(summitId);
        climb.AscensionDate.Should().Be(ascensionDate);
    }

    /// <summary>
    /// Prova d'establir un identificador de cim vàlid en una ascensió
    /// </summary>
    [Fact]
    public void ClimbSetSummitId_WhenSummitIdIsValid_ThenSuccess()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una ascensió de prova
        Guid newSummitId = Guid.NewGuid(); // Nou identificador de cim vàlid

        // Act
        var result = climb.SetSummitId(newSummitId); // Establir el nou identificador

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        climb.SummitId.Should().Be(newSummitId);
    }

    /// <summary>
    /// Prova d'establir un identificador de cim no vàlid en una ascensió
    /// </summary>
    [Fact]
    public void ClimbSetSummitId_WhenSummitIdIsInvalid_ThenFailureClimbInvalidSummitBadReference()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una ascensió de prova
        Guid newSummitId = Guid.Empty; // Nou identificador de cim no vàlid

        // Act
        var result = climb.SetSummitId(newSummitId); // Intentar establir el nou identificador

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidSummitBadReference); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'establir una data d'ascensió vàlida en una ascensió
    /// </summary>
    [Fact]
    public void ClimbSetAscensionDate_WhenAscensionDateIsValid_ThenSuccess()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una ascensió de prova
        DateTime validDate = DateTime.UtcNow.AddDays(-1); // Nova data d'ascensió vàlida

        // Act
        var result = climb.SetAscensionDate(validDate); // Establir la nova data

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operació ha tingut èxit
        climb.AscensionDate.Should().Be(validDate);
    }

    /// <summary>
    /// Prova d'establir una data d'ascensió futura en una ascensió
    /// </summary>
    [Fact]
    public void ClimbSetAscensionDate_WhenDateIsInFuture_ThenFailureClimbInvalidAscensionDate()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una escalada de prova
        DateTime futureDate = DateTime.UtcNow.AddDays(1); // Nova data d'ascensió futura

        // Act
        var result = climb.SetAscensionDate(futureDate); // Intentar establir la nova data

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operació ha fallat
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidAscensionDate); // Comprovar l'error esperat
    }
}