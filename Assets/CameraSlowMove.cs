using UnityEngine;

public class CameraSlowMove : MonoBehaviour
{
    [SerializeField] float _speed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    void Update()
    {
        transform.localEulerAngles += (Vector3)(Vector2.one * _speed * Time.deltaTime);
    }
}
