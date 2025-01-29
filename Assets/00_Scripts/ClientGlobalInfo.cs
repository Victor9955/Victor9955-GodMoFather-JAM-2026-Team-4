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

    public void SetName(TMP_InputField inputField)
    {
        playerName = inputField.text;
    }

    public void SetShootParticle()
    {
        foreach (GameObject go in skinsPrefab)
        {
            ShootParticle[] mat = go.GetComponents<ShootParticle>();
            foreach (ShootParticle c in mat)
            {
                DestroyImmediate(c,true);
            }
        }
        foreach (GameObject go in skinsPrefab)
        {
            go.AddComponent<ShootParticle>();
            ParticleSystem[] c = go.GetComponentsInChildren<ParticleSystem>();
            foreach (var item in c)
            {
                item.playOnAwake = false;
            }
        }
    }
}
