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
    public GameObject canvas;
    public Vector2 startingPosition = new Vector2(-10, -10);
    public float verticalSpacing = 50f;
    public TMP_FontAsset eightBitFont;
    public List<Resource> resources = new List<Resource>();

    private Dictionary<Asteroid.AsteroidType, int> totalResources;
    private Dictionary<Asteroid.AsteroidType, TextMeshProUGUI> resourceTexts = new();

    void Start()
    {
        totalResources = SetResourcesToTenThousand();
        GenerateResourceUI();
    }

    private Dictionary<Asteroid.AsteroidType, int> SetResourcesToTenThousand()
    {
        var result = new Dictionary<Asteroid.AsteroidType, int>();

        foreach (var res in resources)
        {
            result[res.type] = 0;

            switch (res.type)
            {
                case Asteroid.AsteroidType.Iron:
                    result[res.type] = GameState.Instance.iron;
                    break;
                case Asteroid.AsteroidType.Gold:
                    result[res.type] = GameState.Instance.gold;
                    break;
                case Asteroid.AsteroidType.Tungsten:
                    result[res.type] = GameState.Instance.tungsten;
                    break;
            }
        }

        return result;
    }

    private void GenerateResourceUI()
    {
        Vector2 positionOffset = startingPosition;

        foreach (var resource in resources)
        {
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

            GameObject iconObject = new GameObject(resource.type + "Icon");
            iconObject.transform.SetParent(resourceContainer.transform, false);
            Image iconImage = iconObject.AddComponent<Image>();
            iconImage.sprite = resource.icon;
            iconObject.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);

            GameObject textObject = new GameObject(resource.type + "Text");
            textObject.transform.SetParent(resourceContainer.transform, false);
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.font = eightBitFont;
            text.text = totalResources[resource.type].ToString();
            text.fontSize = 24;
            text.alignment = TextAlignmentOptions.Left;

            resourceTexts.Add(resource.type, text);
            positionOffset += new Vector2(0, -verticalSpacing);
        }
    }
}
