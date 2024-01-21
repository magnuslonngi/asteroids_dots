using Unity.Entities;
using UnityEngine;
using Unity.Transforms;

public partial struct PlayerSpriteSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.TempJob);

        new InstantiateSpriteJob { ecb = ecb }.Run();
        new MoveSpriteJob { }.Run();
        new RemoveSpriteJob { ecb = ecb }.Run();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [WithNone(typeof(PlayerAnimator))]
    public partial struct InstantiateSpriteJob : IJobEntity {
        public EntityCommandBuffer ecb;

        public void Execute(PlayerSprite spritePrefab, in Entity entity) {
            GameObject gameObject = Object.Instantiate(spritePrefab.visual);

            PlayerAnimator animator = new PlayerAnimator {
                animator = gameObject.GetComponentInChildren<Animator>(),
                parentTransform = gameObject.GetComponentInParent<Transform>()
            };

            ecb.AddComponent(entity, animator);
        }
    }

    public partial struct MoveSpriteJob : IJobEntity {

        public void Execute(ref LocalTransform localTransform,
            PlayerAnimator animator, in Input input) {

            animator.animator.SetBool("Accelerating", input.movement.y > 0);
            animator.parentTransform.position = localTransform.Position;
            animator.parentTransform.rotation = localTransform.Rotation;
        }
    }

    [WithNone(typeof(PlayerSprite))]
    [WithNone(typeof(LocalTransform))]
    public partial struct RemoveSpriteJob : IJobEntity {
        public EntityCommandBuffer ecb;

        public void Execute(PlayerAnimator animator, in Entity entity) {
            Object.Destroy(animator.parentTransform.gameObject);
            ecb.RemoveComponent<PlayerAnimator>(entity);
        }
    }
}
