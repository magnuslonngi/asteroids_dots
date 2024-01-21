using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct AsteroidSpriteSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.Temp);

        new InstantiateSpriteJob { ecb = ecb }.Run();
        new MoveSpriteJob { }.Run();
        new RemoveSpriteJob { ecb = ecb }.Run();

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }

    [WithNone(typeof(AsteroidAnimator))]
    public partial struct InstantiateSpriteJob : IJobEntity {
        public EntityCommandBuffer ecb;

        public void Execute(AsteroidSprite spritePrefab, in Entity entity) {
            GameObject gameObject = Object.Instantiate(spritePrefab.visual);

            AsteroidAnimator animator = new AsteroidAnimator {
                animator = gameObject.GetComponent<Animator>(),
            };

            ecb.AddComponent(entity, animator);
        }
    }

    public partial struct MoveSpriteJob : IJobEntity {

        public void Execute(ref LocalTransform localTransform,
            AsteroidAnimator animator) {

            animator.animator.transform.position = localTransform.Position;
        }
    }

    [WithNone(typeof(AsteroidSprite))]
    [WithNone(typeof(LocalTransform))]
    public partial struct RemoveSpriteJob : IJobEntity
    {
        public EntityCommandBuffer ecb;

        public void Execute(AsteroidAnimator animator, in Entity entity) {
            Object.Destroy(animator.animator.gameObject);
            ecb.RemoveComponent<AsteroidAnimator>(entity);
        }
    }
}
