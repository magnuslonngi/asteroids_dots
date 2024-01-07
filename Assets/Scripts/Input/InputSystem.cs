using Unity.Entities;
using UnityEngine;
using Unity.Mathematics;

public partial class InputSystem : SystemBase {
    PlayerInput input;

    protected override void OnCreate() {
        input = new PlayerInput();
        input.Enable();
    }

    protected override void OnUpdate() {
        float2 moveInput = input.Movement.Move.ReadValue<Vector2>();
        bool shootInput = input.Shooting.Shoot.ReadValue<float>() > 0.5f;

        foreach (var inputComponent in SystemAPI.Query<RefRW<Input>>()) {

            inputComponent.ValueRW.movement = moveInput;
            inputComponent.ValueRW.shooting = shootInput;
        }
    }
}