using Unity.Entities;
using Unity.Mathematics;

public struct AsteroidComponent : IComponentData {
    public float speed;
    public float3 direction;
    public float sizeMultiplier;
}
