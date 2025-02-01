using System.Collections.Generic;
using UnityEngine;

public class SerializationTest : MonoBehaviour
{
    void Start()
    {
        ExampleType original = new ExampleType(10); // Original values
        List<byte> serializedData = new List<byte>();

        original.Serialize(serializedData); // Serialize the original object

        int offset = 0;
        ExampleType deserialized = new ExampleType(0); // Original values
        deserialized.Deserialize(serializedData.ToArray(), ref offset);

        Debug.Log("Original: " + original.someValue);
        Debug.Log("Deserialized: " + deserialized.someValue);
    }
}
