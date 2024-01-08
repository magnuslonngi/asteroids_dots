using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

public partial struct BulletMovementSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        var bulletJob = new BulletMovementJob {
            deltaTime = SystemAPI.Time.DeltaTime
        };

        bulletJob.Schedule();
    }

    [BurstCompile]
    public partial struct BulletMovementJob : IJobEntity{
        public float deltaTime;

        public void Execute(in Bullet bullet,
            ref LocalTransform bulletTransform) {

            bulletTransform.Position += bullet.direction *
                deltaTime * bullet.speed;
        }
    }
}

