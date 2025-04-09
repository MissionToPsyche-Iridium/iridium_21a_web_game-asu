using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Obstacle : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update

    private ProgressBarController progressBarController; //Reference to Pbc

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        progressBarController = FindObjectOfType<ProgressBarController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Border")
        {
            Destroy(this.gameObject);
        } 
        else if(collision.tag == "Player")
        {
            if (progressBarController != null)
            {
                progressBarController.StopProgress(); // Stop the progress bar when hitting the player
            }
            Destroy(player.gameObject);
        }
    }
}
