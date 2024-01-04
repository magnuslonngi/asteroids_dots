using Unity.Entities;
using Unity.Mathematics;

public struct PlayerMovementComponent : IComponentData {
    public float thrustSpeed;
    public float maxSpeed;
    public float rotationSpeed;
    public float acceleration;
    public float deacceleration;
    public float3 velocity;
}