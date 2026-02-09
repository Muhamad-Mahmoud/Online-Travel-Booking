namespace OnlineTravel.Domain.Entities._Shared.ValueObjects;

public record Duration
{
    public int Minutes { get; init; }

    protected Duration() { } // For EF

    public Duration(int minutes)
    {
        if (minutes < 0) throw new ArgumentException("Duration cannot be negative");
        Minutes = minutes;
    }

    public static Duration FromHours(int hours) => new(hours * 60);

    public int TotalHours => Minutes / 60;
    public int RemainingMinutes => Minutes % 60;

    public override string ToString() => $"{TotalHours}h {RemainingMinutes}m";
}




