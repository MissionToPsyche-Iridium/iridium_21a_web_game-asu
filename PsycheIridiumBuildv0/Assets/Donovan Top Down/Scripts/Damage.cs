using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private GameManager manager;
    [SerializeField] private PlayerController player;

    private bool playerNearby = false;

    private void Start()
    {
        player.Interact += Interacted;
    }

    private void Interacted()
    {
        if (playerNearby)
        {
            manager.FixDamage();
            player.Interact -= Interacted;
            gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        playerNearby = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerNearby = false;
    }
}
