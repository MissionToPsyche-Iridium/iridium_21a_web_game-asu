using System;
using System.Collections.Generic;
using UnityEngine;

public static class PartStorage
{
    // Store parts in a dictionary that persists between reloads
    public static Dictionary<Vector3, Tuple<KeyCode, Quaternion>> partStorage = new();
}
