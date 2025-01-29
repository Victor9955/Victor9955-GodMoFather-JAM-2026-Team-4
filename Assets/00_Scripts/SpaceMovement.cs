using DG.Tweening;
using UnityEngine;

public class SpaceMovement : MonoBehaviour
{
    [SerializeField] private float lookSensitivity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateAngle;
    [SerializeField] private float rotateSpeed;
    [SerializeField] private NetworkClient networkClient;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Transform shipAncor;
    private Vector2 _lookInput;
    public Vector2 _moveInput;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIManager.Instance.crosshairFollow.ship = transform;
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
        Vector3 rotateTo = shipAncor.localEulerAngles;
        rotateTo.z = -_moveInput.x * rotateAngle;
        shipAncor.DOLocalRotate(rotateTo, rotateSpeed);
    }

    private void ReadInput()
    {
        _moveInput = inputManager.inputActions.Player.Move.ReadValue<Vector2>();
        _lookInput = inputManager.inputActions.Player.Look.ReadValue<Vector2>();
    }
}
