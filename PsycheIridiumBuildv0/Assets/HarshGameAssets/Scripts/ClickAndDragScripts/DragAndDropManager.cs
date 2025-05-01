using UnityEngine;
using UnityEngine.UI;

public class DragAndDropManager : MonoBehaviour
{
    public int totalSlots = 5; // Set in Inspector
    private int correctPlacements = 0;

    public GameObject nextStepUI; // UI to show when complete (e.g. button or panel)

    [SerializeField] private AudioClip correctAudio;
    [SerializeField] private AudioClip incorrectAudio;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        correctPlacements = 0;

        if (nextStepUI != null)
        {
            nextStepUI.SetActive(false);
        }
    }

    public void RegisterCorrectPlacement()
    {
        correctPlacements++;
        audioSource.PlayOneShot(correctAudio);

        if (correctPlacements >= totalSlots)
        {
            Debug.Log("All items correctly placed!");
            if (nextStepUI != null)
            {
                nextStepUI.SetActive(true); // Show next step
            }

            // Optional: auto-progress or trigger next phase
            // StartCoroutine(ProceedToNextPhase());
        }
    }

    public void RegisterIncorrectPlacement()
    {
        audioSource.PlayOneShot(incorrectAudio);
    }
}
