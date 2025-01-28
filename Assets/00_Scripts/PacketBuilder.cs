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

struct ClientInitData : ISerializeInterface
{
    public Opcode opcode => Opcode.OnClientConnect;

    public string playerName;
    public byte skinId;
    public byte matId;
    public ClientInitData(string m_playerName, int m_skinNumber, int m_matNumber)
    {
        playerName = m_playerName;
        skinId = (byte)m_skinNumber;
        matId = (byte)m_matNumber;
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeString(byteArray, playerName);
        Serialization.SerializeU8(byteArray, skinId);
        Serialization.SerializeU8(byteArray, matId);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        playerName = Serialization.DeserializeString(byteArray,ref offset);
        skinId = Serialization.DeserializeU8(byteArray,ref offset);
        matId = Serialization.DeserializeU8(byteArray,ref offset);
    }
}

struct ConnectServerInitData : ISerializeInterface
{
    public Opcode opcode => Opcode.OnClientConnectResponse;

    public byte playerNum;
    public Vector3 playerStartPos;
    public ConnectServerInitData(int m_playerNum, Vector3 m_playerStartPos)
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

struct InitData : ISerializeInterface
{
    public Opcode opcode => Opcode.OnOtherClientConnect;
    public ClientInitData clientInitData;
    public ConnectServerInitData serverClientInitData;

    public InitData(ClientInitData m_clientInitData, ConnectServerInitData m_serverClientInitData)
    {
        clientInitData = m_clientInitData;
        serverClientInitData = m_serverClientInitData;
    }

    public void Serialize(List<byte> byteArray)
    {
        clientInitData.Serialize(byteArray);
        serverClientInitData.Serialize(byteArray);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        clientInitData.Deserialize(byteArray, ref offset);
        serverClientInitData.Deserialize(byteArray, ref offset);
    }

}