using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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
    private TMP_Text previewName;
    private TMP_Text previewDesc;
    private SpriteRenderer previewImage;
    private string previewScene;

    [Header("Minimap")]
    [SerializeField] GameObject minimap;
    [SerializeField] GameObject minimapIndicator;

    [Header("Damage")]
    [SerializeField] TMP_Text damageIndicator;

    private void Start()
    {
        // Textbox Components
        textboxName = textbox.transform.Find("Name").GetComponent<TMP_Text>();
        textboxText = textbox.transform.Find("Text").GetComponent<TMP_Text>();

        // Minigame Preview Components
        previewName = minigamePreview.transform.Find("Name").GetComponent<TMP_Text>();
        previewDesc = minigamePreview.transform.Find("Description").GetComponent<TMP_Text>();
        previewImage = minigamePreview.transform.Find("Image").GetComponent<SpriteRenderer>();
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
    }

    public void StartMinigame()
    {
        Debug.Log("START MINIGAME " + previewScene + "!");
        HideMinigamePreview();
    }

    public void HideMinigamePreview()
    {
        minigamePreview.SetActive(false);
        playerController.ExitInteraction();
    }

    private void UpdateMinimap()
    {
        RectTransform rectTransform = minimapIndicator.GetComponent<RectTransform>();
        Vector3 playerPos = playerController.transform.position;
        rectTransform.localPosition = new Vector3(Mathf.Round(playerPos.x / 10) * 10, Mathf.Round(playerPos.y / 10) * 10, 0);
    }

    public void UpdateDamage(int damage)
    {
        damageIndicator.text = "Damage Left: " + damage.ToString();
    }
}
