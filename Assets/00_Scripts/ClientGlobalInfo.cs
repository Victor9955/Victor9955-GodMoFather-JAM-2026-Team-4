using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ClientGlobalInfo", menuName = "Scriptable Objects/ClientGlobalInfo")]
public class ClientGlobalInfo : ScriptableObject
{
    public string playerName;
    public int skinId;
    public int matId;

    public List<GameObject> skinsPrefab;
    public List<Material> materials;

    public void SetName(TextMeshProUGUI inputField)
    {
        playerName = inputField.text;
    }
}
