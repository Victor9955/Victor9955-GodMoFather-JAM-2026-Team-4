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

    public void SendPacket<T>(T packet) where T : class , ISerializeInterface
    {
        List<byte> byteArray = new List<byte>();

        Serialization.SerializeU8(byteArray, (byte)packet.opcode);
        packet.Serialize(byteArray);

        Packet enetPacket = default(Packet);

        enetPacket.Create(byteArray.ToArray());
        serverPeer.Send(channelID, ref enetPacket);
    }
}

public class ClientInitData : ISerializeInterface
{
    public Opcode opcode => Opcode.OnClientConnect;

    public string playerName;
    public byte skinId;
    public byte matId;

    public ClientInitData() { }
    public ClientInitData(string m_playerName, int m_skinNumber, int m_matNumber)
    {
        playerName = m_playerName;
        skinId = (byte)m_skinNumber;
        matId = (byte)m_matNumber;
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeU8(byteArray, skinId);
        Serialization.SerializeU8(byteArray, matId);
        Serialization.SerializeString(byteArray, playerName);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        skinId = Serialization.DeserializeU8(byteArray,ref offset);
        matId = Serialization.DeserializeU8(byteArray,ref offset);
        playerName = Serialization.DeserializeString(byteArray, ref offset);
    }
}

public class PlayerInputData : ISerializeInterface
{
    public Opcode opcode => Opcode.PlayerInputsData;

    public float currentTime;
    public Vector2 moveInput;
    public Quaternion rotation;

    public PlayerInputData() { }
    public PlayerInputData(Vector2 moveInput, Quaternion rotation)
    {
        currentTime = Time.time;
        this.moveInput = moveInput;
        this.rotation = rotation;
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeF32(byteArray, currentTime);
        Serialization.SerializeVector2(byteArray, moveInput);
        Serialization.SerializeQuaternion(byteArray, rotation);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        currentTime = Serialization.DeserializeF32(byteArray,ref offset);
        moveInput = Serialization.DeserializeVector2(byteArray,ref offset);
        rotation = Serialization.DeserializeQuaternion(byteArray,ref offset);
    }
}

public class ConnectServerInitData : ISerializeInterface
{
    public Opcode opcode => Opcode.OnClientConnectResponse;

    public byte playerNum;
    public Vector3 playerStartPos;

    public ConnectServerInitData() { }
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

public class InitData : ISerializeInterface
{
    public Opcode opcode => Opcode.OnOtherClientConnect;
    public ClientInitData clientInitData = new ClientInitData();
    public ConnectServerInitData serverClientInitData = new ConnectServerInitData();

    public InitData() { }
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

public class ClientSendShoot : ISerializeInterface
{
    public Opcode opcode => Opcode.ClientShoot;

    public Vector3 position;
    public Vector3 direction;

    public ClientSendShoot() { }

    public ClientSendShoot(Vector3 m_position, Vector3 m_direction)
    {
        position = m_position;
        direction = m_direction;
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeVector3(byteArray, position);
        Serialization.SerializeVector3(byteArray, position);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        position = Serialization.DeserializeVector3(byteArray, ref offset);
        direction = Serialization.DeserializeVector3(byteArray, ref offset);
    }
}