using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameButton : MonoBehaviour
{
    public string sceneToLoad;

    public void LoadNextScene()
    {
        ProgressBarController.gameOver = true;

        // Destroy music
        BackgroundMusic music = FindObjectOfType<BackgroundMusic>();
        if (music != null)
        {
            Destroy(music.gameObject);
        }

        SceneManager.LoadScene(sceneToLoad);
    }

}
