using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
public partial struct BulletMovementSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        new BulletMovementJob {
            deltaTime = SystemAPI.Time.DeltaTime
        }.Schedule();
    }
}

[BurstCompile]
public partial struct BulletMovementJob : IJobEntity {
    public float deltaTime;

    public void Execute(BulletComponent bulletComponent, RefRW<LocalTransform> bulletTransform) {
        bulletTransform.ValueRW.Position += bulletComponent.direction * deltaTime * bulletComponent.speed;
    }
}
