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

    Tape current;
    ushort currentId;
    GunState state;

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, range))
        {
            switch (state)
            {
                case GunState.NewTape:
                    {
                        current = Instantiate(tape,hit.point + pointOffset, Quaternion.identity);
                        if (NetworkClient.instance != null)
                        {
                            NetworkClient.instance.packetBuilder.SendPacket(new SpawnTape(NetworkClient.instance.tapeNum, hit.point + pointOffset, 0));
                            currentId = NetworkClient.instance.tapeNum;
                            NetworkClient.instance.tapeNum++;
                        }
                        state = GunState.ContinueTape;
                    }
                    break;
                case GunState.ContinueTape:
                    {
                        if(hit.collider.gameObject.CompareTag("Tape"))
                        {
                            current.Close(false);
                            if (NetworkClient.instance != null)
                            {
                                NetworkClient.instance.packetBuilder.SendPacket(new SpawnTape(currentId, hit.point + pointOffset, 2));
                            }
                            current = null;
                            state = GunState.NewTape;
                        }
                        else
                        {
                            if(current.AddTapeAtPosOrIsToLong(hit.point + pointOffset))
                            {
                                current = null;
                                state = GunState.NewTape;
                            }
                            else
                            {
                                if(NetworkClient.instance != null)
                                {
                                    NetworkClient.instance.packetBuilder.SendPacket(new SpawnTape(currentId, hit.point + pointOffset, 1));
                                }
                            }
                        }
                    }
                    break;
            }
        }
    }
}
