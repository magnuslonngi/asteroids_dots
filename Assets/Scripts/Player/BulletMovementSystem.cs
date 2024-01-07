using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

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

    public void Execute(in Bullet bulletComponent,
        ref LocalTransform bulletTransform) {

        bulletTransform.Position +=
            bulletComponent.direction * deltaTime * bulletComponent.speed;
    }
}
