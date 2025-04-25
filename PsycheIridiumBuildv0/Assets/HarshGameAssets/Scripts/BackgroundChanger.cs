using UnityEngine;

public class BackgroundManager : MonoBehaviour
{
    public GameObject[] backgroundElements; // Array to hold the different background elements
    public float[] changeTimes;  // Time intervals at which the background elements should change
    private float elapsedTime = 0f;
    private int currentLayerIndex = 0;

    void Start()
    {
        // Make sure all backgrounds are disabled initially, except the first one
        for (int i = 1; i < backgroundElements.Length; i++)
        {
            backgroundElements[i].SetActive(false);
        }
    }

    void Update()
    {
        if (ProgressBarController.gameOver) return;
        // Update elapsed time
        elapsedTime += Time.deltaTime;

        // Check if it's time to change the background
        if (currentLayerIndex < changeTimes.Length && elapsedTime >= changeTimes[currentLayerIndex])
        {
            ChangeBackground();
        }
    }

    void ChangeBackground()
    {
        if (ProgressBarController.gameOver) return;
        // Deactivate the current background
        backgroundElements[currentLayerIndex].SetActive(false);

        // Move to the next background
        currentLayerIndex++;

        // Ensure we don't go out of bounds
        if (currentLayerIndex < backgroundElements.Length)
        {
            // Activate the next background
            backgroundElements[currentLayerIndex].SetActive(true);
        }
    }
}
