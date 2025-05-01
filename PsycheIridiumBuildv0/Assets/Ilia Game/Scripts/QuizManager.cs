using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;
using System;

public class QuizManager : MonoBehaviour
{
    [System.Serializable]
    public class Resource
    {
        public string name;
        public Sprite icon;
        public int amount;
    }

    public List<Resource> resources = new List<Resource>(); // List of resources with icons and amounts
    public List<Sprite> resourceIcons; // List of icons for resources to assign in the Unity editor
    private Dictionary<string, TextMeshProUGUI> resourceTexts = new Dictionary<string, TextMeshProUGUI>();

    public GameObject canvas; // Canvas for UI
    public TMP_FontAsset eightBitFont; // Font for the UI

    public TextMeshProUGUI questionText; // TextMeshPro for displaying questions
    public Button[] answerButtons; // Buttons for answers

    private List<Question> questions = new List<Question>();
    private int currentQuestionIndex = 0;
    private int questionsAnswered = 0;

    [SerializeField] private GameObject answerButtonsParent;
    [SerializeField] private GameObject nextButton;

    [SerializeField] private AudioClip correctAudio;
    [SerializeField] private AudioClip incorrectAudio;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        LoadResourcesFromGameState();
        InitializeQuestions();
        InitializeResourceDisplay();
        LoadNextQuestion();
    }

    void LoadResourcesFromGameState()
    {
        resources.Clear();

        resources.Add(new Resource
        {
            name = "Iron",
            icon = resourceIcons.Count > 0 ? resourceIcons[0] : null,
            amount = GameState.Instance.collectedIron
        });

        resources.Add(new Resource
        {
            name = "Gold",
            icon = resourceIcons.Count > 1 ? resourceIcons[1] : null,
            amount = GameState.Instance.collectedGold
        });

        resources.Add(new Resource
        {
            name = "Tungsten",
            icon = resourceIcons.Count > 2 ? resourceIcons[2] : null,
            amount = GameState.Instance.collectedTungsten
        });
    }

    void InitializeResourceDisplay()
    {
        Vector2 startingPosition = new Vector2(10, -10);
        float verticalSpacing = 50f;

        foreach (Resource resource in resources)
        {
            GameObject resourceContainer = new GameObject(resource.name + "Container");
            resourceContainer.transform.SetParent(canvas.transform, false);

            RectTransform containerRect = resourceContainer.AddComponent<RectTransform>();
            containerRect.anchorMin = new Vector2(0, 1);
            containerRect.anchorMax = new Vector2(0, 1);
            containerRect.pivot = new Vector2(0, 1);
            containerRect.anchoredPosition = startingPosition;

            HorizontalLayoutGroup layoutGroup = resourceContainer.AddComponent<HorizontalLayoutGroup>();
            layoutGroup.childAlignment = TextAnchor.MiddleLeft;
            layoutGroup.spacing = 10;
            layoutGroup.childControlWidth = false;
            layoutGroup.childControlHeight = false;

            GameObject iconObject = new GameObject(resource.name + "Icon");
            iconObject.transform.SetParent(resourceContainer.transform, false);
            Image iconImage = iconObject.AddComponent<Image>();
            iconImage.sprite = resource.icon;
            RectTransform iconRect = iconObject.GetComponent<RectTransform>();
            iconRect.sizeDelta = new Vector2(40, 40);

            GameObject textObject = new GameObject(resource.name + "Text");
            textObject.transform.SetParent(resourceContainer.transform, false);
            TextMeshProUGUI text = textObject.AddComponent<TextMeshProUGUI>();
            text.text = resource.amount.ToString();
            text.fontSize = 24;
            text.alignment = TextAlignmentOptions.Left;

            resourceTexts.Add(resource.name, text);
            startingPosition += new Vector2(0, -verticalSpacing);
        }
    }

    void InitializeQuestions()
    {
        questions.Add(new Question("What is Psyche's main mission?",
            //new string[] { "Study an asteroid", "Study a comet", "Study Mars", "Study the Moon" }, 0));
            new string[] { "Study Mars", "Study a comet", "Study an asteroid", "Study the Moon" }, 2));
        questions.Add(new Question("What kind of asteroid is Psyche targeting?",
            //new string[] { "Metal-rich", "Ice-rich", "Carbon-rich", "Gas-rich" }, 0));
            new string[] { "Gas-rich", "Ice-rich", "Carbon-rich", "Metal-rich" }, 3));
        questions.Add(new Question("When was Psyche launched?",
            new string[] { "2023", "2021", "2020", "2019" }, 0));
        questions.Add(new Question("What is Psyche made primarily of?",
            //new string[] { "Metal", "Ice", "Rock", "Gas" }, 0));
            new string[] { "Ice", "Metal", "Rock", "Gas" }, 1));
        questions.Add(new Question("How far is Psyche's target asteroid?",
            //new string[] { "280 million miles", "150 million miles", "400 million miles", "500 million miles" }, 0));
            new string[] { "400 million miles", "150 million miles", "280 million miles", "500 million miles" }, 2));
        questions.Add(new Question("Which organization leads Psyche?",
            //new string[] { "NASA", "ESA", "SpaceX", "Blue Origin" }, 0));
            new string[] { "Blue Origin", "ESA", "SpaceX", "NASA" }, 3));
        questions.Add(new Question("What will Psyche study?",
            new string[] { "Asteroid composition", "Moon craters", "Martian soil", "Comet tails" }, 0));
        questions.Add(new Question("What is the mission's goal?",
            //new string[] { "Understand planet formation", "Study black holes", "Explore Europa", "Map the Sun" }, 0));
            new string[] { "Study black holes", "Understand planet formation", "Explore Europa", "Map the Sun" }, 1));
        questions.Add(new Question("How long will the Psyche mission last?",
            //new string[] { "4 years", "2 years", "6 years", "1 year" }, 0));
            new string[] { "6 years", "2 years", "4 years", "1 year" }, 2));
        questions.Add(new Question("What instrument does Psyche use?",
            //new string[] { "Gamma Ray Spectrometer", "Lidar", "X-ray camera", "Optical telescope" }, 0));
            new string[] { "Optical telescope", "Lidar", "X-ray camera", "Gamma Ray Spectrometer" }, 3));
    }

    public void LoadNextQuestion()
    {
        if (questionsAnswered >= 3)
        {
            FinalizeResources();
            SceneManager.LoadScene("Ilia_PregameManager");
            return;
        }

        currentQuestionIndex = UnityEngine.Random.Range(0, questions.Count);
        Question question = questions[currentQuestionIndex];
        questionText.text = question.text;

        for (int i = 0; i < answerButtons.Length; i++)
        {
            int buttonIndex = i; // Capture index for lambda
            answerButtons[i].GetComponentInChildren<TextMeshProUGUI>().text = question.answers[i];
            answerButtons[i].onClick.RemoveAllListeners();
            answerButtons[i].onClick.AddListener(() => OnAnswerSelected(buttonIndex));
        }

        nextButton.SetActive(false);
        answerButtonsParent.SetActive(true);
    }

    void OnAnswerSelected(int selectedIndex)
    {
        Question question = questions[currentQuestionIndex];
        if (selectedIndex == question.correctAnswerIndex)
        {
            Debug.Log("Correct answer!");
            audioSource.PlayOneShot(correctAudio);
            questionText.text = "CORRECT!\nGained 10% resources!";
            UpdateResources(1.1f); // Increase by 10%
        }
        else
        {
            Debug.Log("Incorrect answer.");
            audioSource.PlayOneShot(incorrectAudio);
            questionText.text = "INCORRECT!\nLost 5% resources!";
            UpdateResources(0.95f); // Decrease by 5%
        }

        questionsAnswered++;
        answerButtonsParent.SetActive(false);
        nextButton.SetActive(true);
    }

    void UpdateResources(float multiplier)
    {
        GameState.Instance.collectedIron = (int)Math.Round(GameState.Instance.collectedIron * multiplier);
        GameState.Instance.collectedGold = (int)Math.Round(GameState.Instance.collectedGold * multiplier);
        GameState.Instance.collectedTungsten = (int)Math.Round(GameState.Instance.collectedTungsten * multiplier);

        foreach (var resource in resources)
        {
            int updatedAmount = Mathf.RoundToInt(resource.amount * multiplier);
            resource.amount = updatedAmount;
            resourceTexts[resource.name].text = updatedAmount.ToString();
        }
    }

    void FinalizeResources()
    {
        GameState.Instance.AddCollectedToTotal();
    }

    class Question
    {
        public string text;
        public string[] answers;
        public int correctAnswerIndex;

        public Question(string text, string[] answers, int correctAnswerIndex)
        {
            this.text = text;
            this.answers = answers;
            this.correctAnswerIndex = correctAnswerIndex;
        }
    }
}
