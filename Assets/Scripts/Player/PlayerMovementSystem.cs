using Unity.Entities;
using Unity.Burst;

[UpdateBefore(typeof(PlayerInputSystem))]
public partial struct PlayerMovementSystem : ISystem {

    [BurstCompile]
    public void OnUpdate(ref SystemState state) {
        DynamicBuffer<PlayerInput> input;
        SystemAPI.TryGetSingletonBuffer(out input);

        new PlayerMovementJob {
            input = input,
            deltaTime = SystemAPI.Time.DeltaTime
        }.Schedule();
    }

    [BurstCompile]
    public partial struct PlayerMovementJob : IJobEntity {
        public DynamicBuffer<PlayerInput> input;
        public float deltaTime;

        public void Execute(PlayerMovementAspect movementAspect){
            foreach(var inputValue in input)
                movementAspect.Move(inputValue.move, deltaTime);
        }
    }
}
