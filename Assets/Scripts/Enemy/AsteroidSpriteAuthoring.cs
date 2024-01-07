using UnityEngine;
using Unity.Entities;

public class AsteroidSpriteAuthoring : MonoBehaviour {
    public GameObject value;

    public class Baker : Baker<AsteroidSpriteAuthoring> {
        public override void Bake(AsteroidSpriteAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponentObject(entity, new AsteroidSprite {
                visual = authoring.value
            });
        }
    }
}

public class AsteroidAnimator : ICleanupComponentData {
    public Animator animator;
}

public class AsteroidSprite : IComponentData {
    public GameObject visual;
}



