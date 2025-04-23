using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalController : MonoBehaviour
{
    static LineRenderer lineRenderer;

    AudioSource audioSource;

    public static bool isComplete;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Randomize start position
        transform.position = Random.insideUnitCircle.normalized * 200;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(1, transform.position);

        isComplete = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!EditorManager.IsEditMode)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, SpacecraftController.CenterOfMass);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        StartCoroutine(WinGame());
    }

    IEnumerator WinGame()
    {
        if (GameData.instance != null)
            GameData.instance.tardigradeRepaired = true;

        isComplete = true;
        audioSource.Play();
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1f;

        
        SceneManager.LoadScene("Donovan Top Down");
    }
}
