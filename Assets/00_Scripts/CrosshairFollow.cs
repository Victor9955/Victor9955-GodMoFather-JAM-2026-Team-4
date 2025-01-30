using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    public Transform ship;
    public float amountForward;
    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        if (ship != null)
        {
            rect.position = (Vector2)Camera.main.WorldToScreenPoint(ship.position + ship.forward * amountForward);
        }
    }
}
