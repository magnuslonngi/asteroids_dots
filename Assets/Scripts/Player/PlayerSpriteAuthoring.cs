using UnityEngine;
using Unity.Entities;

public class PlayerSpriteAuthoring : MonoBehaviour {
    public GameObject playerVisual;
}

public class PlayerSpriteBaker : Baker<PlayerSpriteAuthoring> {
    public override void Bake(PlayerSpriteAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.None);

        AddComponentObject(entity, new PlayerSpriteComponent {
            visual = authoring.playerVisual
        });
    }
}
