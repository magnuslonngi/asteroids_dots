using UnityEngine;
using Unity.Entities;

public class WarpAuthoring : MonoBehaviour {
    public Texture2D texture;
    public int pixelsPerUnit;
    public int spriteDivision;
}

public class WarpBaker : Baker<WarpAuthoring> {
    public override void Bake(WarpAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new WarpComponent
        {
            spriteHeigth = authoring.texture.height,
            spriteWidth = authoring.texture.width,
            pixelsPerUnit = authoring.pixelsPerUnit,
            spriteDivision = authoring.spriteDivision
        });
    }
}
