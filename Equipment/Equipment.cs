namespace CW2.Equipment;

public abstract class Equipment
{
    public int Id { get; }
    public string Name { get; }
    public EquipmentStatus Status { get; private set; }

    protected Equipment(int id, string name)
    {
        Id = id;
        Name = name;
        Status = EquipmentStatus.Available;
    }

    public void MarkAsBorrowed()
    {
        Status = EquipmentStatus.Borrowed;
    }

    public void MarkAsAvailable()
    {
        Status = EquipmentStatus.Available;
    }

    public void MarkAsUnavailable()
    {
        Status = EquipmentStatus.Unavailable;
    }
}