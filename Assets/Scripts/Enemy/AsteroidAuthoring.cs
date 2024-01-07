using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class AsteroidAuthoring : MonoBehaviour {
    public float speed;
    public float sizeMultiplier;

    public class Baker : Baker<AsteroidAuthoring> {
        public override void Bake(AsteroidAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Asteroid {
                speed = authoring.speed,
                sizeMultiplier = authoring.sizeMultiplier
            });
        }
    }
}

public struct Asteroid : IComponentData {
    public float speed;
    public float3 direction;
    public float sizeMultiplier;
}


