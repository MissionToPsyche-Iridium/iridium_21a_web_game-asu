using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement; // Import TextMeshPro

public class ProgressBarController : MonoBehaviour
{
    public Image progressBarFillImage;
    public float fillDuration = 10f;
    private float elapsedTime = 0f;
    private bool isStopped = false;
    private bool hasCompleted = false; // Prevent multiple triggers
    public static bool gameOver = false; // Global static flag

    public TMP_Text distanceText;
    public TMP_Text timeText;

    public float maxDistance = 430f;
    public float maxTime = 10f;

    [Header("End Game Settings")]
    public GameObject endGamePanel; // Assign optional win screen
    public string nextSceneName;    // Optional scene to load

    void Update()
    {
        if (isStopped || hasCompleted) return;

        elapsedTime += Time.deltaTime;
        float fillAmount = Mathf.Clamp01(elapsedTime / fillDuration);

        progressBarFillImage.fillAmount = fillAmount;

        float traveledDistance = fillAmount * maxDistance;
        float scaledTime = fillAmount * maxTime;

        if (distanceText != null)
            distanceText.text = traveledDistance.ToString("F1");

        if (timeText != null)
            timeText.text = scaledTime.ToString("F1");

        // End game trigger
        if (fillAmount >= 1f)
        {
            hasCompleted = true;
            StartCoroutine(HandleEndGame());
        }
    }

    public void StopProgress()
    {
        isStopped = true;
    }

    IEnumerator HandleEndGame()
    {
        Debug.Log("Rocket reached orbit!");
        gameOver = true; // Stops movement/spawning if other scripts check this

        if (endGamePanel != null)
        {
            endGamePanel.SetActive(true); // Show "Next" button or message
        }

        yield return null; // Wait here — transition will happen on button press
    }
}

