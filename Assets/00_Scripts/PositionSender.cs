using System;
using UnityEngine;

public class PositionSender : MonoBehaviour
{
    PacketBuilder packetBuilder;
    public int id;
    Rigidbody rb;
    private void Start()
    {
        NetworkClient cash = FindFirstObjectByType<NetworkClient>();
        if (cash != null)
        {
            cash.ReicevedId += OnReiceived;
        }
        rb = GetComponent<Rigidbody>();
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
       if(packetBuilder != null)
       {
            ushort isRun = rb.linearVelocity.magnitude > 0f ? (ushort)1 : (ushort)0;
            packetBuilder.SendPacket(new SendPlayerState((byte)id,transform.position,Camera.main.transform.rotation, isRun));
       }
    }
}
