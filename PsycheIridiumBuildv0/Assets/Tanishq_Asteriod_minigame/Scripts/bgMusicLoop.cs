using UnityEngine;

/* Used to loop background music. */
public class bgMusicLoop : MonoBehaviour
{
    public static bgMusicLoop instance;

    // Creates background music, doesn't destroy object when new scene is loaded
    private void Awake()
    {
        //GameObject[] bgMusic = GameObject.FindGameObjectsWithTag("music");
        //if (bgMusic.Length > 1)
        //    Destroy(this.gameObject);

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(this.gameObject);
    }

    public void StopMusic()
    {
        Destroy(this.gameObject);
    }
}
