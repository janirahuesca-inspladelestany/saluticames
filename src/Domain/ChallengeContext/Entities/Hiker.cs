using SharedKernel.Abstractions;
using SharedKernel.Common;

namespace Domain.ChallengeContext.Entities;

public sealed class Hiker : AggregateRoot<string>
{
    private ICollection<Diary> _diaries = new List<Diary>();

    private Hiker(string id)
        : base(id)
    {
        
    }

    public string Name { get; private set; } = null!;
    public string Surname { get; private set; } = null!;
    public IEnumerable<Diary> Diaries => _diaries;

    public static Result<Hiker, Error> Create(string id, string name, string surname) 
    {
        return new Hiker(id)
        {
            Name = name,
            Surname = surname
        };
    }

    public EmptyResult<Error> AddDiary(Diary diary)
    {
        _diaries.Add(diary);
        return EmptyResult<Error>.Success();
    }
}
