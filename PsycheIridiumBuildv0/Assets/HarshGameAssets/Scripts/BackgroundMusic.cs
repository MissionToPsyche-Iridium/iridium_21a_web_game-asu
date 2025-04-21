using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic backgroundMusic;
    void Awake()
    {
        if (ProgressBarController.gameOver) return;
        if (backgroundMusic == null)
        {
            backgroundMusic = this;
            DontDestroyOnLoad(backgroundMusic);
        } else
        {
            Destroy(gameObject);
        }
    }
    public void DestroyMusic()
    {
        Destroy(gameObject);
    }

}
