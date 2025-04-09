using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;
    private static bool instanceCreated = false;

    public bool introComplete = false;
    public bool outroComplete = false;
    public bool spiderRepaired = false;
    public bool jellyfishRepaired = false;
    public bool tardigradeRepaired = false;
    public bool pigeonRepaired = false;
    public int damageRepaired = 0;
    public bool[] repairedDamageObjects = new bool[20];

    private void Awake()
    {
        if (!GameData.instanceCreated)
        {
            instance = this;
            GameData.instanceCreated = true;
            DontDestroyOnLoad(gameObject);
        }
    }


}
