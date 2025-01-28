using ENet6;
using System.Collections.Generic;
using UnityEngine;

public class PacketBuilder
{
    Peer serverPeer;
    byte channelID;

    public PacketBuilder(Peer m_serverPeer, byte m_channelID)
    {
        serverPeer = m_serverPeer;
        channelID = m_channelID;
    }

    public void SendPacket<T>(T packet) where T : struct , ISerializeInterface
    {
        List<byte> byteArray = new List<byte>();

        Serialization.SerializeU8(byteArray, (byte)packet.opcode);
        packet.Serialize(byteArray);

        Packet enetPacket = default(Packet);

        enetPacket.Create(byteArray.ToArray());
        serverPeer.Send(channelID, ref enetPacket);
    }
}

struct Position : ISerializeInterface
{
    public Opcode opcode { get => Opcode.Test; }

    public Vector3 position;

    public Position(Vector3 m_position)
    {
        position = m_position;
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeVector3(byteArray, position);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        Debug.Log(Serialization.DeserializeVector3(byteArray,ref offset));
    }

}