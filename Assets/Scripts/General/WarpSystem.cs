using Unity.Entities;
using Unity.Transforms;
using UnityEngine;
using Unity.Mathematics;

[UpdateAfter(typeof(PlayerMovementSystem))]
[UpdateAfter(typeof(AsteroidMovementSystem))]
public partial struct WarpSystem : ISystem {

    public void OnUpdate(ref SystemState state) {
        foreach (var (warpData, localTransform) in
            SystemAPI.Query<Texture, RefRW<LocalTransform>>()) {

            float3 cameraDimension =
                Camera.main.ScreenToWorldPoint(new float3(Screen.width,
                Screen.height, Camera.main.transform.position.z));

            float heigth = warpData.spriteHeigth;
            float width = warpData.spriteWidth;

            int pixelPerUnit = warpData.pixelsPerUnit;
            int halfSprite = warpData.spriteDivision;

            float halfSpriteHeight = width / pixelPerUnit / halfSprite;
            float halfSpriteWidth = heigth / pixelPerUnit / halfSprite;

            float3 currentPosition = localTransform.ValueRO.Position;

            if (currentPosition.x - halfSpriteWidth > cameraDimension.x ||
                currentPosition.x + halfSpriteWidth < -cameraDimension.x) {

                localTransform.ValueRW.Position.x *= -1;
            }

            if (currentPosition.y - halfSpriteHeight > cameraDimension.y ||
                currentPosition.y + halfSpriteHeight < -cameraDimension.y) {

                localTransform.ValueRW.Position.y *= -1;
            }
        }
    }
}
