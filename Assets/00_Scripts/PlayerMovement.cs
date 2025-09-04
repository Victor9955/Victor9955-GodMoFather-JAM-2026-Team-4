using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;

    [Header("Keybinds")]
    [SerializeField] InputAction WASD;
    

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        WASD.Enable();
    }

    private void OnDestroy()
    {
        WASD.Disable();
    }

    private void Update()
    {
        MyInput();
        SpeedControl();
        if (rb.linearVelocity.x == 0 && rb.linearVelocity.y == 0)
        {
            GetComponent<AudioSource>().loop = false;
            GetComponent<AudioSource>().Play();
        }
        else
        {
            GetComponent<AudioSource>().loop = true;
        }

    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        Vector2 input = WASD.ReadValue<Vector2>();
        horizontalInput = input.x;
        verticalInput = input.y;
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        rb.linearVelocity = new Vector3((moveDirection.normalized * moveSpeed * 10f * Time.deltaTime).x,rb.linearVelocity.y, (moveDirection.normalized * moveSpeed * 10f * Time.deltaTime).z);
       
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}