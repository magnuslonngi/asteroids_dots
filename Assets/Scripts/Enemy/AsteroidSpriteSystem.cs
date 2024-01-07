using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct AsteroidSpriteSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer ecb = new(Unity.Collections.Allocator.Temp);

        foreach (var (spritePrefab, entity) in
            SystemAPI.Query<AsteroidSprite>()
            .WithNone<AsteroidAnimator>().WithEntityAccess()) {

            GameObject gameObject = Object.Instantiate(spritePrefab.visual);

            AsteroidAnimator animator = new AsteroidAnimator {
                animator = gameObject.GetComponent<Animator>(),
            };

            ecb.AddComponent(entity, animator);
        }

        foreach (var (localTransform, animator) in
            SystemAPI.Query<RefRO<LocalTransform>, AsteroidAnimator>()) {

            animator.animator.transform.position
                = localTransform.ValueRO.Position;
        }

        foreach (var (animatorComponent, entity) in
            SystemAPI.Query<AsteroidAnimator>()
            .WithNone<AsteroidSprite, LocalTransform>().WithEntityAccess()) {

            Object.Destroy(animatorComponent.animator.gameObject);
            ecb.RemoveComponent<AsteroidAnimator>(entity);
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
