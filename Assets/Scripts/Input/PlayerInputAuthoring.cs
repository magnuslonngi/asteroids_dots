using Unity.Entities;
using Unity.Mathematics;

public struct PlayerInput : IBufferElementData {
    public float2 move;
    public bool shoot;
}
