using System.Collections.Generic;
using UnityEngine;

public class ClientSkinLoader : MonoBehaviour
{
    [SerializeField] ClientGlobalInfo clientInfo;
    [SerializeField] Transform ancor;
    [SerializeField] ShootManager shoot;
    public void LoadSkin(int skinId, int matId)
    {
        Debug.Log(skinId + " " + matId);
        GameObject obj = Instantiate(clientInfo.skinsPrefab[skinId], ancor);
        obj.transform.GetComponent<MeshRenderer>().material = clientInfo.materials[matId];
        if(shoot != null)
        {
            shoot.SetupShoot(obj.transform.GetComponent<ShootParticle>());
        }
    }
}
