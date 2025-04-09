using UnityEngine;

/* Used to loop background music. */
public class bgMusicLoop : MonoBehaviour
{
    // Creates background music, doesn't destroy object when new scene is loaded
    private void Awake()
    {
        GameObject[] bgMusic = GameObject.FindGameObjectsWithTag("music");
        if (bgMusic.Length > 1)
            Destroy(this.gameObject);

        DontDestroyOnLoad(this.gameObject);
    }
}
