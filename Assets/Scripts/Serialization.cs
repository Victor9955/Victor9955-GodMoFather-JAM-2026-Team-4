using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using UnityEngine;

public static class Serialization
{
    public static void SerializeColor(List<byte> byteArray, Color value)
    {
        SerializeU8(byteArray, (byte)(value.r * 255f));
        SerializeU8(byteArray, (byte)(value.g * 255f));
        SerializeU8(byteArray, (byte)(value.b * 255f));
        SerializeU8(byteArray, (byte)(value.a * 255f));
    }

    public static Color DeserializeColor(byte[] byteArray, ref int offset)
    {
        byte r = DeserializeU8(byteArray, ref offset);
        byte g = DeserializeU8(byteArray, ref offset);
        byte b = DeserializeU8(byteArray, ref offset);

        return new Color(r, g, b);
    }

    public static void SerializeVector3(List<byte> byteArray, Vector3 value)
    {
        SerializeF32(byteArray, value.x);
        SerializeF32(byteArray, value.y);
        SerializeF32(byteArray, value.z);
    }

    public static Vector3 DeserializeVector3(byte[] byteArray, ref int offset)
    {
        Vector3 result;
        result.x = DeserializeF32(byteArray, ref offset);
        result.y = DeserializeF32(byteArray, ref offset);
        result.z = DeserializeF32(byteArray, ref offset);
        return result;
    }

    public static void SerializeVector2(List<byte> byteArray, Vector2 value)
    {
        SerializeF32(byteArray, value.x);
        SerializeF32(byteArray, value.y);
    }

    public static Vector2 DeserializeVector2(byte[] byteArray, ref int offset)
    {
        Vector2 result;
        result.x = DeserializeF32(byteArray, ref offset);
        result.y = DeserializeF32(byteArray, ref offset);
        return result;
    }

    public static void SerializeQuaternion(List<byte> byteArray, Quaternion value)
    {
        SerializeF32(byteArray, value.x);
        SerializeF32(byteArray, value.y);
        SerializeF32(byteArray, value.z);
        SerializeF32(byteArray, value.w);
    }

    public static Quaternion DeserializeQuaternion(byte[] byteArray, ref int offset)
    {
        Quaternion result;
        result.x = DeserializeF32(byteArray, ref offset);
        result.y = DeserializeF32(byteArray, ref offset);
        result.z = DeserializeF32(byteArray, ref offset);
        result.w = DeserializeF32(byteArray, ref offset);
        return result;
    }

    public static void SerializeF32(List<byte> byteArray, float value)
    {
        uint intRepresentation = BitConverter.ToUInt32(BitConverter.GetBytes(value), 0);
        SerializeU32(byteArray, intRepresentation);
    }

    public static float DeserializeF32(byte[] byteArray, ref int offset)
    {
        uint intRepresentation = DeserializeU32(byteArray, ref offset);
        return BitConverter.ToSingle(BitConverter.GetBytes(intRepresentation), 0);
    }

    public static void SerializeI8(List<byte> byteArray, sbyte value)
    {
        SerializeU8(byteArray, (byte)value);
    }

    public static sbyte DeserializeI8(byte[] byteArray, ref int offset)
    {
        return (sbyte)DeserializeU8(byteArray, ref offset);
    }

    public static void SerializeI16(List<byte> byteArray, short value)
    {
        SerializeU16(byteArray, (ushort)value);
    }

    public static short DeserializeI16(byte[] byteArray, ref int offset)
    {
        short value = BitConverter.ToInt16(byteArray, offset);
        offset += sizeof(short);
        return IPAddress.NetworkToHostOrder(value);
    }

    public static void SerializeI32(List<byte> byteArray, int value)
    {
        SerializeU32(byteArray, (uint)
            value);
    }

    public static int DeserializeI32(byte[] byteArray, ref int offset)
    {
        int value = BitConverter.ToInt32(byteArray, offset);
        offset += sizeof(int);
        return IPAddress.NetworkToHostOrder(value);
    }

    public static void SerializeU8(List<byte> byteArray, byte value)
    {
        byteArray.Add(value);
    }

    public static byte DeserializeU8(byte[] byteArray, ref int offset)
    {
        return byteArray[offset++];
    }

    public static void SerializeU16(List<byte> byteArray, ushort value)
    {
        value = (ushort)IPAddress.HostToNetworkOrder((short)value);
        byteArray.AddRange(BitConverter.GetBytes(value));
    }

    public static ushort DeserializeU16(byte[] byteArray, ref int offset)
    {
        ushort value = BitConverter.ToUInt16(byteArray, offset);
        offset += sizeof(ushort);
        return (ushort)IPAddress.NetworkToHostOrder((short)value);
    }

    public static void SerializeU32(List<byte> byteArray, uint value)
    {
        value = (uint)IPAddress.HostToNetworkOrder((int)value);
        byteArray.AddRange(BitConverter.GetBytes(value));
    }

    public static uint DeserializeU32(byte[] byteArray, ref int offset)
    {
        uint value = BitConverter.ToUInt32(byteArray, offset);
        offset += sizeof(uint);
        return (uint)IPAddress.NetworkToHostOrder((int)value);
    }

    public static void SerializeString(List<byte> byteArray, string value)
    {
        SerializeU32(byteArray, (uint)value.Length);
        byteArray.AddRange(Encoding.UTF8.GetBytes(value));
    }

    public static string DeserializeString(byte[] byteArray, ref int offset)
    {
        uint length = DeserializeU32(byteArray, ref offset);
        string value = Encoding.UTF8.GetString(byteArray, offset, (int)length);
        offset += (int)length;
        return value;
    }
}
