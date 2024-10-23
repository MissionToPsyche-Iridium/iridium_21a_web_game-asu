using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameObject textbox;

    // Textbox Variables
    private TMP_Text textboxName;
    private TMP_Text textboxText;

    private void Start()
    {
        textboxName = textbox.transform.Find("Name").GetComponent<TMP_Text>();
        textboxText = textbox.transform.Find("Text").GetComponent<TMP_Text>();
    }

    public void ShowTextbox(string name, string text)
    {
        textboxName.text = name;
        textboxText.text = text;
        textbox.SetActive(true);
    }

    public void HideTextbox()
    {
        textbox.SetActive(false);
    }
}
