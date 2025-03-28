using Unity.VisualScripting;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    private UnityEngine.CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private float jumpHeight = 3.0f;
    private float gravityValue = -9.81f;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        controller = gameObject.GetComponent<UnityEngine.CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = -2f;
            animator.SetBool("isJumping", false);
        }


        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            animator.SetBool("isJumping", true);
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
