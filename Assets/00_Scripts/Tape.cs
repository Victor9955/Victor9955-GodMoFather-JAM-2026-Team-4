using System.Collections;
using UnityEngine;

public class Tape : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float maxLength;
    [SerializeField] float fadeTimer = 0.5f;
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
}
