using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class MovementAuthoring : MonoBehaviour {
    public float thrustSpeed;
    public float maxSpeed;
    public float rotationSpeed;
    public float acceleration;
    public float deacceleration;
    public float3 velocity;
}

public class MovementBaker : Baker<MovementAuthoring> {
    public override void Bake(MovementAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new MovementComponent {
            thrustSpeed = authoring.thrustSpeed,
            maxSpeed = authoring.maxSpeed,
            rotationSpeed = authoring.rotationSpeed,
            acceleration = authoring.acceleration,
            deacceleration = authoring.deacceleration
        });

        AddComponent(entity, new InputComponent { });
    }
}
