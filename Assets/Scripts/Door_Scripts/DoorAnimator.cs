using UnityEngine;

/// <summary>
/// Controls the animation state of a door object.
/// </summary>
public class DoorAnimator : MonoBehaviour
{
    [Header("Animator Configuration")]
    [SerializeField] private Animator animator;

    [SerializeField] private string doorUpAnimationParam = "Up";

    /// <summary>
    /// Sets the door animation state.
    /// </summary>
    /// <param name="state">The door animation state.</param>
    public void SetDoorState(bool state)
    {
        animator.SetBool(doorUpAnimationParam, state);
    }
}