using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject gameOverPanel;
    // Update is called once per frame
    void Update()
    {
        if (ProgressBarController.gameOver) return;
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            gameOverPanel.SetActive(true);
        }
    }


    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
