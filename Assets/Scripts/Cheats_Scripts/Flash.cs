using UnityEngine;

public class Flash : MonoBehaviour
{
    [Header("Player Controller Configuration")]
    [SerializeField] PlayerController playerController;
    [SerializeField] float flashMoveSpeedMultiplier = 3f;

    private bool toggleFlash = false;

    public void ToggleFlash()
    {
        toggleFlash = !toggleFlash;
        
        if (toggleFlash)
        {
            playerController.moveSpeed *= flashMoveSpeedMultiplier;
        }
        else
        {
            playerController.moveSpeed /= flashMoveSpeedMultiplier;
        }
    }
}