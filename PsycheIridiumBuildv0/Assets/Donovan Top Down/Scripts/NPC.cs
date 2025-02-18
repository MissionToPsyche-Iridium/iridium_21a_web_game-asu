using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private HUD hud;
    [SerializeField] private PlayerController player;

    [Header("Dialogue")]
    [SerializeField] private string textName;
    [SerializeField] private string[] text;

    [Header("Minigame")]
    [SerializeField] private bool minigameNPC;
    [SerializeField] private string minigameScene;
    [SerializeField] private string minigameName;
    [SerializeField] private string minigameDesc;
    [SerializeField] private Sprite minigameThumbnail;

    private bool playerNearby = false;
    private bool speaking = false;
    private int currentTextbox = -1;

    private void Start()
    {
        player.Interact += Interacted;
    }

    private void Interacted()
    {
        if (playerNearby)
        {
            if (!player.Interacting() && !speaking)
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
                // Hide textbox and show minigame preview.
                if (minigameNPC) hud.ShowMinigamePreview(minigameScene, minigameName, minigameDesc);
                // End interaction, hide textbox, and unfreeze player.
                else player.ExitInteraction();

                // End NPC interaction.
                hud.HideTextbox();
                currentTextbox = -1;
                speaking = false;
            }
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
