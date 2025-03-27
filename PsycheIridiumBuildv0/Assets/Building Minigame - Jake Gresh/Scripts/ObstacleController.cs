using System.Collections;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    AudioSource audioSource;
    bool isIgnoringCollision = false;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!isIgnoringCollision)
        {
            audioSource.Play();
            Camera.main.gameObject.GetComponent<CameraController>().Shake(0.3f, 0.15f);
            SpacecraftController.damage += 0.2f;

            StartCoroutine(IgnoreCollisionForSeconds(1f));
        }
    }

    IEnumerator IgnoreCollisionForSeconds(float seconds)
    {
        isIgnoringCollision = true;
        yield return new WaitForSeconds(seconds);
        isIgnoringCollision = false;
    }
}