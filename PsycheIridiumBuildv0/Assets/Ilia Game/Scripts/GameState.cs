using System;
using UnityEngine;

public class GameState : MonoBehaviour
{
    // Singleton instance
    public static GameState Instance;

    public enum Level
    {
        None,
        Level1,
        Level2,
        Level3,
        Level4
    }

    public Level currentLevel = Level.None;

    // Total resources (across all levels)
    public int iron = 0;
    public int gold = 0;
    public int tungsten = 0;

    // Resources collected in the current level
    public int collectedIron = 0;
    public int collectedGold = 0;
    public int collectedTungsten = 0;

    // Purchased instruments
    public bool level2Purchased = false;
    public bool level3Purchased = false;
    public bool level4Purchased = false;

    public int maxAsteroids = 0;
    public float asteroidInterval = 0;

    void Awake()
    {
        // Ensure this object persists across scenes
        if (Instance == null)
        {
            Debug.Log("Instantiating GameState");
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Check if a level is unlocked
    public bool IsLevelUnlocked(Level level)
    {
        switch (level)
        {
            case Level.Level1: return true; // Level 1 is always unlocked
            case Level.Level2: return level2Purchased;
            case Level.Level3: return level3Purchased;
            case Level.Level4: return level4Purchased;
            default: return false;
        }
    }

    // Set resources collected in the current level
    public void SetCollectedResources(int iron, int gold, int tungsten)
    {
        collectedIron = iron;
        collectedGold = gold;
        collectedTungsten = tungsten;

        Debug.Log($"Collected resources set: Iron={collectedIron}, Gold={collectedGold}, Tungsten={collectedTungsten}");
    }

    // Add collected resources to total and reset collected values
    public void AddCollectedToTotal()
    {
        Debug.Log(iron);
        Debug.Log(collectedIron);
        iron += collectedIron;
        gold += collectedGold;
        tungsten += collectedTungsten;
        Debug.Log(iron);

        collectedIron = 0;
        collectedGold = 0;
        collectedTungsten = 0;

        Debug.Log($"Added collected resources to total: Iron={iron}, Gold={gold}, Tungsten={tungsten}");
    }

    // Transition to the quiz scene
    public void LoadQuizScene()
    {
        Debug.Log("Loading Quiz Scene...");
        UnityEngine.SceneManagement.SceneManager.LoadScene("Ilia_QuizScene");
    }

    internal object GetResources()
    {
        throw new NotImplementedException();
    }
}
