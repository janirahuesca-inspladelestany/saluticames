using Common.Helpers.Factories;
using Domain.Challenge.Entities;
using Domain.Challenge.Errors;
using Domain.Challenge.Rules;
using FluentAssertions;

namespace Domain.UnitTests.Tests;

public class HikerAggregateTests
{
    /// <summary>
    /// Prova de la creaci� d'un excursionista amb par�metres v�lids
    /// </summary>
    [Fact]
    public void Create_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        string id = "12345678P"; // Identificador v�lid
        string name = "Kilian"; // Nom v�lid
        string surname = "Gordet"; // Cognom v�lid

        // Act
        var result = HikerAggregate.Create(id, name, surname); // Crear l'excursionista

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que la creaci� ha tingut �xit
        var hiker = result.Value!;
        hiker.Id.Should().Be(id);
        hiker.Name.Should().Be(name);
        hiker.Surname.Should().Be(surname);
    }

    /// <summary>
    /// Prova d'afegir un diari v�lid a un excursionista
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
        addDiaryResult.IsSuccess().Should().BeTrue(); // Comprovar que l'addici� ha tingut �xit
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
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
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
        addDiaryResult1.IsSuccess().Should().BeTrue(); // Comprovar que la primera addici� ha tingut �xit
        addDiaryResult2.IsFailure().Should().BeTrue(); // Comprovar que la segona addici� ha fallat
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
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(ChallengeErrors.DiaryNotFound); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'afegir una ascensi� que ja existeix en un diari
    /// </summary>
    [Fact]
    public void AddClimbsToDiary_WhenClimbAlreadyExistsInDiary_ThenFailureClimbInvalidDuplicated()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una ascensi� de prova
        var diary = DiaryFactory.CreateWithClimbs(climb); // Crear un diari amb l'ascensi� de prova
        var newClimbs = new List<ClimbEntity> { climb }; // Crear una llista amb l'ascensi� duplicada
        var sut = HikerFactory.CreateWithDiary(diary); // Crear l'excursionista de prova amb el diari

        // Act
        var result = sut.AddClimbsToDiary(diary, newClimbs); // Intentar afegir l'ascensi� duplicada

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidDuplicated); // Comprovar l'error esperat
        diary.Climbs.Should().ContainSingle(); // Comprovar que el diari nom�s cont� una ascensi�
    }

    /// <summary>
    /// Prova d'afegir ascensions v�lides que no existeixen en un diari
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
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operaci� ha tingut �xit
        diary.Climbs.Should().HaveCount(2); // Comprovar que el diari cont� dues ascensions
        diary.Climbs.Should().BeEquivalentTo(originalClimbs); // Comprovar que les noves ascensions s�n correctes
    }

    /// <summary>
    /// Prova d'afegir una ascensi� a un diari nul
    /// </summary>
    [Fact]
    public void AddClimbToDiary_WhenDiaryIsNull_ThenFailureDiaryNotFound()
    {
        // Arrange
        DiaryEntity diary = null; // Diari nul
        var climb = ClimbFactory.Create(); // Crear una ascensi� de prova
        var sut = HikerFactory.Create(); // Crear l'excursionista de prova

        // Act
        var result = sut.AddClimbToDiary(diary, climb); // Intentar afegir l'ascensi�

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(ChallengeErrors.DiaryNotFound); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'afegir una ascensi� que ja existeix en un diari
    /// </summary>
    [Fact]
    public void AddClimbToDiary_WhenClimbAlreadyExistsInDiary_ThenFailureClimbInvalidDuplicated()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una ascensi� de prova
        var diary = DiaryFactory.CreateWithClimbs(climb); // Crear un diari amb l'ascensi� de prova
        var sut = HikerFactory.CreateWithDiary(diary); // Crear l'excursionista de prova amb el diari

        // Act
        var result = sut.AddClimbToDiary(diary, climb); // Intentar afegir l'ascensi� duplicada

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidDuplicated); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'afegir una ascensi� quan l'excursionista ha arribat al m�xim d'ascensions per dia
    /// </summary>
    [Fact]
    public void AddClimbToDiary_WhenHikerReachedMaxClimbsPerDay_ThenFailureClimbInvalidReachedMaxLimits()
    {
        // Arrange
        var climbs = Enumerable
            .Range(0, Constants.MAX_CLIMBS_PER_DAY) // Generar el nombre m�xim d'ascensions
            .Select(_ => ClimbFactory.Create())
            .ToArray();
        var newClimb = ClimbFactory.Create(); // Crear una nova ascensi� de prova
        var diary = DiaryFactory.CreateWithClimbs(climbs); // Crear un diari amb el m�xim d'ascensions
        var sut = HikerFactory.CreateWithDiary(diary); // Crear l'excursionista de prova amb el diari

        // Act
        var result = sut.AddClimbToDiary(diary, newClimb); // Intentar afegir una ascensi� m�s

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidReachedMaxLimit); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'afegir una ascensi� v�lida que no existeix en un diari
    /// </summary>
    [Fact]
    public void AddClimbToDiary_WhenClimbIsValidAndDoesNotExistInDiary_ThenAddsClimbSuccessfully()
    {
        // Arrange
        var hiker = HikerFactory.Create(); // Crear l'excursionista de prova
        var diary = DiaryFactory.Create(); // Crear un diari de prova
        var climb = ClimbFactory.Create(); // Crear una ascensi� de prova

        // Act
        var result = hiker.AddClimbToDiary(diary, climb); // Afegir l'ascensi� al diari

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operaci� ha tingut �xit
        diary.Climbs.Should().Contain(climb); // Comprovar que l'ascensi� s'ha afegit correctament
    }

    /// <summary>
    /// Prova de la creaci� d'una ascensi� amb par�metres v�lids
    /// </summary>
    [Fact]
    public void ClimbCreate_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        Guid summitId = Guid.NewGuid(); // Identificador de cim v�lid
        DateTime ascensionDate = DateTime.UtcNow.AddDays(-1); // Data d'ascensi� v�lida

        // Act
        var result = ClimbEntity.Create(summitId, ascensionDate); // Crear l'ascensi�

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que la creaci� ha tingut �xit
        var climb = result.Value!;
        climb.SummitId.Should().Be(summitId);
        climb.AscensionDate.Should().Be(ascensionDate);
    }

    /// <summary>
    /// Prova d'establir un identificador de cim v�lid en una ascensi�
    /// </summary>
    [Fact]
    public void ClimbSetSummitId_WhenSummitIdIsValid_ThenSuccess()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una ascensi� de prova
        Guid newSummitId = Guid.NewGuid(); // Nou identificador de cim v�lid

        // Act
        var result = climb.SetSummitId(newSummitId); // Establir el nou identificador

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operaci� ha tingut �xit
        climb.SummitId.Should().Be(newSummitId);
    }

    /// <summary>
    /// Prova d'establir un identificador de cim no v�lid en una ascensi�
    /// </summary>
    [Fact]
    public void ClimbSetSummitId_WhenSummitIdIsInvalid_ThenFailureClimbInvalidSummitBadReference()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una ascensi� de prova
        Guid newSummitId = Guid.Empty; // Nou identificador de cim no v�lid

        // Act
        var result = climb.SetSummitId(newSummitId); // Intentar establir el nou identificador

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidSummitBadReference); // Comprovar l'error esperat
    }

    /// <summary>
    /// Prova d'establir una data d'ascensi� v�lida en una ascensi�
    /// </summary>
    [Fact]
    public void ClimbSetAscensionDate_WhenAscensionDateIsValid_ThenSuccess()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una ascensi� de prova
        DateTime validDate = DateTime.UtcNow.AddDays(-1); // Nova data d'ascensi� v�lida

        // Act
        var result = climb.SetAscensionDate(validDate); // Establir la nova data

        // Assert
        result.IsSuccess().Should().BeTrue(); // Comprovar que l'operaci� ha tingut �xit
        climb.AscensionDate.Should().Be(validDate);
    }

    /// <summary>
    /// Prova d'establir una data d'ascensi� futura en una ascensi�
    /// </summary>
    [Fact]
    public void ClimbSetAscensionDate_WhenDateIsInFuture_ThenFailureClimbInvalidAscensionDate()
    {
        // Arrange
        var climb = ClimbFactory.Create(); // Crear una escalada de prova
        DateTime futureDate = DateTime.UtcNow.AddDays(1); // Nova data d'ascensi� futura

        // Act
        var result = climb.SetAscensionDate(futureDate); // Intentar establir la nova data

        // Assert
        result.IsFailure().Should().BeTrue(); // Comprovar que l'operaci� ha fallat
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidAscensionDate); // Comprovar l'error esperat
    }
}