using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;
using System.Collections.Generic;

public class UpdateScore1 : ScoreManager
{

    
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI nameText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateScore("dorian", 100);
        scoreText.text = "Score: " + playerScores["dorian"];
        nameText.text = "Name: " + name;
    }

}
