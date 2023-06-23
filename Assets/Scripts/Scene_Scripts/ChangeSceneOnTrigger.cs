using UnityEngine;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    [Header("Scene Configuration")]
    [SerializeField] int sceneToChangeTo;
    [SerializeField] LayerMask playerLayer;

    /// <summary>
    /// Triggered when the collider of this object collides with another collider.
    /// Checks if the collider belongs to the player and initiates the scene transition.
    /// </summary>
    /// <param name="other">The collider that this object has collided with.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            SceneTransitionController.Instance.StartTransition(sceneToChangeTo);
        }
    }
}