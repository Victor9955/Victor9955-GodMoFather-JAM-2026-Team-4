using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class SpaceMovement : MonoBehaviour
{
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float moveSpeed;

    private InputSystem_Actions m_Movement;
    private Vector2 _turnRotationInputs;
    private Vector2 _moveInputs;

    private void Start()
    {
        m_Movement = new InputSystem_Actions();
        m_Movement.Player.Enable();

        m_Movement.Player.Move.performed += Move_performed;
        m_Movement.Player.Move.canceled += Move_canceled;

        m_Movement.Player.Look.performed += Look_performed;
    }

    private void FixedUpdate()
    {
        Vector3 movementZ = _moveInputs.y * transform.forward * moveSpeed;
        Vector3 movementX = _moveInputs.x * transform.right * moveSpeed;
        Vector3 movement = movementX + movementZ;

        transform.position += movement;
    }

    private void Move_canceled(InputAction.CallbackContext obj)
    {
        _moveInputs = Vector2.zero;
        Debug.Log($"ctx move CANCELED");
    }

    private void Look_performed(InputAction.CallbackContext context)
    {
        //Debug.Log($"ctx look delta : {context.ReadValue<Vector2>()}");
        _turnRotationInputs += context.ReadValue<Vector2>();
        transform.rotation = Quaternion.Euler(-_turnRotationInputs.y * lookSensitivity, _turnRotationInputs.x * lookSensitivity, 0);
    }

    private void Move_performed(InputAction.CallbackContext context)
    {
        Debug.Log($"ctx movement : {context.ReadValue<Vector2>()}");
        _moveInputs = context.ReadValue<Vector2>();
    }
}
