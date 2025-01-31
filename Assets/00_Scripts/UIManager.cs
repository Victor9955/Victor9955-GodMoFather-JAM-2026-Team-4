using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Scrollbar shootScrollbar;
    public CrosshairFollow crosshairFollow;
    public Scrollbar lifeBar;
    public GameObject prefab;
    public Transform leaderboardAncor;
    public GameObject leaderboardUI;
    public TextMeshProUGUI deadCountText;
    public float respawnTime = 5f;

    Dictionary<string,SetNameAndScore> scores = new();

    Coroutine coroutine;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void UpdateLeaderBoard(string m_name, ushort m_score)
    {
        if(scores.TryGetValue(m_name, out SetNameAndScore scoreGameobject))
        {
            scoreGameobject.SetNameAndScoreFunc(m_name, m_score);
        }
        else
        {
            GameObject newObject = Instantiate(prefab,leaderboardAncor);
            scores.Add(m_name, newObject.GetComponent<SetNameAndScore>());
            scores[m_name].SetNameAndScoreFunc(m_name, m_score);
        }
    }

    public void ShowDeadUI()
    {
        leaderboardUI.SetActive(true);
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(CountDown());
    }

    public void HideDeadUI()
    {
        leaderboardUI.SetActive(false);
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }

    IEnumerator CountDown()
    {
        float timer = respawnTime;
        while(timer > 0f)
        {
            deadCountText.text = "Respawn in " + (int)timer + " sec..";
            timer -= Time.deltaTime;
            yield return null;
        }
        deadCountText.text = "Respawn in " + (int)timer + " sec..";
        coroutine = null;
    }
}
