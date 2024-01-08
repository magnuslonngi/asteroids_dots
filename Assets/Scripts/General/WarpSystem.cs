using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Burst;

[UpdateAfter(typeof(PlayerMovementSystem))]
[UpdateAfter(typeof(AsteroidMovementSystem))]
public partial struct WarpSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        new WarpJob {
            cameraDimension = SystemAPI.GetSingleton<Screen>().dimesionInWorld
        }.Schedule();
    }

    [BurstCompile]
    public partial struct WarpJob : IJobEntity {
        public float3 cameraDimension;

        public void Execute(in Texture texture,
            ref LocalTransform localTransform) {

            float heigth = texture.spriteHeigth;
            float width = texture.spriteWidth;

            int pixelPerUnit = texture.pixelsPerUnit;
            int halfSprite = texture.spriteDivision;

            float halfSpriteHeight = width / pixelPerUnit / halfSprite;
            float halfSpriteWidth = heigth / pixelPerUnit / halfSprite;

            float3 currentPosition = localTransform.Position;

            if (currentPosition.x - halfSpriteWidth > cameraDimension.x ||
                currentPosition.x + halfSpriteWidth < -cameraDimension.x) {

                localTransform.Position.x *= -1;
            }

            if (currentPosition.y - halfSpriteHeight > cameraDimension.y ||
                currentPosition.y + halfSpriteHeight < -cameraDimension.y) {

                localTransform.Position.y *= -1;
            }
        }
    }
}
