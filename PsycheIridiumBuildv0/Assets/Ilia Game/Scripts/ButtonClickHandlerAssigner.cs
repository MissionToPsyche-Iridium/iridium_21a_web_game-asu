using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ButtonClickHandlerAssigner : MonoBehaviour
{
    public Button[] levelButtons; // Array of level buttons
    public Button shipUpgradeButton; // Button for ship upgrades
    public TMPro.TextMeshProUGUI errorText; // Pre-existing Text UI element for displaying errors

    void Start()
    {
        if (levelButtons.Length != 4)
        {
            Debug.LogError("Expected exactly 4 level buttons for levels 1-4!");
            return;
        }

        if (errorText == null)
        {
            Debug.LogError("ErrorText is not assigned! Please assign a Text UI element in the Inspector.");
            return;
        }

        // Hardcoded level configurations
        int[] maxAsteroids = { 1, 2, 3, 4 };
        float[] intervals = { 2f, 1f, 0.5f, 0.5f };

        // Assign handlers to level buttons
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int levelIndex = i; // Capture the current index for the lambda
            levelButtons[i].onClick.AddListener(() => OnLevelButtonClick(levelIndex + 1, maxAsteroids[levelIndex], intervals[levelIndex]));
        }

        // Assign handler to the ship upgrade button
        if (shipUpgradeButton != null)
        {
            shipUpgradeButton.onClick.AddListener(OnShipUpgradeButtonClick);
        }
        else
        {
            Debug.LogError("Ship Upgrade button is not assigned!");
        }
    }

    void OnLevelButtonClick(int level, int maxAsteroids, float interval)
    {
        // Check if the level is unlocked
        if (IsLevelUnlocked(level))
        {
            Debug.Log($"Level {level} selected. Parameters: maxAsteroids={maxAsteroids}, interval={interval}");

            // Save parameters to PlayerPrefs to pass them to the next scene
            PlayerPrefs.SetInt("MaxAsteroids", maxAsteroids);
            PlayerPrefs.SetFloat("AsteroidInterval", interval);

            // Load the level scene
            SceneManager.LoadScene("Ilia_MiniGameScene"); // Replace with your actual scene name
        }
        else
        {
            // Display an error message in the pre-existing Text UI element
            DisplayError($"Level {level} is locked! You need to upgrade your ship to access this level.");
        }
    }

    void OnShipUpgradeButtonClick()
    {
        Debug.Log("Ship Upgrade button clicked. Loading Ship Upgrade scene...");

        // Load the ship upgrade scene
        SceneManager.LoadScene("Ilia_ShipUpgradeScene"); // Replace with your actual scene name
    }

    bool IsLevelUnlocked(int level)
    {
        // Hardcoded check for unlocked levels
        switch (level)
        {
            case 1: return true; // Level 1 is always unlocked
            case 2: return PlayerPrefs.GetInt("Level2Unlocked", 0) == 1;
            case 3: return PlayerPrefs.GetInt("Level3Unlocked", 0) == 1;
            case 4: return PlayerPrefs.GetInt("Level4Unlocked", 0) == 1;
            default: return false;
        }
    }

    void DisplayError(string message)
    {
        Debug.LogWarning(message);

        // Update the Text element with the error message
        errorText.text = message;

        // Start a coroutine to clear the error after 3 seconds
        StartCoroutine(ClearErrorAfterDelay(3f));
    }

    System.Collections.IEnumerator ClearErrorAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Clear the error message
        errorText.text = string.Empty;
    }
}
