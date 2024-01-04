using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

public partial struct PlayerSpriteSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer entityCommandBuffer =
            new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (spritePrefab, entity) in
            SystemAPI.Query<PlayerSpriteComponent>()
            .WithNone<PlayerAnimatorComponent>().WithEntityAccess()) {

            GameObject gameObject =
                Object.Instantiate(spritePrefab.visual);

            PlayerAnimatorComponent animator = new PlayerAnimatorComponent {
                animator = gameObject.GetComponentInChildren<Animator>(),
                transform = gameObject.GetComponentInParent<Transform>()
            };

            entityCommandBuffer.AddComponent(entity, animator);
        }

        foreach (var (localTransform, animatorComponent, inputComponent) in
            SystemAPI.Query<LocalTransform, PlayerAnimatorComponent,
            InputComponent>()) {

            animatorComponent.animator.SetBool("Accelerating",
                inputComponent.movement.y > 0);

            animatorComponent.transform.position =
                localTransform.Position;

            animatorComponent.transform.rotation =
                localTransform.Rotation;
        }

        foreach (var (animatorComponent, entity) in
            SystemAPI.Query<PlayerAnimatorComponent>()
            .WithNone<PlayerSpriteComponent,
            LocalTransform>().WithEntityAccess()) {

            Object.Destroy(animatorComponent.transform.gameObject);
            entityCommandBuffer
                .RemoveComponent<PlayerAnimatorComponent>(entity);
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
