using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class BulletAuthoring : MonoBehaviour {
    public float speed;
    public float range;

    public class Baker : Baker<BulletAuthoring> {
        public override void Bake(BulletAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Bullet {
                speed = authoring.speed,
                range = authoring.range,
                currentDistance = 0
            });
        }
    }
}

public struct Bullet : IComponentData {
    public float speed;
    public float3 direction;
    public float range;
    public float currentDistance;
}
