using Unity.Entities;
using UnityEngine;

public class RandomAuthoring : MonoBehaviour {
    public class Baker : Baker<RandomAuthoring> {
        public override void Bake(RandomAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Random {
                value = Unity.Mathematics.Random
                .CreateFromIndex((uint)UnityEngine.Random.Range(0f, 100f))
            });
        }
    }
}

public struct Random : IComponentData {
    public Unity.Mathematics.Random value;
}
