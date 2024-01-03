using Unity.Entities;

public partial struct MovementSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        new MovementJob {
            deltaTime = SystemAPI.Time.DeltaTime
        }.Schedule();
    }
}

public partial struct MovementJob : IJobEntity {
    public float deltaTime;

    public void Execute(MovementAspect movementAspect) {
        movementAspect.Move(deltaTime);
    }
}
