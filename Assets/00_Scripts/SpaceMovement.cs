using UnityEngine;

public class SpaceMovement : MonoBehaviour
{
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float moveSpeed;

    private InputSystem_Actions m_InputActions;
    private Vector2 _lookInput;
    private Vector2 _moveInput;

    public void Start()
    {
        m_InputActions = new InputSystem_Actions();
        m_InputActions.Enable();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ReadInput();
    }

    private void FixedUpdate()
    {
        Vector3 movementZ = _moveInput.y * transform.forward * moveSpeed;
        Vector3 movementX = _moveInput.x * transform.right * moveSpeed;
        Vector3 movement = movementZ + movementX;

        transform.position += movement;

        Vector3 euleurAngle = transform.eulerAngles;
        euleurAngle.x -= _lookInput.y * lookSensitivity;
        euleurAngle.y += _lookInput.x * lookSensitivity;
        transform.rotation = Quaternion.Euler(euleurAngle);
    }

    private void ReadInput()
    {
        _moveInput = m_InputActions.Player.Move.ReadValue<Vector2>();
        _lookInput = m_InputActions.Player.Look.ReadValue<Vector2>();
    }
}
