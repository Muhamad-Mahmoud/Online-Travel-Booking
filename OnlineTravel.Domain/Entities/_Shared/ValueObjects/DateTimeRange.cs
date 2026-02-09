namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record DateTimeRange
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }

    protected DateTimeRange() { } // For EF

    public DateTimeRange(DateTime start, DateTime end)
    {
        if (end < start) throw new ArgumentException("End date-time cannot be before start date-time");
        Start = start;
        End = end;
    }

    public TimeSpan Duration => End - Start;

    public bool OverlapsWith(DateTimeRange other) => Start < other.End && other.Start < End;
}




