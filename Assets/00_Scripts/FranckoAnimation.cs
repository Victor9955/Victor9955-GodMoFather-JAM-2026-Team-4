using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FranckoAnimation : MonoBehaviour
{
    [SerializeField] List<GameObject> run1;
    [SerializeField] List<GameObject> run2;
    [SerializeField] List<GameObject> idle;
    [SerializeField] float interval;
    [SerializeField] int intervalPos;
    public MeshRenderer face;
    Vector3 lastPosition;
    int idleTime = 0;

    float timer = 0f;

    bool run = false;
    private void Update()
    {
        if (Vector3.Distance(transform.position, lastPosition) > 0.01f)
        {
            idleTime = Mathf.Clamp(idleTime, 0, intervalPos);
            lastPosition = transform.position;
            if(idleTime == intervalPos)
            {
                foreach (GameObject g in run1)
                {
                    g.SetActive(false);
                }
                foreach (GameObject g in run2)
                {
                    g.SetActive(false);
                }
                foreach (GameObject g in idle)
                {
                    g.SetActive(true);
                }
            }
        }
        else
        {
            idleTime = 0;
            lastPosition = transform.position;
            if (timer + interval <= Time.time)
            {
                timer += interval;
                run = !run;
                if (run)
                {
                    foreach (GameObject g in run1)
                    {
                        g.SetActive(true);
                    }
                    foreach (GameObject g in run1)
                    {
                        g.SetActive(false);
                    }
                }
                else
                {
                    foreach (GameObject g in run1)
                    {
                        g.SetActive(false);
                    }
                    foreach (GameObject g in run1)
                    {
                        g.SetActive(true);
                    }
                }
                
            }
        }
    }
}
