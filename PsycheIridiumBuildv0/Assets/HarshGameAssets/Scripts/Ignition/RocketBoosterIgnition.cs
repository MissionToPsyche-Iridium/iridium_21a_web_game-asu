using System.Collections;
using UnityEngine;

public class RocketBoosterIgnition : MonoBehaviour
{
    public GameObject boosterFlameObject; // GameObject with flame animation
    public float ignitionDelay = 1f;
    public float liftoffDelay = 2f;
    public float liftSpeed = 2f;

    private bool liftOff = false;

    private void Start()
    {
        boosterFlameObject.SetActive(false); // hide flame at start
        StartCoroutine(IgniteAndLaunch());
    }

    private void Update()
    {
        if (liftOff)
        {
            // Move rocket upward
            transform.position += Vector3.up * liftSpeed * Time.deltaTime;
        }
    }

    private IEnumerator IgniteAndLaunch()
    {
        yield return new WaitForSeconds(ignitionDelay);

        // Turn on flame animation
        boosterFlameObject.SetActive(true);

        yield return new WaitForSeconds(liftoffDelay);

        // Start lifting off
        liftOff = true;
    }
}
