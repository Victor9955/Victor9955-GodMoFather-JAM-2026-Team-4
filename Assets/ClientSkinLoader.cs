using System.Collections.Generic;
using UnityEngine;

public class ClientSkinLoader : MonoBehaviour
{
    [SerializeField] ClientGlobalInfo clientInfo;
    [SerializeField] Transform ancor;
    public void LoadSkin(int skinId, int matId)
    {
        Debug.Log(skinId + " " + matId);
        GameObject obj = Instantiate(clientInfo.skinsPrefab[skinId], ancor);
        obj.GetComponent<MeshRenderer>().material = clientInfo.materials[matId];
    }
}
