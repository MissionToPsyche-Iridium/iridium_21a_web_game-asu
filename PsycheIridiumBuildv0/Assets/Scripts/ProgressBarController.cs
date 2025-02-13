using UnityEngine;
using UnityEngine.UI;
using TMPro; // Import TextMeshPro

public class ProgressBarController : MonoBehaviour
{
    public Image progressBarFillImage;  // Assign in the Inspector
    public float fillDuration = 10f;    // Time in seconds for full progress
    private float elapsedTime = 0f;
    private bool isStopped = false;

    public TMP_Text distanceText;  // Assign TMP_Text for numerical distance
    public TMP_Text timeText;      // Assign TMP_Text for numerical time

    public float maxDistance = 62f; // Example: 62 miles to reach space (100 km)
    public float maxTime = 10f; // Example: 10 minutes for full ascent

    void Update()
    {
        if (isStopped) return; // Stop updating if the rocket collides

        // Update elapsed time
        elapsedTime += Time.deltaTime;
        float fillAmount = Mathf.Clamp01(elapsedTime / fillDuration);

        // Update progress bar
        progressBarFillImage.fillAmount = fillAmount;

        // Scale distance and time
        float traveledDistance = fillAmount * maxDistance;
        float scaledTime = fillAmount * maxTime; // Scale time based on progress

        // Update UI with only numerical values
        if (distanceText != null)
            distanceText.text = traveledDistance.ToString("F1"); // One decimal place

        if (timeText != null)
            timeText.text = scaledTime.ToString("F1"); // One decimal place
    }

    // Call this method to stop the progress bar and tracking
    public void StopProgress()
    {
        isStopped = true;
    }
}
