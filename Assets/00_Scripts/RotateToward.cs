using UnityEngine;

public class RotateToward : MonoBehaviour
{
    [SerializeField] float amountForward = 200f;
    public Transform forward;
    void Start()
    {
        
    }

    private void Update()
    {
        if(forward != null)
        {
            transform.LookAt(forward.position + forward.forward * amountForward);
        }
    }
}
