using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    /// <summary>
    /// Calculates the rotation of the playerParts GameObject based on the direction the Camera is facing and the turnSpeed parameter. 
    /// </summary>
    /// <param name="cam">The Camera object representing the player's point of view.</param>
    /// <param name="playerParts">The GameObject representing the player's body parts.</param>
    /// <param name="turnSpeed">The speed at which the player will be rotated.</param>
    public void PlayerRotationCalculation(Camera cam, 
        GameObject playerParts, 
        float turnSpeed)
    {
        //Camera direction and turn of camera.
        var lookPos = cam.transform.forward;

        lookPos.y = 0;

        var rotation = Quaternion.LookRotation(lookPos);

        //This allows for a smooth rotation of the character.
        playerParts.GetComponent<ConfigurableJoint>().targetRotation = Quaternion.Slerp(playerParts.GetComponent<ConfigurableJoint>().targetRotation, 
            Quaternion.Inverse(rotation), 
            Time.deltaTime * turnSpeed);
    }
}