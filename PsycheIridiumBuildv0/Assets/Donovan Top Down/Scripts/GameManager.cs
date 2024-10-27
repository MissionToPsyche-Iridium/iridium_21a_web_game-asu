using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] HUD hud;

    [Header("Damage")]
    [SerializeField] private Transform damageParent;
    private int damageLeft;

    private void Start()
    {
        damageLeft = damageParent.childCount;
        hud.UpdateDamage(damageLeft);
    }

    public void FixDamage()
    {
        damageLeft--;
        hud.UpdateDamage(damageLeft);
    }
}
