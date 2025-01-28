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

struct ClientConnect : ISerializeInterface
{
    public Opcode opcode { get => Opcode.OnClientConnect; }

    public string playerName;
    public byte skinNumber;
    public ClientConnect(string m_playerName, int m_skinNumber)
    {
        playerName = m_playerName;
        skinNumber = (byte)m_skinNumber;
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeString(byteArray, playerName);
        Serialization.SerializeU8(byteArray, skinNumber);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        playerName = Serialization.DeserializeString(byteArray,ref offset);
        skinNumber = Serialization.DeserializeU8(byteArray,ref offset);
    }
}

struct ServerConnectResponse : ISerializeInterface
{
    public Opcode opcode { get => Opcode.OnClientConnectResponse; }

    public byte playerNum;
    public Vector3 playerStartPos;
    public ServerConnectResponse(int m_playerNum, Vector3 m_playerStartPos)
    {
        playerNum = (byte)m_playerNum;
        playerStartPos = m_playerStartPos;
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeU8(byteArray, playerNum);
        Serialization.SerializeVector3(byteArray, playerStartPos);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        playerNum = Serialization.DeserializeU8(byteArray, ref offset);
        playerStartPos = Serialization.DeserializeVector3(byteArray, ref offset);
    }
}
