using OpenTK.Mathematics;
using Svelto.ECS;

namespace SteveClient.Engine.Components;

public struct HeadComponent : IEntityComponent
{
    /// <summary>
    /// Pitch in radians
    /// </summary>
    public float Pitch;

    public HeadComponent(float pitch)
    {
        Pitch = pitch;
    }
    
    public HeadComponent() : this(0) { }
}

public static class HeadComponentExtensions
{
    public static Vector3 GetForward(this ref HeadComponent head)
    {
        Vector3 forward = Vector3.UnitZ;
        Quaternion rotation = Quaternion.FromEulerAngles(head.Pitch, 0, 0);

        return rotation * forward;
    }
}