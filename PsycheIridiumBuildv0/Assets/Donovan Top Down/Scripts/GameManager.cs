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
    [SerializeField] private GameObject pidgeonB;
    [SerializeField] private GameObject pidgeonR;

    [Header("Progress (Saved Across Minigames)")]
    public bool introComplete = false;
    public bool outroComplete = false;
    public bool spiderRepaired = false;
    public bool jellyfishRepaired = false;
    public bool tardigradeRepaired = false;
    public bool pidgeonRepaired = false;
    public byte damageRepaired = 0;

    [Header("Damage")]
    [SerializeField] private Transform damageParent;

    private void Start()
    {
        // Play the outro or intro cutscene if the conditions are right.
        if (spiderRepaired && jellyfishRepaired && tardigradeRepaired && pidgeonRepaired && !outroComplete)
        {
            outroComplete = true;
            hud.FadeInInstant();
            outroNPC.StartCutscene();
        }
        else if (!introComplete)
        {
            introComplete = true;
            introNPC.StartCutscene();
        }
        else hud.FadeInInstant();

        // Update the damage indicator on the HUD.
        hud.UpdateDamage(damageParent.childCount - damageRepaired, false);

        // Set the currently active mechanical creature NPCs based on their repaired status.
        UpdateNPCs();
    }

    // Fix an optional damage point.
    public void FixDamage()
    {
        damageRepaired += 1;
        hud.UpdateDamage(damageParent.childCount - damageRepaired, true);
    }

    // Sets whether the damaged or repaired version of each mechanical creature NPC is shown.
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
