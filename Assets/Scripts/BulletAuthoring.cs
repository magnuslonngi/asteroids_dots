using Unity.Entities;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour {
    public float speed;
}

public class BulletBaker : Baker<BulletAuthoring> {
    public override void Bake(BulletAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new BulletComponent {
            speed = authoring.speed
        });
    }
}
