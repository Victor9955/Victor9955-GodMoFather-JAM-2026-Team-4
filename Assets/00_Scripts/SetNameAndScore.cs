using TMPro;
using UnityEngine;

public class SetNameAndScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerName; 
    [SerializeField] TextMeshProUGUI score;

    public void SetNameAndScoreFunc(string m_playerName, ushort m_score)
    {
        score.text = m_score.ToString();
        playerName.text = m_playerName;
    }
}
