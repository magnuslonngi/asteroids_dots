using Unity.Entities;
using Unity.Burst;
using Unity.Transforms;

[BurstCompile]
public partial struct PlayerShootingSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer entityCommandBuffer;

        entityCommandBuffer =
            new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (inputComponent, shootingComponent, playerTransform) in SystemAPI.Query<InputComponent, RefRW<PlayerShootingComponent>, LocalTransform>()) {
            float fireRate = shootingComponent.ValueRO.fireRate;

            if (inputComponent.shooting) {
                if (SystemAPI.Time.ElapsedTime - shootingComponent.ValueRO.lastShotTime >= fireRate) {
                    var entity = entityCommandBuffer.Instantiate(shootingComponent.ValueRO.prefab);
                    entityCommandBuffer.SetComponent(entity, new LocalTransform { Position = playerTransform.Position + shootingComponent.ValueRO.spawnPoint * playerTransform.Up(), Scale = shootingComponent.ValueRO.projectileScale });
                    entityCommandBuffer.SetComponent(entity, new BulletComponent { speed = shootingComponent.ValueRO.speed, direction = playerTransform.Up() });
                    shootingComponent.ValueRW.lastShotTime = (float)SystemAPI.Time.ElapsedTime;
                }
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
