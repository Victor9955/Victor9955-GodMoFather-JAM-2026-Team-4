using System.Collections.Generic;
using UnityEngine;

public class SkinSelector : MonoBehaviour
{
    [SerializeField] ClientGlobalInfo clientInfo;
    [SerializeField] Transform ancor;
    [SerializeField] float rotateSpeed = 20;
    List<Transform> skinsTransform = new();
    Vector3 startRot;

    private void OnEnable()
    {
        startRot = ancor.localEulerAngles;
        if (skinsTransform.Count < 0)
        {
            foreach(Transform t in skinsTransform)
            {
                Destroy(t.gameObject);
            }
            skinsTransform.Clear();
        }

        for (int i = 0; i < clientInfo.skinsPrefab.Count; i++) 
        {
            GameObject obj = Instantiate(clientInfo.skinsPrefab[i], ancor);
            obj.transform.localPosition = Vector3.zero;
            if(i == clientInfo.skinId)
            {
                obj.SetActive(true);
            }
            else
            {
                obj.SetActive(false);
            }
            skinsTransform.Add(obj.transform);
        }
        UpdateSkin();
    }

    private void Update()
    {
        ancor.localEulerAngles += Vector3.up * Time.deltaTime * rotateSpeed;
    }

    public void Right()
    {
        HideCurrent();
        clientInfo.skinId++;
        clientInfo.skinId %= clientInfo.skinsPrefab.Count - 1;
        UpdateSkin();
    }

    public void Left()
    {
        HideCurrent();
        clientInfo.skinId--;
        if(clientInfo.skinId < 0) clientInfo.skinId = clientInfo.skinsPrefab.Count - 1;
        UpdateSkin();
    }

    public void RightMat()
    {
        clientInfo.matId++;
        clientInfo.matId %= clientInfo.materials.Count - 1;
        UpdateSkin();
    }

    public void LeftMat()
    {
        clientInfo.matId--;
        if (clientInfo.matId < 0) clientInfo.matId = clientInfo.materials.Count - 1;
        UpdateSkin();
    }

    void HideCurrent()
    {
        ancor.localEulerAngles = startRot;
        skinsTransform[clientInfo.skinId].gameObject.SetActive(false);
    }

    void UpdateSkin()
    {
        skinsTransform[clientInfo.skinId].gameObject.SetActive(true);
        skinsTransform[clientInfo.skinId].gameObject.GetComponent<MeshRenderer>().material = clientInfo.materials[clientInfo.matId];
    }
}
