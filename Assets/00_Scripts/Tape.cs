using System.Collections;
using UnityEngine;

public class Tape : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float maxLength;
    [SerializeField] float fadeTimer = 0.5f;
    [SerializeField] LayerMask mask;
    bool doFirst = true;
    public bool AddTapeAtPosOrIsToLong(Vector3 pos)
    {
        if(doFirst)
        {
            lineRenderer.SetPosition(0, transform.position);
            doFirst = false;
        }
        if (Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), pos) > maxLength)
        {
            StartCoroutine(Fade(Color.red));
            return true;
        }
        if (lineRenderer.loop == false)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
        }
        return false;
    }

    public void Close()
    {
        lineRenderer.loop = true;
        StartCoroutine(Fade(Color.green));
        CheckForObject();
    }

    IEnumerator Fade(Color color)
    {
        Color colorBase = lineRenderer.material.color;
        lineRenderer.material.color = color;
        float timer = fadeTimer;
        while (timer > 0f)
        {
            yield return null;
            lineRenderer.material.color = Color.Lerp(colorBase, color, timer / fadeTimer);
            timer -= Time.deltaTime;
        }
    }

    void CheckForObject()
    {
        bool touched = false;
        for (int x = 0; x < lineRenderer.positionCount; x++)
        {
            for(int y = 0; y < lineRenderer.positionCount; y++)
            {
                if (Physics.Raycast(lineRenderer.GetPosition(x),lineRenderer.GetPosition(y) - lineRenderer.GetPosition(x),out RaycastHit hit,Vector3.Distance(lineRenderer.GetPosition(x), lineRenderer.GetPosition(y)), mask))
                {
                    touched = true;
                }
            }
        }
        Debug.Log("Did touched :" + touched);
    }
}
