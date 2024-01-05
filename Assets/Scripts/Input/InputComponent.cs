using Unity.Entities;
using Unity.Mathematics;

public struct InputComponent : IComponentData {
    public float2 movement;
    public bool shooting;
}
