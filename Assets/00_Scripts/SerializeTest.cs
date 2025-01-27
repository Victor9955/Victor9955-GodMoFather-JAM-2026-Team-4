using System.Collections.Generic;
using UnityEngine;

public class SerializeTest : MonoBehaviour
{
    void Start()
    {
        RunTests();
    }

    void RunTests()
    {
        Debug.Log("Starting Serialization Tests...");

        // Test Color Serialization
        List<byte> byteArray = new List<byte>();
        Color testColor = new Color(0.5f, 0.25f, 0.75f, 1.0f);
        Serialization.SerializeColor(byteArray, testColor);

        int offset = 0;
        Color deserializedColor = Serialization.DeserializeColor(byteArray.ToArray(), ref offset);
        Debug.Log($"Original Color: {testColor}, Deserialized Color: {deserializedColor}");

        // Test Float Serialization
        byteArray.Clear();
        float testFloat = 123.456f;
        Serialization.SerializeF32(byteArray, testFloat);

        offset = 0;
        float deserializedFloat = Serialization.DeserializeF32(byteArray.ToArray(), ref offset);
        Debug.Log($"Original Float: {testFloat}, Deserialized Float: {deserializedFloat}");

        // Test String Serialization
        byteArray.Clear();
        string testString = "Hello, World!";
        Serialization.SerializeString(byteArray, testString);

        offset = 0;
        string deserializedString = Serialization.DeserializeString(byteArray.ToArray(), ref offset);
        Debug.Log($"Original String: {testString}, Deserialized String: {deserializedString}");

        // Test Integer Serialization (int32)
        byteArray.Clear();
        int testInt32 = 123456;
        Serialization.SerializeI32(byteArray, testInt32);

        offset = 0;
        int deserializedInt32 = Serialization.DeserializeI32(byteArray.ToArray(), ref offset);
        Debug.Log($"Original Int32: {testInt32}, Deserialized Int32: {deserializedInt32}");

        // Test Unsigned Byte Serialization (uint8)
        byteArray.Clear();
        byte testUInt8 = 255;
        Serialization.SerializeU8(byteArray, testUInt8);

        offset = 0;
        byte deserializedUInt8 = Serialization.DeserializeU8(byteArray.ToArray(), ref offset);
        Debug.Log($"Original UInt8: {testUInt8}, Deserialized UInt8: {deserializedUInt8}");

        // Test Signed Byte Serialization (int8)
        byteArray.Clear();
        sbyte testInt8 = -128;
        Serialization.SerializeI8(byteArray, testInt8);

        offset = 0;
        sbyte deserializedInt8 = Serialization.DeserializeI8(byteArray.ToArray(), ref offset);
        Debug.Log($"Original Int8: {testInt8}, Deserialized Int8: {deserializedInt8}");

        // Test Unsigned Short Serialization (uint16)
        byteArray.Clear();
        ushort testUInt16 = 65535;
        Serialization.SerializeU16(byteArray, testUInt16);

        offset = 0;
        ushort deserializedUInt16 = Serialization.DeserializeU16(byteArray.ToArray(), ref offset);
        Debug.Log($"Original UInt16: {testUInt16}, Deserialized UInt16: {deserializedUInt16}");

        // Test Signed Short Serialization (int16)
        byteArray.Clear();
        short testInt16 = -32768;
        Serialization.SerializeI16(byteArray, testInt16);

        offset = 0;
        short deserializedInt16 = Serialization.DeserializeI16(byteArray.ToArray(), ref offset);
        Debug.Log($"Original Int16: {testInt16}, Deserialized Int16: {deserializedInt16}");

        // Test Unsigned Integer Serialization (uint32)
        byteArray.Clear();
        uint testUInt32 = 4294967295;
        Serialization.SerializeU32(byteArray, testUInt32);

        offset = 0;
        uint deserializedUInt32 = Serialization.DeserializeU32(byteArray.ToArray(), ref offset);
        Debug.Log($"Original UInt32: {testUInt32}, Deserialized UInt32: {deserializedUInt32}");

        // Test Combined Serialization and Deserialization
        byteArray.Clear();
        SerializeMultiple(byteArray);

        offset = 0;
        DeserializeMultiple(byteArray.ToArray(), ref offset);

        Debug.Log("Serialization Tests Completed.");
    }

    void SerializeMultiple(List<byte> byteArray)
    {
        Debug.Log("Serializing multiple types...");
        Serialization.SerializeU8(byteArray, 128);
        Serialization.SerializeI8(byteArray, -64);
        Serialization.SerializeU16(byteArray, 32000);
        Serialization.SerializeI16(byteArray, -16000);
        Serialization.SerializeU32(byteArray, 1234567890);
        Serialization.SerializeI32(byteArray, -123456789);
        Serialization.SerializeF32(byteArray, 3.140001f);
        Serialization.SerializeString(byteArray, "TestString");
        Serialization.SerializeColor(byteArray, new Color(0.1f, 0.2f, 0.3f, 1.0f));
    }

    void DeserializeMultiple(byte[] byteArray, ref int offset)
    {
        Debug.Log("Deserializing multiple types...");
        byte u8 = Serialization.DeserializeU8(byteArray, ref offset);
        sbyte i8 = Serialization.DeserializeI8(byteArray, ref offset);
        ushort u16 = Serialization.DeserializeU16(byteArray, ref offset);
        short i16 = Serialization.DeserializeI16(byteArray, ref offset);
        uint u32 = Serialization.DeserializeU32(byteArray, ref offset);
        int i32 = Serialization.DeserializeI32(byteArray, ref offset);
        float f32 = Serialization.DeserializeF32(byteArray, ref offset);
        string str = Serialization.DeserializeString(byteArray, ref offset);
        Color color = Serialization.DeserializeColor(byteArray, ref offset);

        Debug.Log($"Deserialized Values: UInt8={u8}, Int8={i8}, UInt16={u16}, Int16={i16}, UInt32={u32}, Int32={i32}, Float={f32}, String={str}, Color={color}");
    }
}
