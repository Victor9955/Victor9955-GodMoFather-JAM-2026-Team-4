using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GlobalInputs<T> where T : IInputActionCollection2, IDisposable, new()
{
    public static T input;

    [RuntimeInitializeOnLoadMethod]
    static void InitInput()
    {
        input = new T();
        input.Enable();
    }

}
