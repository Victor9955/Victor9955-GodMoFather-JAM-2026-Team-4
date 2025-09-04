using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

enum GunState
{
    NewTape,
    ContinueTape
}

public class Gun : MonoBehaviour
{
    [SerializeField] Tape tape;
    [SerializeField] float range;
    [SerializeField] Vector3 pointOffset;
    [SerializeField] LayerMask mask;
    [SerializeField] float coolDown = 10f;
    [SerializeField] Image coolDownImage;
    [SerializeField] Transform transparent;

    Tape current;
    ushort currentId;
    GunState state;
    float lastShot = 0f;

    void Update()
    {
        bool didHit = Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, range, mask);
        if(didHit)
        {
            transparent.gameObject.SetActive(true);
            transparent.position = hit.point;
        }
        else
        {
            transparent.gameObject.SetActive(false);
        }


        if (Input.GetMouseButtonDown(0) && didHit)
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                if(lastShot + coolDown < Time.time) 
                {
                    if (NetworkClient.instance)
                    {
                        NetworkClient.instance.packetBuilder.SendPacket(new Attack());
                    }
                    if (hit.transform.TryGetComponent(out FranckoAnimation animation))
                    {
                        StartCoroutine(Stunt(animation.scotchFace));
                    }
                    coolDownImage.fillAmount = 1f;
                    DOTween.To(() => coolDownImage.fillAmount, x => coolDownImage.fillAmount = x, 0f, coolDown);
                    lastShot = Time.time;
                }
            }
            else
            {
                switch (state)
                {
                    case GunState.NewTape:
                        {
                            current = Instantiate(tape, hit.point + 0.1f * hit.normal, Quaternion.identity);
                            if (NetworkClient.instance != null)
                            {
                                NetworkClient.instance.packetBuilder.SendPacket(new SpawnTape(NetworkClient.instance.tapeNum, hit.point + 0.1f * hit.normal, 0, (ushort)NetworkClient.instance.playerId));
                                currentId = NetworkClient.instance.tapeNum;
                                NetworkClient.instance.tapeNum++;
                            }
                            state = GunState.ContinueTape;
                        }
                        break;
                    case GunState.ContinueTape:
                        {
                            if (hit.collider.gameObject.CompareTag("Tape"))
                            {
                                current.Close(false, NetworkClient.instance.playerId);
                                if (NetworkClient.instance != null)
                                {
                                    NetworkClient.instance.packetBuilder.SendPacket(new SpawnTape(currentId, hit.point + pointOffset, 2, (ushort)NetworkClient.instance.playerId));
                                }
                                current = null;
                                state = GunState.NewTape;
                            }
                            else
                            {
                                if (current.AddTapeAtPosOrIsToLong(hit.point + pointOffset))
                                {
                                    current = null;
                                    state = GunState.NewTape;
                                }
                                else
                                {
                                    if (NetworkClient.instance != null)
                                    {
                                        NetworkClient.instance.packetBuilder.SendPacket(new SpawnTape(currentId, hit.point + pointOffset, 1, (ushort)NetworkClient.instance.playerId));
                                    }
                                }
                            }
                        }
                        break;
                }
            }
        }
    }

    IEnumerator Stunt(GameObject face)
    {
        face.SetActive(true);
        yield return new WaitForSeconds(NetworkClient.instance.stuntTimer);
        face.SetActive(false);
    }
}
