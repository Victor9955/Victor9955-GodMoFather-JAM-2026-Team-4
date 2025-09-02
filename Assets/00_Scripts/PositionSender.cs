using System;
using UnityEngine;

public class PositionSender : MonoBehaviour
{
    PacketBuilder packetBuilder;
    public int id;

    private void Start()
    {
        NetworkClient cash = FindFirstObjectByType<NetworkClient>();
        if (cash != null)
        {
            cash.ReicevedId += OnReiceived;
        }
    }

    private void OnReiceived()
    {
        NetworkClient cash = FindFirstObjectByType<NetworkClient>();
        if (cash != null)
        {
            packetBuilder = cash.packetBuilder;
            id = cash.playerId;
        }
    }

    private void Update()
    {
       if(packetBuilder != null && !Input.GetKey(KeyCode.Space))
       {
            packetBuilder.SendPacket(new SendPlayerState((byte)id,transform.position,Camera.main.transform.rotation));
       }
    }
}
