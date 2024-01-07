using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public readonly partial struct PlayerMovementAspect : IAspect {
    readonly RefRW<LocalTransform> localTransform;
    readonly RefRW<PlayerMovement> movementComponent;
    readonly RefRO<Input> inputComponent;

    public void Move(float deltaTime) {
        float xInput = inputComponent.ValueRO.movement.x;
        float yInput = inputComponent.ValueRO.movement.y;
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
