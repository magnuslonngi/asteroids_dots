using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class PlayerShootingAuthoring : MonoBehaviour {
    public float fireRate;
    public float3 spawnPoint;
    public GameObject prefab;

    public class Baker : Baker<PlayerShootingAuthoring> {
        public override void Bake(PlayerShootingAuthoring authoring) {
            Entity entity = GetEntity(TransformUsageFlags.None);
            AddComponent(entity, new PlayerShooting {
                fireRate = authoring.fireRate,
                spawnPoint = authoring.spawnPoint,
                prefab = GetEntity(authoring.prefab, TransformUsageFlags.None)
            });
        }
    }
}

public struct PlayerShooting : IComponentData {
    public float fireRate;
    public float lastShotTime;
    public float3 spawnPoint;
    public Entity prefab;
}


