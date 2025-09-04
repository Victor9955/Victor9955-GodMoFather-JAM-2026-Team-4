using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ClientGlobalInfo", menuName = "Scriptable Objects/ClientGlobalInfo")]
public class ClientGlobalInfo : ScriptableObject
{
    public string playerName;
    public string ip = "localhost";
    public int skin;
    public float playerOneScore;
    public float playerTwoScore;
    public List<Texture> skins;
    public bool movementNormal = true;

    public void SetName(TMP_InputField inputField)
    {
        playerName = inputField.text;
    }

    public void SetIp(TMP_InputField inputField)
    {
        ip = inputField.text;
    }

    public void SetSkin(int skin)
    {
        this.skin = skin;
    }
}
