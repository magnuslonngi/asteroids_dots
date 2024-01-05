using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

public partial struct AsteroidSpriteSystem : ISystem {
    public void OnUpdate(ref SystemState state) {
        EntityCommandBuffer entityCommandBuffer =
            new EntityCommandBuffer(Unity.Collections.Allocator.Temp);

        foreach (var (spritePrefab, entity) in
            SystemAPI.Query<AsteroidSpriteComponent>()
            .WithNone<AsteroidAnimatorComponent>().WithEntityAccess()) {

            GameObject gameObject =
                Object.Instantiate(spritePrefab.visual);

            AsteroidAnimatorComponent animator = new AsteroidAnimatorComponent {
                animator = gameObject.GetComponent<Animator>(),
            };

            entityCommandBuffer.AddComponent(entity, animator);
        }

        foreach (var (localTransform, animatorComponent) in
            SystemAPI.Query<LocalTransform, AsteroidAnimatorComponent>()) {

            animatorComponent.animator.transform.position
                = localTransform.Position;
        }

        foreach (var (animatorComponent, entity) in
            SystemAPI.Query<AsteroidAnimatorComponent>()
            .WithNone<AsteroidSpriteComponent,
            LocalTransform>().WithEntityAccess()) {

            Object.Destroy(animatorComponent.animator.gameObject);
            entityCommandBuffer
                .RemoveComponent<AsteroidAnimatorComponent>(entity);
        }

        entityCommandBuffer.Playback(state.EntityManager);
        entityCommandBuffer.Dispose();
    }
}
