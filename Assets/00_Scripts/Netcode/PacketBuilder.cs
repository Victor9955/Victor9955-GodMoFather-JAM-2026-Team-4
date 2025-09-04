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
    public ushort skin;

    public SendName() { }
    public SendName(string name, ushort skin)
    {
        this.name = name;
        this.skin = skin;
    }

    public Opcode opcode => Opcode.SendName;

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeString(byteArray, name);
        Serialization.SerializeU16(byteArray, skin);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        name = Serialization.DeserializeString(byteArray, ref offset);
        skin = Serialization.DeserializeU16(byteArray, ref offset);
    }
}

public class SendPlayerState : ISerializeInterface
{
    public byte id;
    public Vector3 pos;
    public Quaternion or;
    public ushort isRunning;

    public SendPlayerState() { }
    public SendPlayerState(byte id, Vector3 pos, Quaternion or, ushort isRunning)
    {
        this.id = id;
        this.pos = pos;
        this.or = or;
        this.isRunning = isRunning;
    }

    public Opcode opcode => Opcode.SendPlayerState;

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeU8(byteArray, id);
        Serialization.SerializeVector3(byteArray, pos);
        Serialization.SerializeQuaternion(byteArray, or);
        Serialization.SerializeU16(byteArray, isRunning);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        id = Serialization.DeserializeU8(byteArray, ref offset);
        pos = Serialization.DeserializeVector3(byteArray, ref offset);
        or = Serialization.DeserializeQuaternion(byteArray, ref offset);
        isRunning = Serialization.DeserializeU16(byteArray, ref offset);
    }
}

public class SendPlayerId : ISerializeInterface
{
    public byte id;
    public ushort seed;

    public SendPlayerId() { }
    public SendPlayerId(byte id, ushort seed)
    {
        this.id = id;
        this.seed = seed;
    }

    public Opcode opcode => Opcode.SendPlayerId;

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeU8(byteArray, id);
        Serialization.SerializeU16(byteArray, seed);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        id = Serialization.DeserializeU8(byteArray, ref offset);
        seed = Serialization.DeserializeU16(byteArray, ref offset);
    }
}

public class SendPlayerInit : ISerializeInterface
{
    public byte id;
    public string name;
    public ushort skin;

    public SendPlayerInit() { }
    public SendPlayerInit(byte id, string name, ushort skin)
    {
        this.id = id;
        this.name = name;
        this.skin = skin;
    }

    public Opcode opcode => Opcode.SendPlayerInit;

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeU8(byteArray, id);
        Serialization.SerializeU16(byteArray, skin);
        Serialization.SerializeString(byteArray, name);
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        id = Serialization.DeserializeU8(byteArray, ref offset);
        skin = Serialization.DeserializeU16(byteArray, ref offset);
        name = Serialization.DeserializeString(byteArray, ref offset);
    }
}

public class SpawnTape : ISerializeInterface
{
    public Vector3 pos;
    public ushort tapeId;
    public ushort playerId;
    public byte doModify;
    public SpawnTape() { }

    public SpawnTape(ushort tapeId, Vector3 pos, byte doModify, ushort playerId)
    {
        this.pos = pos;
        this.tapeId = tapeId;
        this.doModify = doModify;
        this.playerId = playerId;
    }

    public Opcode opcode => Opcode.SpawnTape;

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        tapeId = Serialization.DeserializeU16(byteArray, ref offset);
        playerId = Serialization.DeserializeU16(byteArray, ref offset);
        pos = Serialization.DeserializeVector3(byteArray, ref offset);
        doModify = Serialization.DeserializeU8(byteArray, ref offset);
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeU16(byteArray, tapeId);
        Serialization.SerializeU16(byteArray, playerId);
        Serialization.SerializeVector3(byteArray, pos);
        Serialization.SerializeU8(byteArray, doModify);
    }
}

public class Timer : ISerializeInterface
{
    public float timer;

    public Opcode opcode => Opcode.Timer;

    public Timer() { }
    public Timer(float timer)
    {
        this.timer = timer;
    }

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        timer = Serialization.DeserializeF32(byteArray, ref offset);
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeF32(byteArray, timer);
    }
}

public class WinObject : ISerializeInterface
{
    public ushort id;

    public WinObject() { }
    public WinObject(ushort id)
    {
        this.id = id;
    }

    public Opcode opcode => Opcode.Bar;

    public void Deserialize(byte[] byteArray, ref int offset)
    {
        id = Serialization.DeserializeU16(byteArray, ref offset);
    }

    public void Serialize(List<byte> byteArray)
    {
        Serialization.SerializeU16(byteArray, id);
    }
}

public class Attack : ISerializeInterface
{
    public Attack() { }
    public Opcode opcode => Opcode.Attack;

    public void Deserialize(byte[] byteArray, ref int offset)
    {

    }

    public void Serialize(List<byte> byteArray)
    {

    }
}