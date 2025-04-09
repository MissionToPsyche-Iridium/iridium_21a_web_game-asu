using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] HUD hud;

    [Header("Cutscene NPCs")]
    [SerializeField] private NPC introNPC;
    [SerializeField] private NPC outroNPC;

    [Header("Minigame NPCs")]
    [SerializeField] private GameObject spiderB;
    [SerializeField] private GameObject spiderR;
    [SerializeField] private GameObject jellyfishB;
    [SerializeField] private GameObject jellyfishR;
    [SerializeField] private GameObject tardigradeB;
    [SerializeField] private GameObject tardigradeR;
    [SerializeField] private GameObject pigeonB;
    [SerializeField] private GameObject pigeonR;

    [Header("Damage")]
    [SerializeField] Damage[] damageObjects;

    private void Start()
    {
        // Play the outro or intro cutscene if the conditions are right.
        if (GameData.instance.spiderRepaired && GameData.instance.jellyfishRepaired && GameData.instance.tardigradeRepaired && GameData.instance.pigeonRepaired && !GameData.instance.outroComplete)
        {
            GameData.instance.outroComplete = true;
            hud.FadeInInstant();
            outroNPC.StartCutscene();
        }
        else if (!GameData.instance.introComplete)
        {
            GameData.instance.introComplete = true;
            introNPC.StartCutscene();
        }
        else hud.FadeInInstant();

        InitializeDamage();

        // Set the currently active mechanical creature NPCs based on their repaired status.
        UpdateNPCs();
    }

    private void InitializeDamage()
    {
        for (int i = 0; i < damageObjects.Length; i++)
        {
            damageObjects[i].DamageInit(i);
        }

        // Update the damage indicator on the HUD.
        hud.UpdateDamage(damageObjects.Length - GameData.instance.damageRepaired, false);
    }

    // Fix an optional damage point.
    public void FixDamage()
    {
        GameData.instance.damageRepaired += 1;
        hud.UpdateDamage(damageObjects.Length - GameData.instance.damageRepaired, true);
    }

    // Sets whether the damaged or repaired version of each mechanical creature NPC is shown.
    public void UpdateNPCs()
    {
        if (GameData.instance.spiderRepaired)
        {
            spiderB.SetActive(false);
            spiderR.SetActive(true);
        }
        else
        {
            spiderB.SetActive(true);
            spiderR.SetActive(false);
        }

        if (GameData.instance.jellyfishRepaired)
        {
            jellyfishB.SetActive(false);
            jellyfishR.SetActive(true);
        }
        else
        {
            jellyfishB.SetActive(true);
            jellyfishR.SetActive(false);
        }

        if (GameData.instance.tardigradeRepaired)
        {
            tardigradeB.SetActive(false);
            tardigradeR.SetActive(true);
        }
        else
        {
            tardigradeB.SetActive(true);
            tardigradeR.SetActive(false);
        }

        if (GameData.instance.pigeonRepaired)
        {
            pigeonB.SetActive(false);
            pigeonR.SetActive(true);
        }
        else
        {
            pigeonB.SetActive(true);
            pigeonR.SetActive(false);
        }
    }
}
