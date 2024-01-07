using UnityEngine;
using Unity.Entities;

public class PlayerSpriteAuthoring : MonoBehaviour {
    public GameObject playerVisual;

    public class PlayerSpriteBaker : Baker<PlayerSpriteAuthoring> {
        public override void Bake(PlayerSpriteAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponentObject(entity, new PlayerSprite {
                visual = authoring.playerVisual
            });
        }
    }
}

public class PlayerSprite : IComponentData {
    public GameObject visual;
}

public class PlayerAnimator : ICleanupComponentData {
    public Animator animator;
    public Transform transform;
}



