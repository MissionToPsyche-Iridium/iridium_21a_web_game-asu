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

    // Resource counts
    public int iron = 0;
    public int gold = 0;
    public int tungsten = 0;

    // Purchased instruments
    public bool level2Purchased = false;
    public bool level3Purchased = false;
    public bool level4Purchased = false;

    void Awake()
    {
        // Ensure this object persists across scenes
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadGameState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Save the game state to PlayerPrefs
    public void SaveGameState()
    {
        PlayerPrefs.SetInt("Iron", iron);
        PlayerPrefs.SetInt("Gold", gold);
        PlayerPrefs.SetInt("Tungsten", tungsten);

        PlayerPrefs.SetInt("Level2Purchased", level2Purchased ? 1 : 0);
        PlayerPrefs.SetInt("Level3Purchased", level3Purchased ? 1 : 0);
        PlayerPrefs.SetInt("Level4Purchased", level4Purchased ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("Game state saved.");
    }

    // Load the game state from PlayerPrefs
    public void LoadGameState()
    {
        iron = PlayerPrefs.GetInt("Iron", 0);
        gold = PlayerPrefs.GetInt("Gold", 0);
        tungsten = PlayerPrefs.GetInt("Tungsten", 0);

        level2Purchased = PlayerPrefs.GetInt("Level2Purchased", 0) == 1;
        level3Purchased = PlayerPrefs.GetInt("Level3Purchased", 0) == 1;
        level4Purchased = PlayerPrefs.GetInt("Level4Purchased", 0) == 1;

        Debug.Log("Game state loaded.");
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
}
