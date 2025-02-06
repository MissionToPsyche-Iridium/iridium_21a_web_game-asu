using System;
using System.Collections.Generic;
using UnityEngine;

public static class PartStorage
{
    public static Dictionary<Vector3, Tuple<KeyCode, Quaternion>> partStorage = new();
}
