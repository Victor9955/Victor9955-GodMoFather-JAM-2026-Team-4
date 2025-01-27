using UnityEngine;

public class CrosshairFollow : MonoBehaviour
{
    [SerializeField] Transform ship;
    [SerializeField] float amountForward;
    RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        rect.position = (Vector2)Camera.main.WorldToScreenPoint(ship.position + ship.forward * amountForward);
    }
}
