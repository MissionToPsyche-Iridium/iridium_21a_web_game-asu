using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketToAtmosTransition : MonoBehaviour
{
    [Header("Text Setup")]
    public TextMeshProUGUI storyText;
    public TextMeshProUGUI continuePrompt;
    public float textSpeed = 0.03f;
    public string[] storyLines;

    [Header("Scene Transition")]
    public string nextSceneName = "AtmosClickAndDrag"; // Editable in Inspector

    private int currentLine = 0;
    private bool isTyping = false;
    private bool lineFinished = false;

    private void Start()
    {
        continuePrompt.gameObject.SetActive(false);
        storyText.text = "";

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

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else
        {
            Debug.LogWarning("No scene name specified! Add one to the script in the Inspector.");
        }
    }
}
