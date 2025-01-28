using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public interface ISerializeInterface
{
    public Opcode opcode { get; }
    public void Serialize(List<byte> byteArray);
    public void Deserialize(byte[] byteArray, ref int offset);
}
