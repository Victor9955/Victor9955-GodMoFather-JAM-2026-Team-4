using DG.Tweening;
using UnityEngine;

public class SpaceMovement : MonoBehaviour
{
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateAngle;
    [SerializeField] private float rotateSpeed;

    private InputSystem_Actions m_InputActions;
    [SerializeField] private Transform m_ship;
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

        Vector3 movementZ = _moveInput.y * transform.forward * moveSpeed * Time.deltaTime;
        Vector3 movementX = _moveInput.x * transform.right * moveSpeed * Time.deltaTime;
        Vector3 movement = movementZ + movementX;

        transform.position += movement;

        Vector3 euleurAngle = transform.eulerAngles;
        euleurAngle.x -= _lookInput.y * lookSensitivity * Time.deltaTime;
        euleurAngle.y += _lookInput.x * lookSensitivity * Time.deltaTime;
        transform.rotation = Quaternion.Euler(euleurAngle);
        Vector3 rotateTo = m_ship.localEulerAngles;
        rotateTo.z = -_moveInput.x * rotateAngle;
        m_ship.DOLocalRotate(rotateTo, rotateSpeed);
    }

    private void FixedUpdate()
    {
    }

    private void ReadInput()
    {
        _moveInput = m_InputActions.Player.Move.ReadValue<Vector2>();
        _lookInput = m_InputActions.Player.Look.ReadValue<Vector2>();
    }
}
