using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Burst;

public partial struct AsteroidCollisionSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.Temp);

        foreach (var (playerSprite, playerTransform, playerEntity) in
            SystemAPI.Query<RefRO<Texture>, RefRO<LocalTransform>>()
            .WithAny<PlayerMovement>().WithEntityAccess()) {

            foreach (var (asteroidSprite, asteroidTransform, asteroid) in
                SystemAPI.Query<RefRO<Texture>, RefRO<LocalTransform>,
                RefRO<Asteroid>>()) {

                float3 distanceBetween =
                    math.distancesq(playerTransform.ValueRO.Position,
                    asteroidTransform.ValueRO.Position);

                float playerHalfWidth =
                    playerSprite.ValueRO.spriteWidth /
                    playerSprite.ValueRO.pixelsPerUnit /
                    playerSprite.ValueRO.spriteDivision;

                float playerHalfHeight = playerSprite.ValueRO.spriteHeigth /
                    playerSprite.ValueRO.pixelsPerUnit /
                    playerSprite.ValueRO.spriteDivision;

                float asteroidHalfWidth = asteroidSprite.ValueRO.spriteWidth /
                    asteroidSprite.ValueRO.pixelsPerUnit /
                    asteroidSprite.ValueRO.spriteDivision *
                    asteroid.ValueRO.sizeMultiplier;

                float asteroidHalfHeight = asteroidSprite.ValueRO.spriteHeigth /
                    asteroidSprite.ValueRO.pixelsPerUnit /
                    asteroidSprite.ValueRO.spriteDivision *
                    asteroid.ValueRO.sizeMultiplier;

                float distanceX = distanceBetween.x -
                    playerHalfWidth - asteroidHalfWidth;

                float distanceY = distanceBetween.y -
                    playerHalfHeight - asteroidHalfHeight;


                if (distanceX <= 0 || distanceY <= 0)
                    ecb.DestroyEntity(playerEntity);
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}