using UnityEngine;
using UnityEngine.SceneManagement;


/* Used to control Menu. */
public class MenuController : MonoBehaviour
{

    // Goes to game scene
    public void Play(int who)
    {
        PlayerPrefs.SetInt("ai", who);
        SceneManager.LoadScene("TanishqGameScene");
    }

    // Quits game and resets all PlayerPrefs
    public void Quit()
    {
        PlayerPrefs.DeleteAll();
        bgMusicLoop.instance.StopMusic();
        SceneManager.LoadScene("Donovan Top Down");
        // Application.Quit();
    }
}