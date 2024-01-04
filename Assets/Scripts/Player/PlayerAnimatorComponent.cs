using Unity.Entities;
using UnityEngine;

public class PlayerAnimatorComponent : ICleanupComponentData {
    public Animator animator;
    public Transform transform;
}
