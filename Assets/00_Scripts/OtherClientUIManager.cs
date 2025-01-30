using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OtherClientUIManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpName;
    [SerializeField] Scrollbar healthBar;
    [SerializeField] Canvas canvas;

    private void Start()
    {
        canvas.worldCamera = Camera.main;
    }

    public void LoadName(string name)
    {
        tmpName.text = name;
    }

    public void UpdateHealth(ushort health, ushort maxHealth)
    {
        healthBar.size = (float)health / (float)maxHealth;
    }
}
