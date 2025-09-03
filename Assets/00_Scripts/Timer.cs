using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using System.Collections.Generic;
using DG.Tweening;

public class Timer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI timerFin;
    [SerializeField] private GameObject timerFinObj;
    [SerializeField] private GameObject timerTextObj;

    [SerializeField] private float gameTime;
    [SerializeField] private AudioClip audioClip = null;
    private bool isStopTimer;
    private AudioSource audioSource;


    private void Awake()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }
    void Start()
    {
        isStopTimer = false;


    }
    public void StarSon()
    {
        if (audioClip != null)
        {
           audioSource.PlayOneShot(audioClip);
        }
    }

    void Update()
    {
        if (!isStopTimer)
        {
            gameTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(gameTime / 60F);
            int seconds = Mathf.FloorToInt(gameTime - minutes * 60);
            int milliseconds = Mathf.FloorToInt((gameTime * 100F) % 100);
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
            timerFin.text = gameTime.ToString("0");

        }

        int gameTimeInt = (int)gameTime;
        if (gameTimeInt <= 5 && audioClip != null && !audioSource.isPlaying)
        {
            StarSon();
        }
        

        if (gameTimeInt < 5)
        {
            timerFin.color = Color.red;
            timerFinObj.SetActive(true);
            timerTextObj.SetActive(false);
            timerFinObj.transform.DOScale(1f, 3f);
            if (gameTime <= 0)
            {
                isStopTimer = true;
                timerText.text = "00:00:00";
                timerFin.text = "FIN";
            }
        }
    }
}
