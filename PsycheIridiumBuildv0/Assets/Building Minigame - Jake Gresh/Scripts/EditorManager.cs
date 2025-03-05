using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class EditorManager : MonoBehaviour
{
    public static bool IsEditMode { get; private set; }

    // Parent object for all parts
    [SerializeField] public GameObject spacecraft;

    [SerializeField] GameObject[] partPrefabs;
    List<KeyCode> partKeyCodes = new();

    static GameObject selectedPrefab;
    static GameObject selectedPart;

    public List<GameObject> placedParts = new();
    public static Dictionary<Vector3, Tuple<KeyCode, Quaternion>> partStorage = PartStorage.partStorage;
    public static KeyCode selectedPartKeyCode;

    HashSet<GameObject> connectedParts = new();
    [SerializeField] GameObject ConnectionAlertText;

    const float gridWidth = 8f;
    const float gridHeight = 4f;

    [SerializeField] GameObject editText;
    [SerializeField] GameObject controlText;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndEditMode();
        }

        if (IsEditMode)
        {
            // Selected part follows cursor
            selectedPart.transform.position = GetMouseGridPosition();

            SelectedPartEffects();

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

            if (placedParts.Count > 0 && !AreAllPartsConnected())
            {
                ConnectionAlertText.SetActive(true);
                return;
            }
            ConnectionAlertText.SetActive(false);
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart();
            }
        }
    }

    void StartEditMode()
    {
        IsEditMode = true;
        Time.timeScale = 1f;
        Physics2D.simulationMode = SimulationMode2D.Script;

        selectedPrefab = partPrefabs[0];
        selectedPart = Instantiate(selectedPrefab, spacecraft.transform);

        selectedPartKeyCode = partKeyCodes[0];

        if (partStorage.Count > 0)
        {
            LoadParts();
        }

        editText.SetActive(true);
        controlText.SetActive(false);
    }

    void LoadParts()
    {
        for (float x = -gridWidth; x <= gridWidth; x++)
        {
            for (float y = -gridHeight; y <= gridHeight; y++)
            {
                Vector3 position = new Vector3(x, y, 0);
                if (partStorage.ContainsKey(position))
                {
                    selectedPrefab = partPrefabs[partKeyCodes.IndexOf(partStorage[position].Item1)];
                    GameObject part = Instantiate(selectedPrefab, spacecraft.transform);
                    part.transform.position = position;
                    part.transform.rotation = partStorage[position].Item2;
                    placedParts.Add(part);
                }
            }
        }
    }

    void EndEditMode()
    {
        if (placedParts.Count == 0 || ConnectionAlertText.activeSelf == true)
        {
            return;
        }

        IsEditMode = false;
        Physics2D.simulationMode = SimulationMode2D.FixedUpdate;

        Destroy(selectedPart);

        spacecraft.GetComponent<SpacecraftController>().enabled = true;
        foreach (ThrusterController thrusterController in spacecraft.GetComponentsInChildren<ThrusterController>())
        {
            thrusterController.enabled = true;
        }

        // Zoom out camera
        Camera.main.orthographicSize = 10f;

        editText.SetActive(false);
        controlText.SetActive(true);
    }

    static Vector3 GetMouseGridPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Dont allow mousePosition to be offscreen
        mousePosition.x = Mathf.Clamp(mousePosition.x, -gridWidth, gridWidth);
        mousePosition.y = Mathf.Clamp(mousePosition.y, -gridHeight, gridHeight);

        return new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0f);
    }
    void PlacePart()
    {
        // Replace the selected position's part with the selected part
        DeletePart();
        GameObject lastPlacedPart = Instantiate(selectedPrefab, spacecraft.transform);
        lastPlacedPart.transform.SetPositionAndRotation(GetMouseGridPosition(), selectedPart.transform.rotation);
        placedParts.Add(lastPlacedPart);

        // Store the part in partStorage
        partStorage[GetMouseGridPosition()] = new Tuple<KeyCode, Quaternion>(selectedPartKeyCode, lastPlacedPart.transform.rotation);
    }
    void DeletePart()
    {
        foreach (GameObject part in placedParts)
        {
            if (part.transform.position == GetMouseGridPosition())
            {
                placedParts.Remove(part);
                partStorage.Remove(GetMouseGridPosition());
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

        selectedPartKeyCode = keyCode;
    }
    static void RotatePart(float degrees)
    {
        selectedPart.transform.Rotate(0f, 0f, degrees);
    }

    public static void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    bool AreAllPartsConnected()
    {
        if (placedParts.Count == 0)
        {
            return false;
        }
        connectedParts.Clear();
        SetConnectedParts(placedParts[0]);
        return connectedParts.SetEquals(placedParts);
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

    // Make selected part transparent and on top
    void SelectedPartEffects()
    {
        // Add selected part's sprites to list
        List<SpriteRenderer> selectedPartSpriteRenderers = new();
        List<SpriteShapeRenderer> selectedPartSpriteShapeRenderers = new();
        foreach (SpriteRenderer spriteRenderer in selectedPart.GetComponents<SpriteRenderer>())
        {
            selectedPartSpriteRenderers.Add(spriteRenderer);
        }
        foreach (SpriteShapeRenderer spriteShapeRenderer in selectedPart.GetComponents<SpriteShapeRenderer>())
        {
            selectedPartSpriteShapeRenderers.Add(spriteShapeRenderer);
        }
        foreach (SpriteRenderer spriteRenderer in selectedPart.GetComponentsInChildren<SpriteRenderer>())
        {
            selectedPartSpriteRenderers.Add(spriteRenderer);
        }
        foreach (SpriteShapeRenderer spriteShapeRenderer in selectedPart.GetComponentsInChildren<SpriteShapeRenderer>())
        {
            selectedPartSpriteShapeRenderers.Add(spriteShapeRenderer);
        }

        // Make all sprites in list transparent and on top
        foreach (SpriteRenderer spriteRenderer in selectedPartSpriteRenderers)
        {
            Color selectedPartColor = spriteRenderer.color;
            selectedPartColor.a = 0.5f;
            spriteRenderer.color = selectedPartColor;
            spriteRenderer.sortingOrder = 1;
        }
        foreach (SpriteShapeRenderer spriteShapeRenderer in selectedPartSpriteShapeRenderers)
        {
            Color selectedPartColor = spriteShapeRenderer.color;
            selectedPartColor.a = 0.5f;
            spriteShapeRenderer.color = selectedPartColor;
            spriteShapeRenderer.sortingOrder = 1;
        }
    }
}