using Content.UnitTests.Helpers.Factories;
using Domain.Challenge.Entities;
using Domain.Challenge.Errors;
using Domain.Challenge.Rules;
using FluentAssertions;

namespace Challenge.UnitTests.Domain;

public class HikerAggregateTest
{
    [Fact]
    public void Create_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        string id = "12345678P";
        string name = "Kilian";
        string surname = "Gordet";

        // Act
        var result = Hiker.Create(id, name, surname);

        // Assert
        result.IsSuccess().Should().BeTrue();
        var hiker = result.Value!;
        hiker.Id.Should().Be(id);
        hiker.Name.Should().Be(name);
        hiker.Surname.Should().Be(surname);
    }

    [Fact]
    public void AddDiary_WhenDiaryIsValid_ThenSuccess()
    {
        // Arrange
        var diary = DiaryFactory.Create();
        var sut = HikerFactory.Create();

        // Act
        var addDiaryResult = sut.AddDiary(diary);

        // Assert
        addDiaryResult.IsSuccess().Should().BeTrue();
    }

    [Fact]
    public void AddDiary_WhenDiaryIsNull_ThenFailureDiaryNotFound()
    {
        // Arrange
        Diary diary = null;
        var sut = HikerFactory.Create();

        // Act
        var result = sut.AddDiary(diary);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(ChallengeErrors.DiaryNotFound);
    }

    [Fact]
    public void AddDiary_WhenDiaryAlreadyExists_ThenFailureDiaryAlreadyExists()
    {
        // Arrange
        var diary = DiaryFactory.Create();
        var sut = HikerFactory.Create();

        // Act
        var addDiaryResult1 = sut.AddDiary(diary);
        var addDiaryResult2 = sut.AddDiary(diary);

        // Assert
        addDiaryResult1.IsSuccess().Should().BeTrue();
        addDiaryResult2.IsFailure().Should().BeTrue();
        addDiaryResult2.Error.Should().Be(ChallengeErrors.DiaryAlreadyExists);
    }

    [Fact]
    public void AddClimbsToDiary_WhenDiaryIsNull_ThenFailureDiaryNotFound()
    {
        // Arrange
        Diary diary = null;
        var climbs = new List<Climb> { ClimbFactory.Create() };
        var sut = HikerFactory.Create();

        // Act
        var result = sut.AddClimbsToDiary(diary, climbs);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(ChallengeErrors.DiaryNotFound);
    }

    [Fact]
    public void AddClimbsToDiary_WhenClimbAlreadyExistsInDiary_ThenFailureClimbInvalidDuplicated()
    {
        // Arrange
        var climb = ClimbFactory.Create();
        var diary = DiaryFactory.CreateWithClimbs(climb);
        var newClimbs = new List<Climb> { climb };
        var sut = HikerFactory.CreateWithDiary(diary);

        // Act
        var result = sut.AddClimbsToDiary(diary, newClimbs);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidDuplicated);
        diary.Climbs.Should().ContainSingle();
    }

    [Fact]
    public void AddClimbsToDiary_WhenClimbsAreValidAndDoNotExistInDiary_ThenSuccess()
    {
        // Arrange
        var diary = DiaryFactory.Create();
        var originalClimbs = diary.Climbs;
        var climbs = new List<Climb> { ClimbFactory.Create(), ClimbFactory.Create() };
        var sut = HikerFactory.CreateWithDiary(diary);

        // Act
        var result = sut.AddClimbsToDiary(diary, climbs);

        // Assert
        result.IsSuccess().Should().BeTrue();
        diary.Climbs.Should().HaveCount(2);
        diary.Climbs.Should().BeEquivalentTo(originalClimbs);
    }

    [Fact]
    public void AddClimbToDiary_WhenDiaryIsNull_ThenFailureDiaryNotFound()
    {
        // Arrange
        Diary diary = null;
        var climb = ClimbFactory.Create();
        var sut = HikerFactory.Create();

        // Act
        var result = sut.AddClimbToDiary(diary, climb);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(ChallengeErrors.DiaryNotFound);
    }

    [Fact]
    public void AddClimbToDiary_WhenClimbAlreadyExistsInDiary_ThenFailureClimbInvalidDuplicated()
    {
        // Arrange
        var climb = ClimbFactory.Create();
        var diary = DiaryFactory.CreateWithClimbs(climb);
        var sut = HikerFactory.CreateWithDiary(diary);

        // Act
        var result = sut.AddClimbToDiary(diary, climb);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidDuplicated);
    }

    [Fact]
    public void AddClimbToDiary_WhenHikerReachedMaxClimbsPerDay_ThenFailureClimbInvalidReachedMaxLimits()
    {
        // Arrange
        var climbs = Enumerable
            .Range(0, Constants.MAX_CLIMBS_PER_DAY)
            .Select(_ => ClimbFactory.Create())
            .ToArray();
        var newClimb = ClimbFactory.Create();
        var diary = DiaryFactory.CreateWithClimbs(climbs);
        var sut = HikerFactory.CreateWithDiary(diary);

        // Act
        var result = sut.AddClimbToDiary(diary, newClimb);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidReachedMaxLimit);
    }

    [Fact]
    public void AddClimbToDiary_WhenClimbIsValidAndDoesNotExistInDiary_ThenAddsClimbSuccessfully()
    {
        // Arrange
        var hiker = HikerFactory.Create();
        var diary = DiaryFactory.Create();
        var climb = ClimbFactory.Create();

        // Act
        var result = hiker.AddClimbToDiary(diary, climb);

        // Assert
        result.IsSuccess().Should().BeTrue();
        diary.Climbs.Should().Contain(climb);
    }

    [Fact]
    public void ClimbCreate_WhenValidParameters_ThenSuccess()
    {
        // Arrange
        Guid summitId = Guid.NewGuid();
        DateTime ascensionDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var result = Climb.Create(summitId, ascensionDate);

        // Assert
        result.IsSuccess().Should().BeTrue();
        var climb = result.Value!;
        climb.SummitId.Should().Be(summitId);
        climb.AscensionDate.Should().Be(ascensionDate);
    }

    [Fact]
    public void ClimbSetSummitId_WhenSummitIdIsValid_ThenSuccess()
    {
        // Arrange
        var climb = ClimbFactory.Create();
        Guid newSummitId = Guid.NewGuid();

        // Act
        var result = climb.SetSummitId(newSummitId);

        // Assert
        result.IsSuccess().Should().BeTrue();
        climb.SummitId.Should().Be(newSummitId);
    }

    [Fact]
    public void ClimbSetSummitId_WhenSummitIdIsInvalid_ThenFailureClimbInvalidSummitBadReference()
    {
        // Arrange
        var climb = ClimbFactory.Create();
        Guid newSummitId = Guid.Empty;

        // Act
        var result = climb.SetSummitId(newSummitId);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidSummitBadReference);
    }

    [Fact]
    public void ClimbSetAscensionDate_WhenAscensionDateIsValid_ThenSuccess()
    {
        // Arrange
        var climb = ClimbFactory.Create();
        DateTime validDate = DateTime.UtcNow.AddDays(-1);

        // Act
        var result = climb.SetAscensionDate(validDate);

        // Assert
        result.IsSuccess().Should().BeTrue();
        climb.AscensionDate.Should().Be(validDate);
    }

    [Fact]
    public void ClimbSetAscensionDate_WhenDateIsInFuture_ThenFailureClimbInvalidAscensionDate()
    {
        // Arrange
        var climb = ClimbFactory.Create();
        DateTime futureDate = DateTime.UtcNow.AddDays(1);

        // Act
        var result = climb.SetAscensionDate(futureDate);

        // Assert
        result.IsFailure().Should().BeTrue();
        result.Error.Should().Be(ChallengeErrors.ClimbInvalidAscensionDate);
    }
}