using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameManager manager;
    [SerializeField] private PlayerController player;

    public int damageIndex = -1;

    private bool playerNearby = false;
    private GameObject buttonPromptCanvas;

    private void Start()
    {
        player.Interact += Interacted;
        buttonPromptCanvas = player.gameObject.transform.GetChild(1).gameObject;
    }

    public void DamageInit(int index)
    {
        damageIndex = index;
        if (GameData.instance.repairedDamageObjects[index]) gameObject.SetActive(false);
    }

    private void Interacted()
    {
        if (playerNearby)
        {
            manager.FixDamage();
            player.Interact -= Interacted;
            GameData.instance.repairedDamageObjects[damageIndex] = true;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerNearby = true;
        buttonPromptCanvas.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerNearby = false;
        buttonPromptCanvas.SetActive(false);
    }
}
