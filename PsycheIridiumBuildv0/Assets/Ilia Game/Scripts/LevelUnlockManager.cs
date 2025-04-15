using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUnlockManager : MonoBehaviour
{
    [System.Serializable]
    public class Resource
    {
        public string name; // Resource name (e.g., Iron, Gold, Tungsten)
        public Sprite icon; // Resource icon
    }

    public List<Resource> resources = new List<Resource>(); // List of resources with icons
    public GameObject canvas; // Reference to the Canvas
    public TMP_FontAsset eightBitFont; // Reference to the 8-bit font
    public TextMeshProUGUI errorText; // Reference to the error TextMeshPro element
    public Button[] unlockButtons; // Buttons for unlocking levels

    private Dictionary<string, TextMeshProUGUI> resourceTexts = new Dictionary<string, TextMeshProUGUI>();

    private readonly int[,] levelCosts = {
        { 200, 100, 80 },       // Costs for Level 2
        { 1, 1, 1 }, //{ 4000, 1000, 900 },    // Costs for Level 3
        { 9000, 7000, 3600 }    // Costs for Level 4
    };

    void Start()
    {
        LoadResourcesFromPersistentStorage();
        InitializeResourceDisplay(); // Fixed method name
        AssignButtonHandlers();
    }

    void LoadResourcesFromPersistentStorage()
    {
        foreach (var resource in resources)
        {
            switch (resource.name)
            {
                case "Iron":
                    resourceTexts[resource.name] = CreateResourceUI(resource, GameState.Instance.iron);
                    break;
                case "Gold":
                    resourceTexts[resource.name] = CreateResourceUI(resource, GameState.Instance.gold);
                    break;
                case "Tungsten":
                    resourceTexts[resource.name] = CreateResourceUI(resource, GameState.Instance.tungsten);
                    break;
                default:
                    Debug.LogWarning($"Unknown resource: {resource.name}");
                    break;
            }
        }
    }

    void InitializeResourceDisplay()
    {
        foreach (var resource in resources)
        {
            if (!resourceTexts.ContainsKey(resource.name))
            {
                CreateResourceUI(resource, 0);
            }
        }
    }

    TextMeshProUGUI CreateResourceUI(Resource resource, int amount)
    {
        Vector2 positionOffset = new Vector2(10, -10 - resourceTexts.Count * 50);

        GameObject resourceContainer = new GameObject(resource.name + "Container");
        resourceContainer.transform.SetParent(canvas.transform, false);

        RectTransform containerRect = resourceContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = new Vector2(1, 1);
        containerRect.anchorMax = new Vector2(1, 1);
        containerRect.pivot = new Vector2(1, 1);
        containerRect.anchoredPosition = positionOffset;

        HorizontalLayoutGroup layoutGroup = resourceContainer.AddComponent<HorizontalLayoutGroup>();
        layoutGroup.childAlignment = TextAnchor.MiddleLeft;
        layoutGroup.spacing = 10;
        layoutGroup.childControlWidth = false;
        layoutGroup.childControlHeight = false;

        // Add icon
        GameObject iconObject = new GameObject(resource.name + "Icon");
        iconObject.transform.SetParent(resourceContainer.transform, false);
        Image iconImage = iconObject.AddComponent<Image>();
        iconImage.sprite = resource.icon;
        RectTransform iconRect = iconObject.GetComponent<RectTransform>();
        iconRect.sizeDelta = new Vector2(40, 40);

        // Add text
        GameObject textObject = new GameObject(resource.name + "Text");
        textObject.transform.SetParent(resourceContainer.transform, false);
        TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
        text.font = eightBitFont; // Use the 8-bit font
        text.text = amount.ToString();
        text.fontSize = 24;
        text.alignment = TextAlignmentOptions.Left;

        resourceTexts[resource.name] = text;
        return text;
    }

    void AssignButtonHandlers()
    {
        if (unlockButtons.Length != 3)
        {
            Debug.LogError("Assign exactly 3 buttons for unlocking levels.");
            return;
        }

        for (int i = 0; i < unlockButtons.Length; i++)
        {
            int levelIndex = i; // Capture index for lambda
            unlockButtons[i].onClick.AddListener(() => TryUnlockLevel(levelIndex));
        }
    }

    void TryUnlockLevel(int levelIndex)
    {
        // Check if the level is already purchased
        if (IsLevelPurchased(levelIndex))
        {
            errorText.text = "You already bought this.";
            return;
        }

        // Check if the previous level is unlocked
        if (levelIndex > 0 && !IsLevelPurchased(levelIndex - 1))
        {
            errorText.text = "You must unlock the previous level first.";
            return;
        }

        // Check if there are enough resources to unlock
        int requiredIron = levelCosts[levelIndex, 0];
        int requiredGold = levelCosts[levelIndex, 1];
        int requiredTungsten = levelCosts[levelIndex, 2];

        if (GameState.Instance.iron < requiredIron || GameState.Instance.gold < requiredGold || GameState.Instance.tungsten < requiredTungsten)
        {
            errorText.text = "You don't have enough resources to buy it!";
            return;
        }

        // Unlock the level and subtract resources
        SubtractResources(requiredIron, requiredGold, requiredTungsten);
        UnlockLevel(levelIndex);
        errorText.text = $"Level {levelIndex + 2} unlocked!";
    }

    bool IsLevelPurchased(int levelIndex)
    {
        switch (levelIndex)
        {
            case 0: return GameState.Instance.level2Purchased;
            case 1: return GameState.Instance.level3Purchased;
            case 2: return GameState.Instance.level4Purchased;
            default: return false;
        }
    }

    void UnlockLevel(int levelIndex)
    {
        switch (levelIndex)
        {
            case 0: GameState.Instance.level2Purchased = true; break;
            case 1: GameState.Instance.level3Purchased = true; GameData.instance.tardigradeRepaired = true; break;
            case 2: GameState.Instance.level4Purchased = true; break;
        }

        GameState.Instance.SaveGameState(); // Persist changes
    }

    void SubtractResources(int iron, int gold, int tungsten)
    {
        GameState.Instance.iron -= iron;
        GameState.Instance.gold -= gold;
        GameState.Instance.tungsten -= tungsten;

        UpdateResourceDisplay();
        GameState.Instance.SaveGameState(); // Persist changes
    }

    void UpdateResourceDisplay()
    {
        resourceTexts["Iron"].text = GameState.Instance.iron.ToString();
        resourceTexts["Gold"].text = GameState.Instance.gold.ToString();
        resourceTexts["Tungsten"].text = GameState.Instance.tungsten.ToString();
    }
}
