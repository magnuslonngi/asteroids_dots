using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(PlayerInputSystem))]
public readonly partial struct PlayerMovementAspect : IAspect {
    readonly RefRW<LocalTransform> localTransform;
    readonly RefRW<PlayerMovement> movementComponent;

    public void Move(float2 input, float deltaTime) {
        float xInput = input.x;
        float yInput = input.y;
        float3 yAxis = localTransform.ValueRO.Up();

        float rotationSpeed = movementComponent.ValueRO.rotationSpeed;
        float rotationAngle = xInput * deltaTime * rotationSpeed * -1f;

        float thrustSpeed = movementComponent.ValueRO.thrustSpeed;
        float acceleration = movementComponent.ValueRO.acceleration;
        float deacceleration = movementComponent.ValueRO.deacceleration;
        float maxSpeed = movementComponent.ValueRO.maxSpeed;

        if (yInput > 0) {
            float3 thrustForce = yAxis * yInput * thrustSpeed;
            movementComponent.ValueRW.velocity +=
                thrustForce * acceleration * deltaTime;
        }
        else {
            movementComponent.ValueRW.velocity -=
                movementComponent.ValueRO.velocity * deacceleration * deltaTime;
        }

        movementComponent.ValueRW.velocity =
            math.clamp(movementComponent.ValueRW.velocity, -maxSpeed, maxSpeed);

        localTransform.ValueRW.Position +=
            movementComponent.ValueRO.velocity * deltaTime;

        localTransform.ValueRW = localTransform.ValueRO.RotateZ(rotationAngle);
    }
}
