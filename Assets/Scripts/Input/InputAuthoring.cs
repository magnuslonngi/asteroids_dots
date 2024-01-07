using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class InputAuthoring : MonoBehaviour {
    public class Baker : Baker<InputAuthoring>{
        public override void Bake(InputAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new Input { });
        }
    }
}

public struct Input : IComponentData {
    public float2 movement;
    public bool shooting;
}
