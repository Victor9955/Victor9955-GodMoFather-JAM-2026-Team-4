using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FranckoAnimation : MonoBehaviour
{
    [SerializeField] List<GameObject> run1;
    [SerializeField] List<GameObject> run2;
    [SerializeField] List<GameObject> idle;
    [SerializeField] float interval;
    public MeshRenderer face;
    public int isRuning = 0;
    float timer = 0f;

    bool run = false;
    private void Update()
    {
        if(isRuning == 0)
        {
            foreach (GameObject g in idle)
            {
                g.SetActive(false);
            }
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
                    foreach (GameObject g in run2)
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
                    foreach (GameObject g in run2)
                    {
                        g.SetActive(true);
                    }
                }

            }
        }
        else
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
}
