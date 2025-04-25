using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float cameraSpeed;

    // Update is called once per frame
    void Update()
    {
        if (ProgressBarController.gameOver) return;
        transform.position += new Vector3(cameraSpeed * Time.deltaTime, 0, 0);
    }
}
