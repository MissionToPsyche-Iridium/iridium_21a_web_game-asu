using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private HUD hud;
    [SerializeField] private PlayerController player;

    [Header("Dialogue")]
    [SerializeField] private string textName;
    [SerializeField] private string[] text;

    [Header("Cutscene")]
    [SerializeField] private bool isCutscene;
    [SerializeField] private bool fadeInAtEnd;

    [Header("Minigame")]
    [SerializeField] private bool minigameNPC;
    [SerializeField] private string minigameScene;
    [SerializeField] private string minigameName;
    [SerializeField] private string minigameDesc;
    [SerializeField] private Sprite minigameThumbnail;

    [Header("Audio")]
    [SerializeField] AudioSource audioSource;


    private bool playerNearby = false;
    private bool speaking = false;
    private bool cutsceneStarted = false;
    private int currentTextbox = -1;

    private void Start()
    {
        StartCoroutine(WaitForPlayer());
        player.Interact += Interacted;
    }

    public void StartCutscene()
    {
        if (isCutscene)
        {
            cutsceneStarted = true;
            Interacted();
        }
    }

    public void Interacted()
    {
        // Check to make sure the player is nearby.
        // Cutscenes bypass this check since they are started from scripts.
        if ((!isCutscene && playerNearby) || (isCutscene && cutsceneStarted))
        {
            // First Interaction
            if (!player.Interacting() && !speaking)
            {
                // Freeze player and show first textbox.
                StartCoroutine(WaitForPlayer());
                player.EnterInteraction();
                speaking = true;
                currentTextbox = 0;
                hud.ShowTextbox(textName, text[currentTextbox]);
                currentTextbox++;
                if (!isCutscene) audioSource.Play();
            }
            // Middle Interactions
            else if (speaking && currentTextbox < text.Length)
            {
                // Advance to next textbox.
                hud.ShowTextbox(textName, text[currentTextbox]);
                currentTextbox++;
                audioSource.Play();
            }
            // Last Interaction
            else if (speaking && currentTextbox >= text.Length)
            {
                // If the NPC fades in after dialogue, do so.
                if (fadeInAtEnd) StartCoroutine(hud.FadeIn(null));

                // Hide textbox and show minigame preview.
                if (minigameNPC) hud.ShowMinigamePreview(minigameScene, minigameName, minigameDesc, minigameThumbnail);
                // End interaction, hide textbox, and unfreeze player.
                else player.ExitInteraction();

                // End NPC interaction.
                hud.HideTextbox();
                currentTextbox = -1;
                speaking = false;
                cutsceneStarted = false;
                audioSource.Play();
            }
        }
    }

    private IEnumerator WaitForPlayer()
    {
        while (player == null)
        {
            Debug.Log("Waiting for player...");
            yield return new WaitForEndOfFrame();
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
