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
    private Vector2 lookInput;
    public Vector2 moveInput;

    public Vector3 visualError;
    public Vector3 baseTransformPos;

    public float MoveSpeed => moveSpeed;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        UIManager.instance.crosshairFollow.ship = transform;
        baseTransformPos = transform.position;
    }

    public void SetPositionRotation(Vector3 position, Quaternion rotation)
    {
        baseTransformPos = position;
        transform.rotation = rotation;
    }

    public void AdvanceSpaceShip (float TickDelay)
    {
        Vector3 euleurAngle = transform.eulerAngles;
        euleurAngle.x -= lookInput.y * lookSensitivity * TickDelay;
        euleurAngle.y += lookInput.x * lookSensitivity * TickDelay;
        transform.rotation = Quaternion.Euler(euleurAngle);
        Vector3 rotateTo = shipAncor.localEulerAngles;
        rotateTo.z = -moveInput.x * rotateAngle;
        shipAncor.DOLocalRotate(rotateTo, rotateSpeed);

        Vector3 movementZ = moveInput.y * transform.forward * moveSpeed * TickDelay;
        Vector3 movementX = moveInput.x * transform.right * moveSpeed * TickDelay;
        Vector3 movement = movementZ + movementX;

        //Debug.Log("move input : " + moveInput + " / movement : " + movement + " moveSpeed : " + moveSpeed + " tick delay : " + TickDelay);

        baseTransformPos += movement;
        //transform.position += movement;
    }

    public void AdvanceSpaceShip (Vector2 moveInput, Quaternion rotation, float TickDelay)
    {
        transform.rotation = rotation;

        Vector3 movementZ = moveInput.y * transform.forward * moveSpeed * TickDelay;
        Vector3 movementX = moveInput.x * transform.right * moveSpeed * TickDelay;
        Vector3 movement = movementZ + movementX;

        baseTransformPos += movement;
        //transform.position += movement;
    }

    private void Update()
    {
        ReadInput();
        transform.position = Vector3.Lerp(transform.position, baseTransformPos + visualError, 0.8f);
        visualError *= 0.9f;
    }

    private void ReadInput()
    {
        moveInput = inputManager.inputActions.Player.Move.ReadValue<Vector2>();
        lookInput = inputManager.inputActions.Player.Look.ReadValue<Vector2>();
    }
}
