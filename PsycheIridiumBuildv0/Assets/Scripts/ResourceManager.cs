using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    [System.Serializable]
    public class Resource
    {
        public string name;
        public Sprite icon;
        public int amount;
    }

    public List<Resource> resources = new List<Resource>();
    private Dictionary<string, TextMeshProUGUI> resourceTexts = new Dictionary<string, TextMeshProUGUI>();

    public GameObject canvas;  // Assign the main canvas here
    public float scaleFactor = 0.5f; // Controls the size of icons and text
    public Vector2 startingPosition = new Vector2(-10, -10); // Start position for the first resource UI element in top-right
    public float verticalSpacing = 50f; // Spacing between each resource UI element

    void Start()
    {
        GenerateResourceUI();
    }

    void GenerateResourceUI()
    {
        Vector2 positionOffset = startingPosition;

        foreach (Resource resource in resources)
        {
            GameObject resourceContainer = new GameObject(resource.name + "Container");
            resourceContainer.transform.SetParent(canvas.transform, false);

            RectTransform containerRect = resourceContainer.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(1, 1); // Top-right corner
            containerRect.anchorMax = new Vector2(1, 1);
            containerRect.pivot = new Vector2(1, 1); // Align to top-right corner
            containerRect.anchoredPosition = positionOffset;

            HorizontalLayoutGroup layoutGroup = resourceContainer.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.childAlignment = TextAnchor.MiddleLeft;
            layoutGroup.spacing = 10 * scaleFactor;
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;

            // Create Icon
            GameObject iconObject = new GameObject(resource.name + "Icon");
            iconObject.transform.SetParent(resourceContainer.transform, false);
            Image iconImage = iconObject.AddComponent<Image>();
            iconImage.sprite = resource.icon;
            RectTransform iconRect = iconObject.GetComponent<RectTransform>();
            iconRect.sizeDelta = new Vector2(40, 40) * scaleFactor;

            // Create Text
            GameObject textObject = new GameObject(resource.name + "Text");
            textObject.transform.SetParent(resourceContainer.transform, false);
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.text = "0"; // Starts at 0, only displays the number
            text.fontSize = 24 * scaleFactor;
            text.alignment = TextAlignmentOptions.Left;

            resourceTexts.Add(resource.name, text);

            // Move the next resource container down by verticalSpacing
            positionOffset += new Vector2(0, -verticalSpacing);
        }
    }

    public void CollectResource(string resourceName, int amount)
    {
        if (resourceTexts.ContainsKey(resourceName))
        {
            resources.Find(r => r.name == resourceName).amount += amount;
            UpdateResourceUI(resourceName);
        }
    }

    void UpdateResourceUI(string resourceName)
    {
        resourceTexts[resourceName].text = resources.Find(r => r.name == resourceName).amount.ToString();
    }
}
