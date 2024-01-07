using Unity.Entities;
using Unity.Burst;

public partial struct PlayerMovementSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        new PlayerMovementJob {
            deltaTime = SystemAPI.Time.DeltaTime
        }.Schedule();
    }
}

[BurstCompile]
public partial struct PlayerMovementJob : IJobEntity {
    public float deltaTime;

    public void Execute(PlayerMovementAspect movementAspect) {
        movementAspect.Move(deltaTime);
    }
}
