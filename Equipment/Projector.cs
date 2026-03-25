namespace Project.Equipments;
public class Projector : Equipment
{
    public string Resolution { get; }
    public int BrightnessLumens { get; }

    public Projector(int id, string name, string resolution, int brightnessLumens)
        : base(id, name)
    {
        Resolution = resolution;
        BrightnessLumens = brightnessLumens;
    }
}