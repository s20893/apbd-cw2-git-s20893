namespace CW2.Equipment;

public class Camera : Equipment
{
    public string SensorType { get; }
    public string LensMount { get; }

    public Camera(int id, string name, string sensorType, string lensMount)
        : base(id, name)
    {
        SensorType = sensorType;
        LensMount = lensMount;
    }
}