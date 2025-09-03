using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class FranckoAnimation : MonoBehaviour
{
    [SerializeField] List<GameObject> anim;
    [SerializeField] float interval;

    float timer = 0f;
    private void Update()
    {
        if(timer + interval <= Time.time)
        {
            timer += interval;
            foreach(GameObject g in anim)
            {
                g.SetActive(!g.activeSelf);
            }
        }
    }
}
