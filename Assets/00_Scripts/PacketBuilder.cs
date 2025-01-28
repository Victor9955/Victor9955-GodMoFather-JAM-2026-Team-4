using ENet6;
using System.Collections.Generic;

public class PacketBuilder
{
    Peer serverPeer;
    byte channelID;

    public PacketBuilder(Peer m_serverPeer, byte m_channelID)
    {
        serverPeer = m_serverPeer;
        channelID = m_channelID;
    }

    public void SendPacket<T>() where T : struct , ISerializeInterface
    {
        List<byte> byteArray = new List<byte>();

        T packet = new T();

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

    public void Serialize(List<byte> byteArray)
    {

    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {

    }

}