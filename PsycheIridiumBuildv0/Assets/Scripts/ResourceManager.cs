using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ResourceManager : MonoBehaviour
{
    public int ironCollected = 0;
    public int goldCollected = 0;
    public int tungstenCollected = 0;

    public TextMeshProUGUI ironText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI tungstenText;

    public void CollectResource(string asteroidType, int amount)
    {
        if (asteroidType == "Iron")
            ironCollected += amount;
        else if (asteroidType == "Gold")
            goldCollected += amount;
        else if (asteroidType == "Tungsten")
            tungstenCollected += amount;

        UpdateUI();
    }

    void UpdateUI()
    {
        ironText.text = "Iron: " + ironCollected;
        goldText.text = "Gold: " + goldCollected;
        tungstenText.text = "Tungsten: " + tungstenCollected;
    }
}
