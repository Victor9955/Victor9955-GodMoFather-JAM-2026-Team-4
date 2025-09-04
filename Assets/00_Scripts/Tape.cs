using System.Collections;
using UnityEngine;

public class Tape : MonoBehaviour
{
    [SerializeField] LineRenderer lineRenderer;
    [SerializeField] float maxLength;
    [SerializeField] float fadeTimer = 0.5f;
    [SerializeField] LayerMask mask;
    bool doFirst = true;
    Coroutine coroutine;
    public bool AddTapeAtPosOrIsToLong(Vector3 pos)
    {
        if(doFirst)
        {
            lineRenderer.SetPosition(0, transform.position);
            doFirst = false;
        }
        if (Vector3.Distance(lineRenderer.GetPosition(lineRenderer.positionCount - 1), pos) > maxLength)
        {
            coroutine = StartCoroutine(Fade(Color.red));
            return true;
        }
        if (lineRenderer.loop == false)
        {
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, pos);
        }
        return false;
    }

    public void Close(bool fromServer, int id)
    {
        lineRenderer.loop = true;
        coroutine = StartCoroutine(Fade(Color.green));
        if(CheckForObject(fromServer, id) && !fromServer)
        {
            if(NetworkClient.instance != null)
            {
                NetworkClient.instance.score += 0.1f;
                NetworkClient.instance.packetBuilder.SendPacket(new Bar((ushort)NetworkClient.instance.playerId, NetworkClient.instance.score));
            }
        }
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
        coroutine = null;
    }
    IEnumerator FadeValid(Color color)
    {
        Color colorBase = lineRenderer.material.color;
        float timer = fadeTimer;
        while (timer > 0f)
        {
            yield return null;
            lineRenderer.material.color = Color.Lerp(color, colorBase, timer / fadeTimer);
            timer -= Time.deltaTime;
        }
        coroutine = null;
    }

    bool CheckForObject(bool fromServer, int id)
    {
        for (int x = 0; x < lineRenderer.positionCount; x++)
        {
            for(int y = 0; y < lineRenderer.positionCount; y++)
            {
                if (Physics.Raycast(lineRenderer.GetPosition(x),lineRenderer.GetPosition(y) - lineRenderer.GetPosition(x),out RaycastHit hit,Vector3.Distance(lineRenderer.GetPosition(x), lineRenderer.GetPosition(y)), mask))
                {
                    if(coroutine != null)
                    {
                        StopCoroutine(coroutine);
                    }
                    hit.transform.gameObject.GetComponent<Collider>().enabled = false;
                    hit.transform.gameObject.GetComponentInChildren<Collider>().enabled = false;
                    return true;
                }
            }
        }
        return false;
    }
}
