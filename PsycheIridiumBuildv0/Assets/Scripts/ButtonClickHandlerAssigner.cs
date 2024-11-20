using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonClickHandlerAssigner : MonoBehaviour
{
    public Button[] levelButtons; // Array of level buttons
    public Button shipUpgradeButton; // Button for ship upgrades

    void Start()
    {
        if (levelButtons.Length != 4)
        {
            Debug.LogError("Expected exactly 4 level buttons for levels 1-4!");
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
            SceneManager.LoadScene("MiniGameScene"); // Replace with your actual scene name
        }
        else
        {
            // Notify the player that the level is locked
            NotifyPlayer($"Level {level} is locked! You need to upgrade your ship to access this level.");
        }
    }

    void OnShipUpgradeButtonClick()
    {
        Debug.Log("Ship Upgrade button clicked. Loading Ship Upgrade scene...");

        // Load the ship upgrade scene
        SceneManager.LoadScene("ShipUpgradeScene"); // Replace with your actual scene name
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

    void NotifyPlayer(string message)
    {
        Debug.LogWarning(message);

        // Create a toast-like message dynamically
        GameObject toastObject = new GameObject("Toast");
        Canvas canvas = toastObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;

        CanvasScaler scaler = toastObject.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);

        toastObject.AddComponent<GraphicRaycaster>();

        GameObject panelObject = new GameObject("Panel");
        panelObject.transform.SetParent(toastObject.transform, false);

        Image panelImage = panelObject.AddComponent<Image>();
        panelImage.color = new Color(0, 0, 0, 0.8f);

        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.sizeDelta = new Vector2(600, 100);
        panelRect.anchorMin = new Vector2(0.5f, 0.5f);
        panelRect.anchorMax = new Vector2(0.5f, 0.5f);
        panelRect.pivot = new Vector2(0.5f, 0.5f);
        panelRect.anchoredPosition = Vector2.zero;

        GameObject textObject = new GameObject("Text");
        textObject.transform.SetParent(panelObject.transform, false);

        Text text = textObject.AddComponent<Text>();
        text.text = message;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.color = Color.white;
        text.fontSize = 24;
        text.alignment = TextAnchor.MiddleCenter;

        RectTransform textRect = textObject.GetComponent<RectTransform>();
        textRect.sizeDelta = new Vector2(580, 80);
        textRect.anchorMin = new Vector2(0.5f, 0.5f);
        textRect.anchorMax = new Vector2(0.5f, 0.5f);
        textRect.pivot = new Vector2(0.5f, 0.5f);
        textRect.anchoredPosition = Vector2.zero;

        Destroy(toastObject, 3f); // Destroy the toast after 3 seconds
    }
}
