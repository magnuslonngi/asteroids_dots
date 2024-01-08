using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public partial struct AsteroidMovementSystem : ISystem {
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

    public partial struct AsteroidInitializeJob : IJobEntity {
        public void Execute(ref Asteroid asteroid, in Random random) {
            asteroid.direction =
                new float3(random.value.NextFloat2(-1f, 1f), 0);
        }
    }

    public partial struct AsteroidMovementJob : IJobEntity {
        public float deltaTime;

        public void Execute(in Asteroid asteroid,
            ref LocalTransform localTransform) {

            float3 direction = math.normalize(asteroid.direction);
            localTransform.Position += direction * asteroid.speed * deltaTime;
        }
    }

}
