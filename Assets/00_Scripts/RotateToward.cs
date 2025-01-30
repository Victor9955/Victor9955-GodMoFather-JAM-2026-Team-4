using UnityEngine;

public class RotateToward : MonoBehaviour
{
    [SerializeField] float amountForward = 200f;
    [SerializeField] Transform ship;
    void Start()
    {
        if(ship != null)
        {
            transform.LookAt(transform.parent.parent.parent.position + transform.parent.parent.parent.forward * amountForward);
        }
    }
}
