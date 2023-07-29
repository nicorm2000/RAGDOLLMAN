using UnityEngine;

public class PressureSwitchPortal : MonoBehaviour
{
    [Header("Switch Configuration")]
    [SerializeField] private GameObject currentObject;

    [SerializeField] private PressureSwitchAnimator switchAnimator;

    [SerializeField] private LayerMask playerLayer;

    private void OnTriggerStay(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            currentObject.SetActive(true);

            switchAnimator.SetSwitchState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & playerLayer) != 0)
        {
            currentObject.SetActive(false);

            switchAnimator.SetSwitchState(false);
        }
    }
}
