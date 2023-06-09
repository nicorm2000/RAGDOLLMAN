using UnityEngine;

/// <summary>
/// Checks if the player is balanced and if they're currently in ragdoll mode. If both are true, calls `RagdollDeactivator` to deactivate the ragdoll and reset the player's pose. If both are false, calls `RagdollActivator` to activate the ragdoll.
/// </summary>
public class RagdollCheck : MonoBehaviour
{
    [Header("Player Controller Dependencies")]
    [SerializeField] private PlayerController playerController;

    [Header("Ragdoll Activator Dependencies")]
    [SerializeField] private ActivateRagdoll activateRagdoll;

    [Header("Ragdoll Deactivator Dependencies")]
    [SerializeField] private DeactivateRagdoll deactivateRagdoll;

    /// <summary>
    /// Checks if the player is balanced and if they're currently in ragdoll mode. If both are true, calls `RagdollDeactivator` to deactivate the ragdoll and reset the player's pose. If both are false, calls `RagdollActivator` to activate the ragdoll.
    /// </summary>
    /// <param name="balanced">A boolean representing whether or not the player is balanced.</param>
    /// <param name="isRagdoll">A boolean representing whether or not the player is currently in ragdoll mode.</param>
    public void RagdollChecker(bool balanced, 
        bool isRagdoll)
    {
        //Balance on/off
        if (balanced && isRagdoll)
        {
            deactivateRagdoll.RagdollDeactivator();
        }
        else if (!balanced && !isRagdoll)
        {
            activateRagdoll.RagdollActivator();
        }
    }
}