using UnityEngine;

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

    Tape current;
    ushort currentId;
    GunState state;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, range, mask))
        {
            if(hit.collider.gameObject.CompareTag("Player"))
            {
                if(NetworkClient.instance)
                {
                    NetworkClient.instance.packetBuilder.SendPacket(new Attack());
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
}
