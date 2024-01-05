using UnityEngine;
using Unity.Entities;

public class AsteroidAuthoring : MonoBehaviour {
    public float speed;
    public float sizeMultiplier;
}

public class AsteroidBaker : Baker<AsteroidAuthoring> {
    public override void Bake(AsteroidAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.Dynamic);
        AddComponent(entity, new AsteroidComponent {
            speed = authoring.speed,
            sizeMultiplier = authoring.sizeMultiplier
        });
        AddComponent(entity, new RandomComponent {
            value = Unity.Mathematics.Random
            .CreateFromIndex((uint)Random.Range(0f, 100f))
        });
    }
}
