namespace Domain.Catalogues.Enums;

//public sealed record DifficultyLevel(int Id, string Level)
//{
//    public static readonly DifficultyLevel EASY = new DifficultyLevel(0, "EASY");
//    public static readonly DifficultyLevel MODERATE = new DifficultyLevel(1, "MODERATE");
//    public static readonly DifficultyLevel DIFFICULT = new DifficultyLevel(2, "DIFFICULT");
//}

public enum DifficultyLevel 
{
    NONE = -1,
    EASY = 1,
    MODERATE,
    DIFFICULT
}