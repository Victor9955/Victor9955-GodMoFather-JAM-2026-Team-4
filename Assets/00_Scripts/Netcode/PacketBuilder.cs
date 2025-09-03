using ENet6;
using System.Collections.Generic;
using UnityEngine;

public class PacketBuilder
{
    public Peer peer;
    byte channelID;

    public PacketBuilder(Peer m_serverPeer, byte m_channelID)
    {
        peer = m_serverPeer;
        channelID = m_channelID;
    }

    public void SendPacket<T>(T packet) where T : class , ISerializeInterface
    {
        List<byte> byteArray = new List<byte>();

        Serialization.SerializeU8(byteArray, (byte)packet.opcode);
        packet.Serialize(byteArray);

        Packet enetPacket = default(Packet);

        enetPacket.Create(byteArray.ToArray());
        peer.Send(channelID, ref enetPacket);
    }
}

public class SendName : ISerializeInterface
{
    public string name;

    public SendName() { }
    public SendName(string name)
    {
        this.name = name;
    }

    public Opcode opcode => Opcode.SendName;

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeString(byteArray, name);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        name = Serialization.DeserializeString(byteArray, ref offset);
    }
}

public class SendPlayerState : ISerializeInterface
{
    public byte id;
    public Vector3 pos;
    public Quaternion or;

    public SendPlayerState() { }
    public SendPlayerState(byte id, Vector3 pos, Quaternion or)
    {
        this.id = id;
        this.pos = pos;
        this.or = or;
    }

    public Opcode opcode => Opcode.SendPlayerState;

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeU8(byteArray, id);
        Serialization.SerializeVector3(byteArray, pos);
        Serialization.SerializeQuaternion(byteArray, or);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        id = Serialization.DeserializeU8(byteArray, ref offset);
        pos = Serialization.DeserializeVector3(byteArray, ref offset);
        or = Serialization.DeserializeQuaternion(byteArray, ref offset);
    }
}

public class SendPlayerId : ISerializeInterface
{
    public byte id;

    public SendPlayerId() { }
    public SendPlayerId(byte id)
    {
        this.id = id;
    }

    public Opcode opcode => Opcode.SendPlayerId;

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeU8(byteArray, id);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        id = Serialization.DeserializeU8(byteArray, ref offset);
    }
}

public class SendPlayerInit : ISerializeInterface
{
    public byte id;
    public string name;

    public SendPlayerInit() { }
    public SendPlayerInit(byte id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public Opcode opcode => Opcode.SendPlayerInit;

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeU8(byteArray, id);
        Serialization.SerializeString(byteArray, name);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        id = Serialization.DeserializeU8(byteArray, ref offset);
        name = Serialization.DeserializeString(byteArray, ref offset);
    }
}

public class SpawnTape : ISerializeInterface
{
    Vector3 pos;

    public SpawnTape() { }

    public SpawnTape(Vector3 pos) 
    {
        this.pos = pos; 
    }

    public Opcode opcode => Opcode.SpawnTape;

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        pos = Serialization.DeserializeVector3(byteArray, ref offset);
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeVector3(byteArray, pos);
    }
}