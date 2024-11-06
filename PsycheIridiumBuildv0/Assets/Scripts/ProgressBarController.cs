using UnityEngine;
using UnityEngine.UI;

public class ProgressBarController : MonoBehaviour
{
    public Image progressBarFillImage;  // Assign this in the Inspector
    public float fillDuration = 10f;    // Time in seconds for the bar to fill
    private float elapsedTime = 0f;

    void Update()
    {
            // Increase the elapsed time
            elapsedTime += Time.deltaTime;

            // Calculate the fill amount based on elapsed time
            float fillAmount = Mathf.Clamp01(elapsedTime / fillDuration);

            // Update the fill image
            progressBarFillImage.fillAmount = fillAmount;

            // Optional: Reset or stop when fully filled
            if (fillAmount >= 1f)
            {
                // Bar is fully filled; you can stop increasing elapsedTime or trigger other events here.
            }
        
    }
}


