using UnityEngine;

public class RotateToward : MonoBehaviour
{
    [SerializeField] float amountForward = 200f;
    [SerializeField] Transform ship;
    void Start()
    {
        transform.LookAt(ship.parent.parent.position + ship.parent.parent.forward * amountForward);
    }
}
