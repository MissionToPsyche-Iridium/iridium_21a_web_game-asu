using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Dialogue Testing")]
    [SerializeField] private string textName;
    [SerializeField] private string[] text;

    [Header("Object References")]
    [SerializeField] private HUD hud;
    [SerializeField] private PlayerController player;

    private bool playerNearby = false;
    private bool speaking = false;
    private int currentTextbox = -1;

    private void Start()
    {
        player.Interact += Interacted;
    }

    private void Interacted()
    {
        if (playerNearby && !player.Interacting() && !speaking)
        {
            // Freeze player and show first textbox.
            player.EnterInteraction();
            speaking = true;
            currentTextbox = 0;
            hud.ShowTextbox(textName, text[currentTextbox]);
            currentTextbox++;
        }
        else if (speaking && currentTextbox < text.Length)
        {
            // Advance to next textbox.
            hud.ShowTextbox(textName, text[currentTextbox]);
            currentTextbox++;
        }
        else if (speaking && currentTextbox >= text.Length)
        {
            // End interaction, hide textbox, and unfreeze player.
            hud.HideTextbox();
            currentTextbox = -1;
            speaking = false;
            player.ExitInteraction();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerNearby = false;
    }
}
