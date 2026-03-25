namespace Project.Equipments;
public class Laptop : Equipment
{
    public int RamGb { get; }
    public string CpuModel { get; }

    public Laptop(int id, string name, int ramGb, string cpuModel)
        : base(id, name)
    {
        RamGb = ramGb;
        CpuModel = cpuModel;
    }
}