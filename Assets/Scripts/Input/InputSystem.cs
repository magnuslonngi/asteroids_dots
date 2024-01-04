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

        foreach (RefRW<InputComponent> inputComponent
            in SystemAPI.Query<RefRW<InputComponent>>()) {

            inputComponent.ValueRW.movement = moveInput;
        }
    }
}