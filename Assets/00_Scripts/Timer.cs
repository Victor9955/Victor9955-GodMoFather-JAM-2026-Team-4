using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using System.Collections.Generic;

public class Timer : MonoBehaviour
{
    [SerializeField]private TextMeshProUGUI timerText;
    [SerializeField]private float gameTime;
    [SerializeField]private AudioClip audioClip = null;
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

    void Update()
    {
        if (!isStopTimer)
        {
            gameTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(gameTime / 60F);
            int seconds = Mathf.FloorToInt(gameTime - minutes * 60);
            int milliseconds = Mathf.FloorToInt((gameTime * 100F) % 100);
            timerText.text = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
            if (gameTime <= 10 && audioClip != null && !audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioClip);
            }
        }
        if (gameTime <= 0)
        {
            isStopTimer = true;
            timerText.text = "00:00:00";
        }
    }
}
