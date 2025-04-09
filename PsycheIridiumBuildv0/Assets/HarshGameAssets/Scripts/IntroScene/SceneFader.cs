using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    public CanvasGroup fadeCanvasGroup;
    public float fadeDuration = 1f;

    void Start()
    {
        // Start with a fade in
        StartCoroutine(FadeIn());
    }

    public IEnumerator FadeIn()
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            fadeCanvasGroup.alpha = 1 - (time / fadeDuration);
            time += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.interactable = false;
        fadeCanvasGroup.blocksRaycasts = false;
    }

    public IEnumerator FadeOut()
    {
        fadeCanvasGroup.interactable = true;
        fadeCanvasGroup.blocksRaycasts = true;

        float time = 0f;
        while (time < fadeDuration)
        {
            fadeCanvasGroup.alpha = time / fadeDuration;
            time += Time.deltaTime;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;
    }
}
