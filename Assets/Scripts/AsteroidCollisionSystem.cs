using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;

[BurstCompile]
public partial struct AsteroidCollisionSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer entityCommandBuffer =
            new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (playerSprite, playerTransform, playerEntity) in
            SystemAPI.Query<WarpComponent, LocalTransform>()
            .WithAny<PlayerMovementComponent>().WithEntityAccess()) {

            foreach (var (asteroidSprite, asteroidTransform, asteroid) in
                SystemAPI.Query<WarpComponent, LocalTransform,
                AsteroidComponent>()) {

                float3 distanceBetween =
                    math.distancesq(playerTransform.Position,
                    asteroidTransform.Position);

                float playerHalfWidth =
                    playerSprite.spriteWidth
                    / playerSprite.pixelsPerUnit / playerSprite.spriteDivision;

                float playerHalfHeight = playerSprite.spriteHeigth
                    / playerSprite.pixelsPerUnit / playerSprite.spriteDivision;

                float asteroidHalfWidth = (asteroidSprite.spriteWidth
                    / asteroidSprite.pixelsPerUnit
                    / asteroidSprite.spriteDivision) * asteroid.sizeMultiplier;

                float asteroidHalfHeight = (asteroidSprite.spriteHeigth
                    / asteroidSprite.pixelsPerUnit
                    / asteroidSprite.spriteDivision) * asteroid.sizeMultiplier;

                float distanceX = distanceBetween.x
                    - playerHalfWidth - asteroidHalfWidth;

                float distanceY = distanceBetween.y
                    - playerHalfHeight - asteroidHalfHeight;


                if (distanceX <= 0 || distanceY <= 0) {
                    entityCommandBuffer.DestroyEntity(playerEntity);
                }
            }
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}