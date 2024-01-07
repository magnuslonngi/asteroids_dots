using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

public partial struct PlayerSpriteSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.Temp);

        foreach (var (spritePrefab, entity) in
            SystemAPI.Query<PlayerSprite>().WithNone<PlayerAnimator>()
            .WithEntityAccess()) {

            GameObject gameObject = Object.Instantiate(spritePrefab.visual);

            PlayerAnimator animator = new PlayerAnimator {
                animator = gameObject.GetComponentInChildren<Animator>(),
                transform = gameObject.GetComponentInParent<Transform>()
            };

            ecb.AddComponent(entity, animator);
        }

        foreach (var (localTransform, animator, input) in
            SystemAPI.Query<RefRO<LocalTransform>, PlayerAnimator,
            RefRO<Input>>()) {

            animator.animator.SetBool(
                "Accelerating",
                input.ValueRO.movement.y > 0
                );

            animator.transform.position = localTransform.ValueRO.Position;
            animator.transform.rotation = localTransform.ValueRO.Rotation;
        }

        foreach (var (animator, entity) in SystemAPI.Query<PlayerAnimator>()
            .WithNone<PlayerSprite, LocalTransform>()
            .WithEntityAccess()) {

            Object.Destroy(animator.transform.gameObject);
            ecb.RemoveComponent<PlayerAnimator>(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
