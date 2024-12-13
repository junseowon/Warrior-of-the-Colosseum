using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] float rotationSpeed = 500f;

    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    float ySpeed;

    float stopSpeed = 0f;
    float walkSpeed = 3f;
    float runSpeed = 8f;

    bool isAttack = false;

    float animationAmount = 0f;

    Quaternion targetRotation;

    CameraController cameraController;
    Animator animator;
    CharacterController characterController;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        bool click = Input.GetMouseButtonDown(0);

        bool focus = Input.GetKey(KeyCode.LeftShift);

        float moveAmount = Mathf.Clamp01(Mathf.Abs(h) + Mathf.Abs(v));

        var moveInput = (new Vector3(h, 0, v)).normalized;

        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();

        if (click)
        {
            AttackSlash();
        }       

        if (focus)
        {
            moveSpeed = runSpeed;
            animationAmount = 1;
        }
        else
        {
            
            moveSpeed = walkSpeed;
            animationAmount = 0.3f;
            
            
        }

        if (isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }
        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;

        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 
                                                      rotationSpeed * Time.deltaTime);

        cameraController.focusCamera(focus);
        animator.SetFloat("moveAmount", moveAmount * animationAmount, 0.2f, Time.deltaTime);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    void AttackSlash()
    {
        animator.SetTrigger("Slash");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}
