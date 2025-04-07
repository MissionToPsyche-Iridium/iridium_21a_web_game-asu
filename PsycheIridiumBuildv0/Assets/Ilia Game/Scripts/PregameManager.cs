using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class Resource
{
    public Asteroid.AsteroidType type; // Resource type (Iron, Gold, Tungsten)
    public Sprite icon;               // Resource icon
}

public class PregameManager : MonoBehaviour
{
    public GameObject canvas; // Reference to the Canvas
    public Vector2 startingPosition = new Vector2(-10, -10);
    public float verticalSpacing = 50f;
    public TMP_FontAsset eightBitFont; // Reference to the 8-bit font
    public List<Resource> resources = new List<Resource>(); // List of resources

    private Dictionary<Asteroid.AsteroidType, int> totalResources; // Total collected resources
    private Dictionary<Asteroid.AsteroidType, TextMeshProUGUI> resourceTexts = new Dictionary<Asteroid.AsteroidType, TextMeshProUGUI>();

    void Start()
    {
        // Ensure game state is loaded
        GameState.Instance.LoadGameState();

        // Load resources from GameState
        totalResources = LoadResourcesFromGameState();

        // Generate resource UI
        GenerateResourceUI();
    }

    private Dictionary<Asteroid.AsteroidType, int> LoadResourcesFromGameState()
    {
        return new Dictionary<Asteroid.AsteroidType, int>
        {
            { Asteroid.AsteroidType.Iron, GameState.Instance.iron },
            { Asteroid.AsteroidType.Gold, GameState.Instance.gold },
            { Asteroid.AsteroidType.Tungsten, GameState.Instance.tungsten }
        };
    }

    void GenerateResourceUI()
    {
        Vector2 positionOffset = startingPosition;

        foreach (var resource in resources)
        {
            // Create container for each resource
            GameObject resourceContainer = new GameObject(resource.type + "Container");
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
            GameObject iconObject = new GameObject(resource.type + "Icon");
            iconObject.transform.SetParent(resourceContainer.transform, false);
            Image iconImage = iconObject.AddComponent<Image>();
            iconImage.sprite = resource.icon; // Assign the icon
            RectTransform iconRect = iconObject.GetComponent<RectTransform>();
            iconRect.sizeDelta = new Vector2(40, 40);

            // Add text
            GameObject textObject = new GameObject(resource.type + "Text");
            textObject.transform.SetParent(resourceContainer.transform, false);
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.font = eightBitFont; // Use the 8-bit font
            text.text = totalResources.ContainsKey(resource.type) ? totalResources[resource.type].ToString() : "0";
            text.fontSize = 24;
            text.alignment = TextAlignmentOptions.Left;

            resourceTexts.Add(resource.type, text);

            positionOffset += new Vector2(0, -verticalSpacing); // Move down for the next resource
        }
    }
}
