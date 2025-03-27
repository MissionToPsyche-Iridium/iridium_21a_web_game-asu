using System;
using System.Collections.Generic;
using UnityEngine;

// static class to persistent between reloads
public static class PartStorage
{
    // store placed parts in a dictionary with position, part type, and rotation
    public static Dictionary<Vector3, Tuple<KeyCode, Quaternion>> partStorage = new();
}
