using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            audioSource.Play();
            StartCoroutine(SwitchScene());
        }
    }

    IEnumerator SwitchScene()
    {
        yield return new WaitForSecondsRealtime(0.9f);
        // Go to the next scene index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }
}
