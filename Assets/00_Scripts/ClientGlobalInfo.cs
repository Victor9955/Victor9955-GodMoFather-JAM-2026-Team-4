using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ClientGlobalInfo", menuName = "Scriptable Objects/ClientGlobalInfo")]
public class ClientGlobalInfo : ScriptableObject
{
    public string playerName;
    public int skinNum;

    public void SetName(TextMeshProUGUI inputField)
    {
        playerName = inputField.text;
    }

    public void SetSkin(TextMeshProUGUI inputField)
    {
        skinNum = int.Parse(inputField.text);
    }
}
