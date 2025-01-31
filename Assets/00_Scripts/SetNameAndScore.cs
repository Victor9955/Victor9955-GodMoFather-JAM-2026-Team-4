using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetNameAndScore : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI playerName; 
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] Image bg;
    public Color color = Color.white;
    public ushort score;

    private void OnEnable()
    {
        scoreText.text = score.ToString();
        bg.color = color;
    }

    public void SetNameAndScoreFunc(string m_playerName, ushort m_score)
    {
        score = m_score;
        scoreText.text = m_score.ToString();
        playerName.text = m_playerName;
    }

    public void SetColor(Color m_color)
    {
        color = m_color;
    }
}
