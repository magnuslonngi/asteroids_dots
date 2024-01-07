using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class PlayerMovementAuthoring : MonoBehaviour {
    public float thrustSpeed;
    public float maxSpeed;
    public float rotationSpeed;
    public float acceleration;
    public float deacceleration;

    public class Baker : Baker<PlayerMovementAuthoring> {
        public override void Bake(PlayerMovementAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new PlayerMovement {
                thrustSpeed = authoring.thrustSpeed,
                maxSpeed = authoring.maxSpeed,
                rotationSpeed = authoring.rotationSpeed,
                acceleration = authoring.acceleration,
                deacceleration = authoring.deacceleration
            });
        }
    }
}

public struct PlayerMovement : IComponentData {
    public float thrustSpeed;
    public float maxSpeed;
    public float rotationSpeed;
    public float acceleration;
    public float deacceleration;
    public float3 velocity;
}

