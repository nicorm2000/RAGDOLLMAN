using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float runningSpeed;
    [SerializeField] private float strafeSpeed;
    [SerializeField] private float jumpForce;

    [SerializeField] Animator animator;

    [SerializeField] Rigidbody hips;

    public bool isGrounded;

    private void Start()
    {
        hips = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                animator.SetBool("isWalk", true);
                animator.SetBool("isRun", true);
                hips.AddForce(hips.transform.forward * speed * runningSpeed);
            }
            else
            {
                animator.SetBool("isWalk", true);
                animator.SetBool("isRun", false);
                hips.AddForce(hips.transform.forward * speed);
            }
        }
        else
        {
            animator.SetBool("isWalk", false);
            animator.SetBool("isRun", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            animator.SetBool("isSideLeft", true);
            hips.AddForce(-hips.transform.right * strafeSpeed);
        }
        else
        {
            animator.SetBool("isSideLeft", false);
        }

        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("isWalk", true);
            hips.AddForce(-hips.transform.forward * speed);
        }
        else if(!Input.GetKey(KeyCode.W))
        {
            animator.SetBool("isWalk", false);
        }

        if (Input.GetKey(KeyCode.D))
        {
            animator.SetBool("isSideRight", true);
            hips.AddForce(hips.transform.right * strafeSpeed);
        }
        else
        {
            animator.SetBool("isSideRight", false);
        }

        if (Input.GetAxis("Jump") > 0)
        {
            if (isGrounded)
            {
                hips.AddForce(new Vector3(0, jumpForce, 0));

                isGrounded = false;
            }
        }

        if (Input.GetKey(KeyCode.V))
        {
            animator.SetBool("isJumpingJack", true);
        }
        else
        {
            animator.SetBool("isJumpingJack", false);
        }
    }
}
