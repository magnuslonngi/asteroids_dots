using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;

public partial struct BulletCollisionSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.Temp);

        foreach (var (asteroidSprite, asteroidTransform, asteroid,
            asteroidEntity) in
            SystemAPI.Query<RefRO<Texture>, RefRO<LocalTransform>,
            RefRO<Asteroid>>().WithEntityAccess()) {

            foreach (var (bulletSprite, bulletTransform, bulletEntity)
                in SystemAPI.Query<RefRO<Texture>, RefRO<LocalTransform>>()
                .WithAny<Bullet>().WithEntityAccess()) {

                float3 distanceBetween =
                    math.distancesq(bulletTransform.ValueRO.Position,
                    asteroidTransform.ValueRO.Position);

                float bulletHalfWidth =
                    bulletSprite.ValueRO.spriteWidth /
                    bulletSprite.ValueRO.pixelsPerUnit /
                    bulletSprite.ValueRO.spriteDivision;

                float bulletHalfHeigth = bulletSprite.ValueRO.spriteHeigth /
                    bulletSprite.ValueRO.pixelsPerUnit /
                    bulletSprite.ValueRO.spriteDivision;

                float asteroidHalfWidth = asteroidSprite.ValueRO.spriteWidth /
                    asteroidSprite.ValueRO.pixelsPerUnit /
                    asteroidSprite.ValueRO.spriteDivision *
                    asteroid.ValueRO.sizeMultiplier;

                float asteroidHalfHeight = asteroidSprite.ValueRO.spriteHeigth /
                    asteroidSprite.ValueRO.pixelsPerUnit /
                    asteroidSprite.ValueRO.spriteDivision *
                    asteroid.ValueRO.sizeMultiplier;

                float distanceX = distanceBetween.x -
                    bulletHalfWidth - asteroidHalfWidth;

                float distanceY = distanceBetween.y -
                    bulletHalfHeigth - asteroidHalfHeight;


                if (distanceX <= 0 || distanceY <= 0) {
                    ecb.DestroyEntity(asteroidEntity);
                    ecb.DestroyEntity(bulletEntity);
                }
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
