using UnityEngine;

public class RotateFace : MonoBehaviour
{
    public Quaternion Dir;

    private void Update()
    {
        transform.rotation = Dir;
    }
}
