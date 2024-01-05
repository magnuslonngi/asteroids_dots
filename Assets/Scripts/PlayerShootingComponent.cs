using Unity.Entities;
using Unity.Mathematics;

public struct PlayerShootingComponent : IComponentData {
    public float fireRate;
    public float lastShotTime;
    public float3 spawnPoint;
    public Entity prefab;
    public float speed;
    public float projectileScale;
}
