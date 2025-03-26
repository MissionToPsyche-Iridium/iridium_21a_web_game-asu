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
            //EditorManager.Restart();
            audioSource.Play();
            SpacecraftController.damage += 0.1f;
            StartCoroutine(IgnoreCollisionForSeconds(1f));
            //Camera.main.gameObject.GetComponent<CameraController>().CameraShake();
        }
    }

    IEnumerator IgnoreCollisionForSeconds(float seconds)
    {
        isIgnoringCollision = true;
        yield return new WaitForSeconds(seconds);
        isIgnoringCollision = false;
    }
}