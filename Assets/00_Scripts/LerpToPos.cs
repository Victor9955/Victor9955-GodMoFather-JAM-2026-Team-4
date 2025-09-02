using UnityEngine;

public class LerpToPos : MonoBehaviour
{
    public Vector3 pos;
    [SerializeField] float speed = 5f;
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, pos, Time.deltaTime * speed);
    }
}
