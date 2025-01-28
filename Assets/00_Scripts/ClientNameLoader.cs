using TMPro;
using UnityEngine;

public class ClientNameLoader : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI tmpName;
    [SerializeField] Canvas canvasName;

    public void LoadName(string name)
    {
        canvasName.worldCamera = Camera.main;
        tmpName.text = name;
    }
}
