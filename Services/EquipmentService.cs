using CW2.Equipment;

namespace CW2.Services;

public class EquipmentService
{
    private readonly List<Equipment> _equipment = new();

    public void AddEquipment(Equipment equipment)
    {
        _equipment.Add(equipment);
    }

    public List<Equipment> GetAllEquipment()
    {
        return _equipment;
    }

    public List<Equipment> GetAvailableEquipment()
    {
        return _equipment
            .Where(e => e.Status == EquipmentStatus.Available)
            .ToList();
    }

    public Equipment GetById(int id)
    {
        Equipment equipment = _equipment.FirstOrDefault(e => e.Id == id)
            ?? throw new InvalidOperationException("Equipment not found.");

        return equipment;
    }

    public void MarkAsUnavailable(int id)
    {
        Equipment equipment = GetById(id);

        if (equipment.Status == EquipmentStatus.Borrowed)
        {
            throw new InvalidOperationException("Borrowed equipment cannot be marked as unavailable.");
        }

        equipment.MarkAsUnavailable();
    }
}