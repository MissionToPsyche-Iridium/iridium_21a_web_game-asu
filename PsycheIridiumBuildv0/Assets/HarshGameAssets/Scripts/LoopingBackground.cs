using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopingBackground : MonoBehaviour
{
    public float BackgroundSpeed;
    public Renderer backgroundRenderer;
   

    // Update is called once per frame
    void Update()
    {
        if (ProgressBarController.gameOver) return;
        backgroundRenderer.material.mainTextureOffset += new Vector2(BackgroundSpeed * Time.deltaTime, 0f);
    }
}
