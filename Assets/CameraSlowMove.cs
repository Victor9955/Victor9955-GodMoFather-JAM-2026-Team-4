using UnityEngine;

public class CameraSlowMove : MonoBehaviour
{
    [SerializeField] float _speed;
    void Update()
    {
        transform.localEulerAngles += (Vector3)(Vector2.one * _speed * Time.deltaTime);
    }
}
