using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [Header("General")]
    [SerializeField] PlayerController playerController;
    [SerializeField] GameManager gameManager;

    [Header("Minimap")]
    [SerializeField] GameObject minimap;
    [SerializeField] GameObject minimapIndicator;

    [Header("Damage")]
    [SerializeField] TMP_Text damageIndicator;

    [Header("Textboxes")]
    [SerializeField] private GameObject textbox;
    private TMP_Text textboxName;
    private TMP_Text textboxText;

    private void Start()
    {
        textboxName = textbox.transform.Find("Name").GetComponent<TMP_Text>();
        textboxText = textbox.transform.Find("Text").GetComponent<TMP_Text>();
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

    private void UpdateMinimap()
    {
        RectTransform rectTransform = minimapIndicator.GetComponent<RectTransform>();
        Vector3 playerPos = playerController.transform.position;
        rectTransform.localPosition = new Vector3(Mathf.Round(playerPos.x / 5) * 10, Mathf.Round(playerPos.y / 5) * 10, 0);
    }

    public void UpdateDamage(int damage)
    {
        damageIndicator.text = "Damage Left: " + damage.ToString();
    }
}
