using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 center;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!EditorManager.IsEditMode)
        {
            // Move camera to center of mass of spacecraft
            center = SpacecraftController.CenterOfMass;
            transform.position = new Vector3(center.x, center.y, transform.position.z);
        }
    }
}
