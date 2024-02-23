using Unity.Entities;

public partial struct PlayerInputSystem : ISystem {
    void OnUpdate(ref SystemState state) {
        DynamicBuffer<PlayerInput> input;
        SystemAPI.TryGetSingletonBuffer(out input);

        input.Clear();
    }
}