using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public class ScreenAuthoring : MonoBehaviour {

    public class Baker : Baker<ScreenAuthoring> {
        public override void Bake(ScreenAuthoring authoring) {
            var entity = GetEntity(TransformUsageFlags.None);

            Camera camera = Camera.main;
            float width = UnityEngine.Screen.width;
            float height = UnityEngine.Screen.height;
            float positionZ = camera.transform.position.z;

            float3 dimensions = new(width, height, positionZ);

            AddComponent(entity, new Screen {
                dimesionInWorld = camera.ScreenToWorldPoint(dimensions)
            });
        }
    }
}

public struct Screen : IComponentData {
    public float3 dimesionInWorld;
}
