using UnityEngine;

public class PressureSwitchAnimator : MonoBehaviour
{
    [Header("Animator Configuration")]
    [SerializeField] private Animator animator;

    [SerializeField] private string switchDownAnimationParam = "Down";

    /// <summary>
    /// Sets the switch animation state.
    /// </summary>
    /// <param name="state">The switch animation state.</param>
    public void SetSwitchState(bool state)
    {
        animator.SetBool(switchDownAnimationParam, state);
    }
}