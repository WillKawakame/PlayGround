using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    // Movement control values
    public float speed;
    public float jumpForce;
    public float mouseSens;

    // Ground Checking
    public Transform groundCheck;
    public LayerMask isGround;
    private bool isGrounded;

    // Other 
    private Rigidbody rb => GetComponent<Rigidbody>();
    private float xrotation;

    void Start() => Cursor.lockState = CursorLockMode.Locked;


    void Update() {

        MouseLook();
        CheckJumpState();
        Debug.Log(isGrounded);
        
    }

    private void FixedUpdate() {
       Move();
    }

    private void Move(){
        // Get horizontal and vertical values
        float x = Input.GetAxis( "Horizontal" ) * Time.deltaTime;
        float z = Input.GetAxis( "Vertical" ) * Time.deltaTime;

        // Direction of move
        Vector3 moveDirection =  Camera.main.transform.forward * z + Camera.main.transform.right * x;

        // Apply movement
        rb.velocity = new Vector3(moveDirection.x * speed, rb.velocity.y, moveDirection.z * speed);
    }

    private void MouseLook() {
        // Get mouse coords values
        float mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

        // Set the rotation values
        xrotation -= mouseY;
        xrotation = Mathf.Clamp(xrotation, -90f, 90f);

        // Apply Rotation
        Camera.main.transform.localRotation = Quaternion.Euler(xrotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void CheckJumpState() {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.2f, isGround);

        if (isGrounded) {
            if (Input.GetKeyDown("space")) {
                Jump();
            }
        }
    }

    private void Jump() {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }
}