using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GoBackToPregame : MonoBehaviour
{
    private Button button;

    private void Awake()
    {
        // Get the Button component attached to this GameObject.
        button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("No Button component found on " + gameObject.name + ". Please attach this script to a GameObject with a Button component.");
        }
    }

    private void Start()
    {
        // Automatically add the GoBack method to the button's onClick event.
        if (button != null)
        {
            button.onClick.AddListener(GoBack);
        }
    }

    private void OnDestroy()
    {
        // Remove the listener when this object is destroyed to prevent potential memory leaks.
        if (button != null)
        {
            button.onClick.RemoveListener(GoBack);
        }
    }

    public void GoBack()
    {
        // Load the "PregameManager" scene when the button is clicked.
        SceneManager.LoadScene("PregameManager");
    }
}
