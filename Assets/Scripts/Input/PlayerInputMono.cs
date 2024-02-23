using Unity.Entities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputMono : MonoBehaviour {
    PlayerInputActions inputActions;
    Entity inputEntity;
    World world;

    Vector2 moveInput;
    bool shootInput;

    void OnEnable() {
        inputActions = new();
        inputActions.Enable();

        inputActions.Movement.Move.started += OnPlayerMove;
        inputActions.Movement.Move.performed += OnPlayerMove;
        inputActions.Movement.Move.canceled += OnPlayerMove;

        inputActions.Shooting.Shoot.started += OnPlayerShoot;
        inputActions.Shooting.Shoot.canceled += OnPlayerShoot;

        world = World.DefaultGameObjectInjectionWorld;

        if (world.IsCreated && !world.EntityManager.Exists(inputEntity)) {
            inputEntity = world.EntityManager.CreateEntity();
            world.EntityManager.AddBuffer<PlayerInput>(inputEntity);
        }
    }

    void Update() {
        world.EntityManager.GetBuffer<PlayerInput>(inputEntity).Add(new PlayerInput {
            move = moveInput,
            shoot = shootInput
        });
    }

    void OnPlayerMove(InputAction.CallbackContext context) {
        moveInput = context.ReadValue<Vector2>();
    }

    void OnPlayerShoot(InputAction.CallbackContext context) {
        shootInput = context.ReadValueAsButton();
    }

    void OnDisable() {
        inputActions.Disable();

        inputActions.Movement.Move.started -= OnPlayerMove;
        inputActions.Movement.Move.performed -= OnPlayerMove;
        inputActions.Movement.Move.canceled -= OnPlayerMove;

        inputActions.Shooting.Shoot.started -= OnPlayerShoot;
        inputActions.Shooting.Shoot.canceled -= OnPlayerShoot;

        if (world.IsCreated && world.EntityManager.Exists(inputEntity))
            world.EntityManager.DestroyEntity(inputEntity);
    }
}
