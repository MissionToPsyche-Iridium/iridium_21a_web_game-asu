using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpacecraftController : MonoBehaviour
{
    Rigidbody2D rb;
    public Vector3 center;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move camera to center of mass
        center = rb.worldCenterOfMass;
        Camera.main.transform.position = new(center.x, center.y, Camera.main.transform.position.z);

        Debug.DrawLine(transform.position, rb.worldCenterOfMass, Color.red);
    }
}