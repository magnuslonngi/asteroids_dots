using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;
using UnityEngine;

public partial struct AsteroidSystem : ISystem {
    bool initialize;

    public void OnCreate(ref SystemState state) {
        initialize = true;
    }

    public void OnUpdate(ref SystemState state) {
        if (initialize)
            new AsteroidInitializeJob { }.Run();
        initialize = false;

        new AsteroidMovementJob {
            deltaTime = SystemAPI.Time.DeltaTime
        }.Schedule();
    }
}

public partial struct AsteroidInitializeJob : IJobEntity {
    public void Execute(RefRW<AsteroidComponent> asteroid, RandomComponent random) {
        asteroid.ValueRW.direction = new float3(random.value.NextFloat2(-1f, 1f), 0);
    }
}

public partial struct AsteroidMovementJob : IJobEntity {
    public float deltaTime;

    public void Execute(AsteroidComponent asteroid, RefRW<LocalTransform> localTransform) {
        localTransform.ValueRW.Position += math.normalize(asteroid.direction) * asteroid.speed * deltaTime;
    }
}
