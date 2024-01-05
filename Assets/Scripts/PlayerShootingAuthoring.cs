using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;

public class PlayerShootingAuthoring : MonoBehaviour {
    public float fireRate;
    public float3 spawnPoint;
    public GameObject prefab;
    public float speed;
    public float projectileScale;
}

public class PlayerShootingBaker : Baker<PlayerShootingAuthoring> {
    public override void Bake(PlayerShootingAuthoring authoring) {
        Entity entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new PlayerShootingComponent {
            fireRate = authoring.fireRate,
            spawnPoint = authoring.spawnPoint,
            prefab = GetEntity(authoring.prefab, TransformUsageFlags.None),
            speed = authoring.speed,
            projectileScale = authoring.projectileScale
        });
    }
}
