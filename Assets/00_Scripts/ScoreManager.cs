using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{

    public Dictionary<string, int> playerScores = new Dictionary<string, int>();

    public void UpdateScore(string name, int score)
    {
        if (playerScores.ContainsKey(name))
        {
            playerScores[name] += score;
        }
        else
        {
            playerScores[name] = score;
        }


    }
}

