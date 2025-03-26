using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 center;
    Vector3 originalPosition;

    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!EditorManager.IsEditMode)
        {
            // Move camera to center of mass of spacecraft
            center = SpacecraftController.CenterOfMass;
            originalPosition = new Vector3(center.x, center.y, transform.position.z);
        }

        transform.position = originalPosition;
        if (SpacecraftController.rb != null && SpacecraftController.activeThrusters > 0)
            ConstantShake(SpacecraftController.rb.velocity.magnitude * .01f);
    }

    public void Shake(float duration, float magnitude)
    {
        StartCoroutine(TimedShake(duration, magnitude));
    }

    IEnumerator TimedShake(float duration, float magnitude)
    {
        Vector3 originalPosition = transform.position;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.position += new Vector3(x, y, 0);

            elapsed += Time.deltaTime;

            yield return null;
        }

        transform.position = originalPosition;
    }

    void ConstantShake(float magnitude)
    {
        float x = Random.Range(-1f, 1f) * magnitude;
        float y = Random.Range(-1f, 1f) * magnitude;
        transform.position += new Vector3(x, y, 0);
    }
}
