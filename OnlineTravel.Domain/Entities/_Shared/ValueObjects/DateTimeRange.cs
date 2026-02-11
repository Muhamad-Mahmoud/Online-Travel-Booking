namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record DateTimeRange
{
    public DateTime Start { get; init; }
    public DateTime End { get; init; }

    protected DateTimeRange() { } // For EF

    public DateTimeRange(DateTime start, DateTime end)
    {
        if (end < start) throw new ArgumentException("End date cannot be before start date");
        Start = start;
        End = end;
    }

    public double TotalHours => (End - Start).TotalHours;
    public int TotalDays => (int)Math.Ceiling((End - Start).TotalDays);
    public int TotalNights => (End.Date - Start.Date).Days;

    public bool OverlapsWith(DateTimeRange other) => Start < other.End && other.Start < End;

    public bool Contains(DateTime date) => date >= Start && date <= End;

    public override string ToString() => $"{Start:yyyy-MM-dd HH:mm} to {End:yyyy-MM-dd HH:mm}";
}
