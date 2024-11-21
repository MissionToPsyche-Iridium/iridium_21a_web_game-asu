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
        public Asteroid.AsteroidType type;
        public Sprite icon;
        public int amount;
    }

    public TMP_FontAsset eightBitFont;
    public List<Resource> resources = new List<Resource>();
    private Dictionary<Asteroid.AsteroidType, TextMeshProUGUI> resourceTexts = new Dictionary<Asteroid.AsteroidType, TextMeshProUGUI>();

    public GameObject canvas;
    public float scaleFactor = 1f;
    public Vector2 startingPosition = new Vector2(-10, -10);
    public float verticalSpacing = 50f;

    public float timerDuration = 60f;
    private TextMeshProUGUI timerText;
    private float timeRemaining;

    private TextMeshProUGUI instructionText;
    private Color orangeColor = new Color(1f, 0.64f, 0f);

    void Start()
    {
        timeRemaining = timerDuration;
        GenerateResourceUI();
        GenerateTimerUI();
        GenerateInstructionUI();
    }

    void Update()
    {
        UpdateTimer();
    }

    void GenerateResourceUI()
    {
        Vector2 positionOffset = startingPosition;

        foreach (Resource resource in resources)
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
            RectTransform iconRect = iconObject.GetComponent<RectTransform>();
            iconRect.sizeDelta = new Vector2(40, 40);

            GameObject textObject = new GameObject(resource.type + "Text");
            textObject.transform.SetParent(resourceContainer.transform, false);
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.text = "0";
            text.fontSize = 24;
            text.alignment = TextAlignmentOptions.Left;

            resourceTexts.Add(resource.type, text);
            positionOffset += new Vector2(0, -verticalSpacing);
        }
    }

    void GenerateTimerUI()
    {
        GameObject timerObject = new GameObject("TimerText");
        timerObject.transform.SetParent(canvas.transform, false);

        timerText = timerObject.AddComponent<TextMeshProUGUI>();
        timerText.font = eightBitFont; // Assign the 8-bit font
        timerText.fontSize = 45;
        timerText.color = orangeColor;
        timerText.alignment = TextAlignmentOptions.TopLeft;
        timerText.rectTransform.anchorMin = new Vector2(0, 1);
        timerText.rectTransform.anchorMax = new Vector2(0, 1);
        timerText.rectTransform.pivot = new Vector2(0, 1);
        timerText.rectTransform.anchoredPosition = new Vector2(10, -10);
    }

    void GenerateInstructionUI()
    {
        GameObject instructionObject = new GameObject("InstructionText");
        instructionObject.transform.SetParent(canvas.transform, false);

        instructionText = instructionObject.AddComponent<TextMeshProUGUI>();
        instructionText.font = eightBitFont; // Assign the 8-bit font
        instructionText.fontSize = 28;
        instructionText.alignment = TextAlignmentOptions.Bottom;
        instructionText.text = "Collect as many resources as you can!";
        instructionText.color = Color.white;
        instructionText.rectTransform.anchorMin = new Vector2(0.5f, 0);
        instructionText.rectTransform.anchorMax = new Vector2(0.5f, 0);
        instructionText.rectTransform.pivot = new Vector2(0.5f, 0);
        instructionText.rectTransform.anchoredPosition = new Vector2(0, 50);

        StartCoroutine(FadeOutText(instructionText, 3f));
    }

    void UpdateTimer()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(timeRemaining / 60);
            int seconds = Mathf.FloorToInt(timeRemaining % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        else
        {
            timeRemaining = 0;
            timerText.text = "00:00";

            // Save collected resources to the persistent GameState object
            GameState.Instance.SetCollectedResources(
                iron: resources.Find(r => r.type == Asteroid.AsteroidType.Iron)?.amount ?? 0,
                gold: resources.Find(r => r.type == Asteroid.AsteroidType.Gold)?.amount ?? 0,
                tungsten: resources.Find(r => r.type == Asteroid.AsteroidType.Tungsten)?.amount ?? 0
            );

            // Save the game state and load the quiz scene
            GameState.Instance.SaveGameState();
            GameState.Instance.LoadQuizScene();
        }
    }

    IEnumerator FadeOutText(TextMeshProUGUI text, float delay)
    {
        yield return new WaitForSeconds(delay);

        float fadeDuration = 1f;
        Color originalColor = text.color;

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            text.color = Color.Lerp(originalColor, Color.clear, t / fadeDuration);
            yield return null;
        }

        text.color = Color.clear;
    }

    public void CollectResource(Asteroid.AsteroidType resourceType, int amount)
    {
        Debug.Log($"Attempting to collect {amount} of {resourceType}.");

        if (resourceTexts.ContainsKey(resourceType))
        {
            Debug.Log($"{resourceType} exists in resourceTexts.");
            Resource resource = resources.Find(r => r.type == resourceType);
            if (resource != null)
            {
                resource.amount += amount;
                UpdateResourceUI(resourceType);
            }
            else
            {
                Debug.LogWarning($"Resource {resourceType} not found in resources list.");
            }
        }
        else
        {
            Debug.LogWarning($"Resource {resourceType} not found in resourceTexts dictionary.");
        }
    }

    void UpdateResourceUI(Asteroid.AsteroidType resourceType)
    {
        if (resourceTexts.ContainsKey(resourceType))
        {
            resourceTexts[resourceType].text = resources.Find(r => r.type == resourceType).amount.ToString();
        }
        else
        {
            Debug.LogWarning($"Resource {resourceType} not found in resourceTexts when updating UI.");
        }
    }
}
