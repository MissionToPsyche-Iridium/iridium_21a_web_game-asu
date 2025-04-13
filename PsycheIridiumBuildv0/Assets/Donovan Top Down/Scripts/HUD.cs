using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [Header("General")]
    [SerializeField] PlayerController playerController;
    [SerializeField] GameManager gameManager;

    [Header("Textboxes")]
    [SerializeField] private GameObject textbox;
    private TMP_Text textboxName;
    private TMP_Text textboxText;

    [Header("Minigame Preview")]
    [SerializeField] private GameObject minigamePreview;
    [SerializeField] private Button startButton;
    [SerializeField] private Button cancelButton;
    private TMP_Text previewName;
    private TMP_Text previewDesc;
    private SpriteRenderer previewImage;
    private string previewScene;

    [Header("Minimap")]
    [SerializeField] GameObject minimap;
    [SerializeField] GameObject minimapIndicator;
    [SerializeField] GameObject creatureIndicatorS;
    [SerializeField] GameObject creatureIndicatorT;
    [SerializeField] GameObject creatureIndicatorP;
    [SerializeField] GameObject creatureIndicatorJ;

    [Header("Damaged Creature References")]
    [SerializeField] GameObject damagedCreatureS;
    [SerializeField] GameObject damagedCreatureT;
    [SerializeField] GameObject damagedCreatureP;
    [SerializeField] GameObject damagedCreatureJ;

    [Header("Status Indicators")]
    [SerializeField] TMP_Text damageIndicator;
    [SerializeField] TMP_Text turboIndicator;

    [Header("Fade")]
    [SerializeField] Image fade;
    [SerializeField] float fadeFrames;
    [SerializeField] float fadeDuration;

    [Header("Sound Effects")]
    [SerializeField] AudioClip selectSFX;
    [SerializeField] AudioClip cancelSFX;
    [SerializeField] AudioClip repairSFX;
    [SerializeField] AudioClip woopSFX;

    private bool fadingIn = false;
    private bool fadingOut = false;

    private void Start()
    {
        // Textbox Components
        textboxName = textbox.transform.Find("Name").GetComponent<TMP_Text>();
        textboxText = textbox.transform.Find("Text").GetComponent<TMP_Text>();

        // Minigame Preview Components
        previewName = minigamePreview.transform.Find("Name").GetComponent<TMP_Text>();
        previewDesc = minigamePreview.transform.Find("Description").GetComponent<TMP_Text>();
        previewImage = minigamePreview.transform.Find("Image").GetComponent<SpriteRenderer>();

        // Minimap Setup
        SetupMinimap();
    }

    private void Update()
    {
        UpdateMinimap();
    }

    public void ShowTextbox(string textName, string text)
    {
        textboxName.text = textName;
        textboxText.text = text;
        textbox.SetActive(true);
    }

    public void HideTextbox()
    {
        textbox.SetActive(false);
    }

    public void ShowMinigamePreview(string minigameScene, string minigameName, string minigameDesc)
    {
        previewName.text = minigameName;
        previewDesc.text = minigameDesc;
        previewScene = minigameScene;

        minigamePreview.SetActive(true);

        startButton.interactable = true;
        cancelButton.interactable = true;
    }

    public void StartMinigame()
    {
        Debug.Log("START MINIGAME " + previewScene + "!");
        GetComponent<AudioSource>().PlayOneShot(selectSFX);
        HideMinigamePreview();
        StartCoroutine(FadeOut(previewScene));
    }

    public void CancelMinigame()
    {
        GetComponent<AudioSource>().PlayOneShot(cancelSFX);
        HideMinigamePreview();
    }

    public void HideMinigamePreview()
    {
        startButton.interactable = false;
        cancelButton.interactable = false;

        minigamePreview.SetActive(false);
        playerController.ExitInteraction();
    }

    // Places the damaged creature indicators on the minimap.
    private void SetupMinimap()
    {
        // Move the damaged creature indicators to the correct spots.
        RectTransform rectTransform = creatureIndicatorS.GetComponent<RectTransform>();
        Vector3 pos = damagedCreatureS.transform.position;
        rectTransform.localPosition = new Vector3(Mathf.Round(pos.x / 5) * 5, Mathf.Round(pos.y / 5) * 5, 0);

        rectTransform = creatureIndicatorT.GetComponent<RectTransform>();
        pos = damagedCreatureT.transform.position;
        rectTransform.localPosition = new Vector3(Mathf.Round(pos.x / 5) * 5, Mathf.Round(pos.y / 5) * 5, 0);

        rectTransform = creatureIndicatorP.GetComponent<RectTransform>();
        pos = damagedCreatureP.transform.position;
        rectTransform.localPosition = new Vector3(Mathf.Round(pos.x / 5) * 5, Mathf.Round(pos.y / 5) * 5, 0);

        rectTransform = creatureIndicatorJ.GetComponent<RectTransform>();
        pos = damagedCreatureJ.transform.position;
        rectTransform.localPosition = new Vector3(Mathf.Round(pos.x / 5) * 5, Mathf.Round(pos.y / 5) * 5, 0);
    }

    // Update's the player's position and display of damaged creatures on the minimap.
    private void UpdateMinimap()
    {
        // Update the player indicator's position.
        RectTransform rectTransform = minimapIndicator.GetComponent<RectTransform>();
        Vector3 playerPos = playerController.transform.position;
        rectTransform.localPosition = new Vector3(Mathf.Round(playerPos.x / 5) * 5, Mathf.Round(playerPos.y / 5) * 5, 0);

        // Update the indicators for each of the damaged creatures.
        creatureIndicatorS.SetActive(damagedCreatureS.activeSelf);
        creatureIndicatorT.SetActive(damagedCreatureT.activeSelf);
        creatureIndicatorP.SetActive(damagedCreatureP.activeSelf);
        creatureIndicatorJ.SetActive(damagedCreatureJ.activeSelf);
    }

    // Updates damage repaired and unlocks TURBO when all damage is repaired.
    public void UpdateDamage(int damage, bool playAudio)
    {
        if (damage == 0)
        {
            damageIndicator.text = "Press T to go TURBO!";
            playerController.turboUnlocked = true;
            SetTurbo(false);
            if (playAudio) GetComponent<AudioSource>().PlayOneShot(woopSFX);
        }
        else
        {
            damageIndicator.text = "Damage Left: " + damage.ToString();
            if (playAudio) GetComponent<AudioSource>().PlayOneShot(repairSFX);
        }
    }

    // Sets the turbo indicator's text.
    public void SetTurbo(bool turbo)
    {
        if (turbo) turboIndicator.text = "TURBO engaged!";
        else turboIndicator.text = "TURBO disengaged!";
    }

    public void FadeInInstant()
    {
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
        fade.gameObject.SetActive(false);
    }

    public IEnumerator FadeIn(NPC startCutsceneNPC)
    {
        fadingIn = true;
        while (fade.color.a > 0)
        {
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a - (1 / fadeFrames));
            yield return new WaitForSeconds(fadeDuration / fadeFrames);
        }
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 0);
        fade.gameObject.SetActive(false);
        fadingIn = false;

        if (startCutsceneNPC != null) { startCutsceneNPC.StartCutscene(); }
    }

    public void FadeOutInstant()
    {
        fade.gameObject.SetActive(true);
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
    }

    public IEnumerator FadeOut(string sceneTransition)
    {
        fadingOut = true;
        fade.gameObject.SetActive(true);
        while (fade.color.a < 1)
        {
            fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, fade.color.a + (1 / fadeFrames));
            yield return new WaitForSeconds(fadeDuration / fadeFrames);
        }
        fade.color = new Color(fade.color.r, fade.color.g, fade.color.b, 1);
        fadingOut = false;

        if (sceneTransition != null) SceneManager.LoadScene(sceneTransition);
    }
}
