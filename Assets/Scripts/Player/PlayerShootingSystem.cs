using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[UpdateBefore(typeof(PlayerInputSystem))]
public partial struct PlayerShootingSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.Temp);

        DynamicBuffer<PlayerInput> input;
        SystemAPI.TryGetSingletonBuffer(out input);

        foreach (var (shootingComponent, playerTransform) in
            SystemAPI.Query<RefRW<PlayerShooting>,RefRO<LocalTransform>>()) {

            foreach (var inputValue in input) {
                float fireRate = shootingComponent.ValueRO.fireRate;
                float fireTime = (float)SystemAPI.Time.ElapsedTime -
                    shootingComponent.ValueRO.lastShotTime;

                if (inputValue.shoot && fireTime >= fireRate) {
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
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
