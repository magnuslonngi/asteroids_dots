using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;

public partial struct BulletCollisionSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.TempJob);

        foreach (var (asteroidSprite, asteroidTransform, asteroid, asteroidEntity) in
            SystemAPI.Query<RefRO<Texture>, RefRO<LocalTransform>,
            RefRO<Asteroid>>().WithEntityAccess()) {

            new BulletCollisionJob {
                ecb = ecb,
                asteroidTransform = asteroidTransform.ValueRO,
                asteroidSprite = asteroidSprite.ValueRO,
                asteroid = asteroid.ValueRO,
                asteroidEntity = asteroidEntity
            }.Schedule(state.Dependency).Complete();
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [WithAny(typeof(Bullet))]
    public partial struct BulletCollisionJob : IJobEntity {
        public EntityCommandBuffer ecb;
        public LocalTransform asteroidTransform;
        public Texture asteroidSprite;
        public Asteroid asteroid;
        public Entity asteroidEntity;

        void Execute(in Texture bulletSprite, in LocalTransform bulletTransform, 
            in Entity bulletEntity) {
            
            float3 distanceBetween = math.distancesq(bulletTransform.Position,
                asteroidTransform.Position);

            float bulletHalfWidth = bulletSprite.spriteWidth / bulletSprite.pixelsPerUnit /
                bulletSprite.spriteDivision;

            float bulletHalfHeigth = bulletSprite.spriteHeigth / bulletSprite.pixelsPerUnit /
                bulletSprite.spriteDivision;

            float asteroidHalfWidth = asteroidSprite.spriteWidth / asteroidSprite.pixelsPerUnit /
                asteroidSprite.spriteDivision * asteroid.sizeMultiplier;

            float asteroidHalfHeight = asteroidSprite.spriteHeigth / asteroidSprite.pixelsPerUnit /
                asteroidSprite.spriteDivision * asteroid.sizeMultiplier;

            float distanceX = distanceBetween.x - bulletHalfWidth - asteroidHalfWidth;

            float distanceY = distanceBetween.y - bulletHalfHeigth - asteroidHalfHeight;

            if (distanceX <= 0 || distanceY <= 0) {
                ecb.DestroyEntity(asteroidEntity);
                ecb.DestroyEntity(bulletEntity);
            }
        }
    }
}
