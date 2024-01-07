using UnityEngine;
using Unity.Entities;

public class TextureAuthoring : MonoBehaviour {
    public Texture2D texture;
    public int pixelsPerUnit;
    public int spriteDivision;

    public class Baker : Baker<TextureAuthoring> {
        public override void Bake(TextureAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Texture {
                spriteHeigth = authoring.texture.height,
                spriteWidth = authoring.texture.width,
                pixelsPerUnit = authoring.pixelsPerUnit,
                spriteDivision = authoring.spriteDivision
            });
        }
    }
}

public struct Texture : IComponentData {
    public float spriteHeigth;
    public float spriteWidth;
    public int pixelsPerUnit;
    public int spriteDivision;
}
