using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketToAtmosTransition : MonoBehaviour
{

    public TextMeshProUGUI storyText;
    public TextMeshProUGUI continuePrompt;
    public float textSpeed = 0.03f;
    public string[] storyLines;

    private int currentLine = 0;
    private bool isTyping = false;
    private bool lineFinished = false;

    private void Start()
    {
        continuePrompt.gameObject.SetActive(false);
        storyText.text = "";

        // Start fade in
        SceneFader fader = FindObjectOfType<SceneFader>();
        if (fader != null)
        {
            StartCoroutine(FadeInAndStartTyping(fader));
        }
        else
        {
            StartCoroutine(TypeLine());
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Skip typing and show full line
                StopAllCoroutines();
                storyText.text = storyLines[currentLine];
                isTyping = false;
                lineFinished = true;
                continuePrompt.gameObject.SetActive(true);
            }
            else if (lineFinished)
            {
                currentLine++;

                if (currentLine < storyLines.Length)
                {
                    continuePrompt.gameObject.SetActive(false);
                    StartCoroutine(TypeLine());
                }
                else
                {
                    StartCoroutine(FadeThenLoadScene());
                }
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        lineFinished = false;
        storyText.text = "";

        foreach (char c in storyLines[currentLine])
        {
            storyText.text += c;
            yield return new WaitForSeconds(textSpeed);
        }

        isTyping = false;
        lineFinished = true;
        continuePrompt.gameObject.SetActive(true);
    }

    IEnumerator FadeInAndStartTyping(SceneFader fader)
    {
        yield return fader.FadeIn();
        StartCoroutine(TypeLine());
    }

    IEnumerator FadeThenLoadScene()
    {
        SceneFader fader = FindObjectOfType<SceneFader>();
        if (fader != null)
        {
            yield return fader.FadeOut();
        }

        // Replace with your next scene's name
        SceneManager.LoadScene("AtmosClickAndDrag");
    }
}
