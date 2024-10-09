using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool EditMode { get; private set; }

    // Parent object for all parts
    [SerializeField] GameObject spacecraft;

    [SerializeField] GameObject[] partPrefabs;
    List<KeyCode> partKeyCodes = new();
    static GameObject selectedPrefab;
    static GameObject selectedPart;

    List<GameObject> placedParts = new();


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < partPrefabs.Length; i++)
        {
            partKeyCodes.Add(KeyCode.Alpha1 + i);
        }

        Physics2D.gravity = Vector2.zero;

        StartEditMode();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndEditMode();
        }

        if (EditMode)
        {
            selectedPart.transform.position = GetMouseGridPosition();

            if (Input.GetMouseButton(0))
            {
                PlacePart(); 
            }

            if (Input.GetMouseButton(1))
            {
                DeletePart();
            }

            foreach (KeyCode keyCode in partKeyCodes)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    ChoosePart(keyCode);
                }
            }

            if (Input.GetKeyDown(KeyCode.Q))
            {
                RotatePart(90f);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                RotatePart(-90f);
            }
        }
    }

    void StartEditMode()
    {
        EditMode = true;
        Time.timeScale = 0f;
        selectedPrefab = partPrefabs[0];
        selectedPart = Instantiate(selectedPrefab, spacecraft.transform);
    }
    void EndEditMode()
    {
        EditMode = false;
        Time.timeScale = 1f;
        Destroy(selectedPart);
    }
    Vector3 GetMouseGridPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        return new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0f);
    }
    void PlacePart()
    {
        DeletePart(); // Clears the position of any existing part
        GameObject placedPart = Instantiate(selectedPrefab, spacecraft.transform);
        placedPart.transform.position = GetMouseGridPosition();
        placedPart.transform.rotation = selectedPart.transform.rotation;
        placedParts.Add(placedPart);
    }
    void DeletePart()
    {
        foreach (GameObject part in placedParts)
        {
            if (part.transform.position == GetMouseGridPosition())
            {
                placedParts.Remove(part);
                Destroy(part);
                return;
            }
        }
    }
    void ChoosePart(KeyCode keyCode)
    {
        selectedPrefab = partPrefabs[partKeyCodes.IndexOf(keyCode)];
        Destroy(selectedPart);
        selectedPart = Instantiate(selectedPrefab, spacecraft.transform);
        selectedPart.transform.position = GetMouseGridPosition();
    }
    void RotatePart(float degrees)
    {
        selectedPart.transform.Rotate(0f, 0f, degrees);
    }
    void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}