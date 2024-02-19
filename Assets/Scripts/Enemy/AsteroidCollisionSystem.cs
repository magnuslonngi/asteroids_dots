using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;

public partial struct AsteroidCollisionSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.TempJob);

        foreach (var (playerSprite, playerTransform, playerEntity) in
            SystemAPI.Query<RefRO<Texture>, RefRO<LocalTransform>>()
            .WithAny<PlayerMovement>().WithEntityAccess()) {
            
            new AsteroidCollisionJob() {
                ecb = ecb,
                playerTransform = playerTransform.ValueRO,
                playerSprite = playerSprite.ValueRO,
                playerEntity = playerEntity
            }.Schedule(state.Dependency).Complete();
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    public partial struct AsteroidCollisionJob : IJobEntity {
        public EntityCommandBuffer ecb;
        public LocalTransform playerTransform;
        public Texture playerSprite;
        public Entity playerEntity;

        void Execute(in Texture asteroidSprite, in LocalTransform asteroidTransform,
            in Asteroid asteroid) {
            
            float3 distanceBetween = math.distancesq(playerTransform.Position, 
                asteroidTransform.Position);

            float playerHalfWidth = playerSprite.spriteWidth / playerSprite.pixelsPerUnit /
                playerSprite.spriteDivision;

            float playerHalfHeight = playerSprite.spriteHeigth / playerSprite.pixelsPerUnit /
                playerSprite.spriteDivision;

            float asteroidHalfWidth = asteroidSprite.spriteWidth / asteroidSprite.pixelsPerUnit / 
                asteroidSprite.spriteDivision * asteroid.sizeMultiplier;

            float asteroidHalfHeight = asteroidSprite.spriteHeigth / asteroidSprite.pixelsPerUnit / 
                asteroidSprite.spriteDivision * asteroid.sizeMultiplier;

            float distanceX = distanceBetween.x - playerHalfWidth - asteroidHalfWidth;

            float distanceY = distanceBetween.y - playerHalfHeight - asteroidHalfHeight;


            if (distanceX <= 0 || distanceY <= 0)
                ecb.DestroyEntity(playerEntity);
        }
    }
}