namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record DateRange
{
    public DateOnly Start { get; init; }
    public DateOnly End { get; init; }

    protected DateRange() { } // For EF

    public DateRange(DateOnly start, DateOnly end)
    {
        if (end < start) throw new ArgumentException("End date cannot be before start date");
        Start = start;
        End = end;
    }

    public int TotalDays => End.DayNumber - Start.DayNumber;
    public int TotalNights => TotalDays;

    public bool OverlapsWith(DateRange other) => Start < other.End && other.Start < End;

    public bool Contains(DateOnly date) => date >= Start && date <= End;

    public override string ToString() => $"{Start:yyyy-MM-dd} to {End:yyyy-MM-dd}";
}
