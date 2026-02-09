namespace OnlineTravel.Domain.Entities.Bookings.ValueObjects;

public record GuestCapacity
{
    public int Adults { get; init; }
    public int Children { get; init; }
    public int Infants { get; init; }

    protected GuestCapacity() { } // For EF

    public GuestCapacity(int adults, int children = 0, int infants = 0)
    {
        if (adults < 0 || children < 0 || infants < 0)
            throw new ArgumentException("Capacity counts cannot be negative");

        Adults = adults;
        Children = children;
        Infants = infants;
    }

    public int Total => Adults + Children + Infants;
}




