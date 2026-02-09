namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record TimeRange
{
    public TimeOnly Start { get; init; }
    public TimeOnly End { get; init; }

    protected TimeRange() { } // For EF

    public TimeRange(TimeOnly start, TimeOnly end)
    {
        if (end < start) throw new ArgumentException("End time cannot be before start time");
        Start = start;
        End = end;
    }

    public TimeSpan Duration => End.ToTimeSpan() - Start.ToTimeSpan();

    public int TotalHours => (int)Duration.TotalHours;
    public int TotalMinutes => (int)Duration.TotalMinutes;

    public bool OverlapsWith(TimeRange other) => Start < other.End && other.Start < End;

    public override string ToString() => $"{Start:HH:mm} - {End:HH:mm}";
}
