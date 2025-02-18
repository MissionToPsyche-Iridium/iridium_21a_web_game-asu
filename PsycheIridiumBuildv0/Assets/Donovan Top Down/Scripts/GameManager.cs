using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] HUD hud;

    [Header("Minigame NPCs")]
    [SerializeField] private GameObject spiderB;
    [SerializeField] private GameObject spiderR;
    [SerializeField] private GameObject jellyfishB;
    [SerializeField] private GameObject jellyfishR;
    [SerializeField] private GameObject tardigradeB;
    [SerializeField] private GameObject tardigradeR;
    [SerializeField] private GameObject pidgeonB;
    [SerializeField] private GameObject pidgeonR;

    [Header("Progress")]
    public bool spiderRepaired = false;
    public bool jellyfishRepaired = false;
    public bool tardigradeRepaired = false;
    public bool pidgeonRepaired = false;

    [Header("Damage")]
    [SerializeField] private Transform damageParent;
    private int damageLeft;

    private void Start()
    {
        damageLeft = damageParent.childCount;
        hud.UpdateDamage(damageLeft);

        UpdateNPCs();
    }

    public void FixDamage()
    {
        damageLeft--;
        hud.UpdateDamage(damageLeft);
    }

    public void UpdateNPCs()
    {
        if (spiderRepaired)
        {
            spiderB.SetActive(false);
            spiderR.SetActive(true);
        }
        else
        {
            spiderB.SetActive(true);
            spiderR.SetActive(false);
        }

        if (jellyfishRepaired)
        {
            jellyfishB.SetActive(false);
            jellyfishR.SetActive(true);
        }
        else
        {
            jellyfishB.SetActive(true);
            jellyfishR.SetActive(false);
        }

        if (tardigradeRepaired)
        {
            tardigradeB.SetActive(false);
            tardigradeR.SetActive(true);
        }
        else
        {
            tardigradeB.SetActive(true);
            tardigradeR.SetActive(false);
        }

        if (pidgeonRepaired)
        {
            pidgeonB.SetActive(false);
            pidgeonR.SetActive(true);
        }
        else
        {
            pidgeonB.SetActive(true);
            pidgeonR.SetActive(false);
        }
    }
}
