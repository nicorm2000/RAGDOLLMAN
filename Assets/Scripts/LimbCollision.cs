using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbCollision : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    private void OnCollisionEnter(Collision collision)
    {
        playerController.isGrounded = true;
    }
}
