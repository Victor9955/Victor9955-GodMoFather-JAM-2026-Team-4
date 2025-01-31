using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public Scrollbar shootScrollbar;
    public CrosshairFollow crosshairFollow;
    public Scrollbar lifeBar;
    public GameObject prefab;
    public Transform ancor;


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void LeaderBoard(List<(string, ushort)> leaderboard)
    {
        for (int i = 0; i < ancor.childCount; i++)
        {
            Destroy(ancor.GetChild(i).gameObject);
        }

        foreach (var ch in leaderboard)
        {
            GameObject playerScore = Instantiate(prefab,ancor);
            playerScore.GetComponent<SetNameAndScore>().SetNameAndScoreFunc(ch.Item1, ch.Item2);
        }
    }
}
