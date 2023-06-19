using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollCheck : MonoBehaviour
{
    /// <summary>
    /// Check the ragdoll state, balanced or not, so that it can be changed.
    /// </summary>
    private void RagdollChecker(bool balanced, bool isRagdoll)
    {
        //Balance on/off
        if (balanced && isRagdoll)
        {
            //DeactivateRagdoll();
        }
        else if (!balanced && !isRagdoll)
        {
            //ActivateRagdoll();
        }
    }
}
