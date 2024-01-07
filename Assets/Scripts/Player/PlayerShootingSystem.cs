using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

public partial struct PlayerShootingSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.Temp);

        foreach (var (inputComponent, shootingComponent, playerTransform) in
            SystemAPI.Query<RefRO<Input>,
            RefRW<PlayerShooting>,
            RefRO<LocalTransform>>()) {

            float fireRate = shootingComponent.ValueRO.fireRate;
            float fireTime = (float)SystemAPI.Time.ElapsedTime -
                shootingComponent.ValueRO.lastShotTime;

            if (inputComponent.ValueRO.shooting && fireTime >= fireRate) {
                var bullet = shootingComponent.ValueRO.prefab;

                var entityTransform =
                    SystemAPI.GetComponentRW<LocalTransform>(bullet);

                var entityBullet =
                    SystemAPI.GetComponentRW<Bullet>(bullet);

                entityTransform.ValueRW.Position =
                    playerTransform.ValueRO.Position +
                    shootingComponent.ValueRO.spawnPoint *
                    playerTransform.ValueRO.Up();

                entityBullet.ValueRW.direction = playerTransform.ValueRO.Up();

                shootingComponent.ValueRW.lastShotTime =
                    (float)SystemAPI.Time.ElapsedTime;

                ecb.Instantiate(bullet);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
