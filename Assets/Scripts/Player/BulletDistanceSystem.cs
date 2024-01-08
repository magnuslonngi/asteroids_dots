using Unity.Entities;
using Unity.Burst;

[UpdateAfter(typeof(BulletMovementSystem))]
public partial struct BulletDistanceSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.TempJob);

        var bulletJob = new BulletDistanceJob {
            deltaTime = SystemAPI.Time.DeltaTime,
            ecb = ecb
        };

        bulletJob.Schedule(state.Dependency).Complete();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [BurstCompile]
    public partial struct BulletDistanceJob : IJobEntity {
        public float deltaTime;
        public EntityCommandBuffer ecb;

        public void Execute(ref Bullet bullet, in Entity entity) {
            bullet.currentDistance += bullet.speed * deltaTime;
            if (bullet.currentDistance > bullet.range)
                ecb.DestroyEntity(entity);
        }
    }
}


