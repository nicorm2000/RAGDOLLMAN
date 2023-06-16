using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbCollision : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    [SerializeField] private LayerMask groundLayer;
    
    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & groundLayer) != 0)
        {
            playerController.isGrounded = true;
        }
    }
}
