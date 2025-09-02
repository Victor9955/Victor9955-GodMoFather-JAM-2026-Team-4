using UnityEngine;

public class TestTape : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, 100.0f))
        {
            Debug.Log(hit.point);
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point + (Vector3.up * 0.1f));
        }
    }
}
