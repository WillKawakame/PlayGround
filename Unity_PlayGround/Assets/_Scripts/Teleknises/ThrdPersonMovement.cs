using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody), typeof(PlayerInput))]
public class ThrdPersonMovement : MonoBehaviour
{
    // Movement Control values
    public float playerSpeed = 5.0f;
    public float jumpForce = 1.0f;
    public float mouseSens = 5f;
    public Animator anim;


    // Ground Checking
    public Transform groundCheck;
    public LayerMask isGround;
    private bool isGrounded;


    private Rigidbody rb => GetComponent<Rigidbody>();


    private PlayerInput playerInput => GetComponent<PlayerInput>();
    private InputAction moveAction => playerInput.actions["Move"];
    private InputAction jumpAction => playerInput.actions["Jump"];


    private Transform camTransform => Camera.main.transform;


    private Vector3 playerVelocity;


    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
       CheckJumpState();
       Animation();

        if(camTransform.gameObject.GetComponent<Telekinesis>().canPull) Rotation1();
        else Rotation2();
    }

    void Rotation1(){
        Vector2 value = moveAction.ReadValue<Vector2>();
        float gunRotation = Mathf.Atan2(value.x, value.y) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, camTransform.eulerAngles.y + gunRotation, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, mouseSens * Time.deltaTime * 2);
    }

    void Rotation2(){
        Quaternion rotation = Quaternion.Euler(0, camTransform.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, mouseSens * Time.deltaTime);
    }

    void Animation(){
        Vector2 value = moveAction.ReadValue<Vector2>();
        if( value.magnitude > 0){
            anim.SetBool("Walking", true);
        }
        if(value.magnitude == 0){
            anim.SetBool("Walking", false);
        }
    }

    void FixedUpdate(){
        Move();
        CheckJumpState();
    }

    private void Move(){
        Vector2 value = moveAction.ReadValue<Vector2>();
        Vector3 moveDirection = ( Camera.main.transform.forward * value.y ) + ( Camera.main.transform.right * value.x );

        // Apply movement
        rb.velocity = new Vector3(moveDirection.x * playerSpeed, rb.velocity.y, moveDirection.z * playerSpeed);
    }

    private void CheckJumpState() {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.3f, isGround);

        if (jumpAction.triggered && isGrounded) {
            Jump();
        }
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}
