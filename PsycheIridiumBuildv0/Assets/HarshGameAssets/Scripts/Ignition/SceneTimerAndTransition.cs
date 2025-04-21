using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTimerAndTransition : MonoBehaviour
{
    public float waitTime = 10f;
    public CanvasGroup continuePrompt;
    public string nextSceneName = "NextScene"; // replace with your actual scene name

    private bool promptVisible = false;

    void Start()
    {
        if (continuePrompt != null)
        {
            continuePrompt.alpha = 0;
            continuePrompt.interactable = false;
            continuePrompt.blocksRaycasts = false;
        }

        StartCoroutine(WaitAndShowPrompt());
    }

    IEnumerator WaitAndShowPrompt()
    {
        yield return new WaitForSeconds(waitTime);

        if (continuePrompt != null)
        {
            continuePrompt.alpha = 1;
            continuePrompt.interactable = true;
            continuePrompt.blocksRaycasts = true;
        }

        promptVisible = true;
    }

    void Update()
    {
        if (promptVisible && Input.GetMouseButtonDown(0))
        {
            StartCoroutine(FadeAndLoadNextScene());
        }
    }

    IEnumerator FadeAndLoadNextScene()
    {
        SceneFader fader = FindObjectOfType<SceneFader>();
        if (fader != null)
        {
            yield return fader.FadeOut();
        }

        SceneManager.LoadScene(nextSceneName);
    }
}
