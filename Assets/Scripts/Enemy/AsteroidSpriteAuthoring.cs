using UnityEngine;
using Unity.Entities;

public class AsteroidSpriteAuthoring : MonoBehaviour {
    public GameObject value;
}

public class AsteroidSpriteBaker : Baker<AsteroidSpriteAuthoring> {
    public override void Bake(AsteroidSpriteAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponentObject(entity, new AsteroidSpriteComponent {
            visual = authoring.value
        });
    }
}
