using System.Collections.Generic;
using UnityEngine;

public class ClientSkinLoader : MonoBehaviour
{
    [SerializeField] List<GameObject> skins;
    public void LoadSkin(int skinId)
    {
        foreach (GameObject t in skins)
        {
            t.SetActive(false);
        }
        skins[skinId].SetActive(true);
    }
}
