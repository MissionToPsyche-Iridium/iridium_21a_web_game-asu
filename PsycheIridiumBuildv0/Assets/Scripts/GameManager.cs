using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool IsEditMode { get; private set; }

    // Parent object for all parts
    [SerializeField] GameObject spacecraft;

    [SerializeField] GameObject[] partPrefabs;
    List<KeyCode> partKeyCodes = new();
    static GameObject selectedPrefab;
    static GameObject selectedPart;

    HashSet<GameObject> placedParts = new();
    GameObject lastPlacedPart;

    HashSet<GameObject> connectedParts = new();
    [SerializeField] GameObject ConnectionAlertText;

    [SerializeField] Camera mainCamera;


    // Start is called before the first frame update
    void Start()
    {
        // Assign keycodes to each part
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
        mainCamera.transform.position = new Vector3(spacecraft.transform.position.x, spacecraft.transform.position.y, mainCamera.transform.position.z);

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndEditMode();
        }

        if (IsEditMode)
        {
            // Selected part follows cursor
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
        IsEditMode = true;
        Time.timeScale = 0f;
        selectedPrefab = partPrefabs[0];
        selectedPart = Instantiate(selectedPrefab, spacecraft.transform);
    }
    void EndEditMode()
    {
        if (!AreAllPartsConnected())
        {
            ConnectionAlertText.SetActive(true);
            return;
        }
        ConnectionAlertText.SetActive(false);

        IsEditMode = false;
        Time.timeScale = 1f;
        Destroy(selectedPart);
    }

    Vector3 GetMouseGridPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Dont allow mousePosition to be offscreen
        mousePosition.x = Mathf.Clamp(mousePosition.x, -8f, 8f);
        mousePosition.y = Mathf.Clamp(mousePosition.y, -4f, 4f);

        return new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0f);
    }
    void PlacePart()
    {
        // Replace the selected position's part with the selected part
        DeletePart();
        lastPlacedPart = Instantiate(selectedPrefab, spacecraft.transform);
        lastPlacedPart.transform.SetPositionAndRotation(GetMouseGridPosition(), selectedPart.transform.rotation);
        placedParts.Add(lastPlacedPart);
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

    bool AreAllPartsConnected()
    {
        connectedParts.Clear();
        SetConnectedParts(lastPlacedPart);
        return placedParts.SetEquals(connectedParts);
    }
    void SetConnectedParts(GameObject parentPart)
    {
        connectedParts.Add(parentPart);
        foreach (GameObject childPart in placedParts)
        {
            if (Vector3.Distance(parentPart.transform.position, childPart.transform.position) <= 1f)
            {
                if (!connectedParts.Contains(childPart))
                {
                    SetConnectedParts(childPart);
                }
            }
        }
    }

    void LogParts()
    {
        int i = 0;
        foreach (GameObject part in placedParts)
        {
            Debug.Log("placedParts " + i, part);
            i++;
        }

        i = 0;
        foreach (GameObject part in connectedParts)
        {
            Debug.Log("connectedParts " + i, part);
            i++;
        }
    }
}