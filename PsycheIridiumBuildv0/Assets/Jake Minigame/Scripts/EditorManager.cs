using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;

public class EditorManager : MonoBehaviour
{
    public static bool IsEditMode { get; private set; }

    // Parent object for all parts
    [SerializeField] public GameObject spacecraft;

    [SerializeField] GameObject[] partPrefabs;
    readonly List<KeyCode> partKeyCodes = new();

    static GameObject selectedPrefab;
    static GameObject selectedPart;

    public List<GameObject> placedParts = new();
    public static Dictionary<Vector3, Tuple<KeyCode, Quaternion>> partStorage = PartStorage.partStorage;
    public static KeyCode selectedPartKeyCode;

    readonly HashSet<GameObject> connectedParts = new();
    [SerializeField] GameObject connectionAlertText;
    [SerializeField] GameObject noThrusterAlertText;
    [SerializeField] GameObject damageAlertText;
    [SerializeField] TMP_Text selectedPartText;

    [SerializeField] String[] partNames;

    const float gridWidth = 8f;
    const float gridTop = 4f;
    const float gridBottom = -3f;

    [SerializeField] GameObject editText;
    [SerializeField] GameObject controlText;

    AudioSource[] audioSource;

    static bool canPlace;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponents<AudioSource>();

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
        if (IsEditMode)
        {
            // Selected part follows cursor
            selectedPart.transform.position = GetMouseGridPosition();

            SelectedPartEffects();

            if (Input.GetMouseButtonDown(0))
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

            if (Input.GetKeyDown(KeyCode.Space))
            {
                EndEditMode();
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
        else
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                Restart(false);
            }
        }
    }

    void StartEditMode()
    {
        IsEditMode = true;
        Time.timeScale = 1f;
        Physics2D.simulationMode = SimulationMode2D.Script;

        if (partStorage.Count > 0)
        {
            LoadParts();
        }

        selectedPrefab = partPrefabs[0];
        selectedPart = Instantiate(selectedPrefab, spacecraft.transform);

        selectedPartKeyCode = partKeyCodes[0];

        editText.SetActive(true);
        controlText.SetActive(false);
        UpdateSelectedPartText();
    }

    void LoadParts()
    {
        for (float x = -gridWidth; x <= gridWidth; x++)
        {
            for (float y = gridBottom; y <= gridTop; y++)
            {
                Vector3 position = new(x, y, 0);
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
        connectionAlertText.SetActive(false);
        damageAlertText.SetActive(false);

        if (!CheckForThruster(true))
        {
            return;
        }

        // check for connection
        if (placedParts.Count == 0 || !AreAllPartsConnected())
        {
            connectionAlertText.SetActive(true);
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
        UpdateSelectedPartText();
    }

    void UpdateSelectedPartText()
    {
        selectedPartText.text = "Selected Part: " + partNames[(int)(selectedPartKeyCode - 48)];
        selectedPartText.gameObject.SetActive(IsEditMode);
    }

    static Vector3 GetMouseGridPosition()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 gridPosition = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0f);

        canPlace = true;
        if (Math.Abs(gridPosition.x) > gridWidth || gridPosition.y > gridTop || gridPosition.y < gridBottom)
        {
            canPlace = false;
            gridPosition.x = Mathf.Clamp(gridPosition.x, -gridWidth, gridWidth);
            gridPosition.y = Mathf.Clamp(gridPosition.y, gridBottom, gridTop);
        }

        return gridPosition;
    }
    void PlacePart()
    {
        if (!canPlace)
        {
            return;
        }

        // Replace the selected position's part with the selected part
        if (!DeletePart())
        {
            PlayBuildSound();
        }
        GameObject lastPlacedPart = Instantiate(selectedPrefab, spacecraft.transform);
        lastPlacedPart.transform.SetPositionAndRotation(GetMouseGridPosition(), selectedPart.transform.rotation);
        placedParts.Add(lastPlacedPart);

        // Store the part in partStorage
        partStorage[GetMouseGridPosition()] = new Tuple<KeyCode, Quaternion>(selectedPartKeyCode, lastPlacedPart.transform.rotation);

        // Remove thruster alert if thruster is placed
        CheckForThruster(false);
    }
    void PlayBuildSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, audioSource.Length);
        audioSource[randomIndex].pitch = UnityEngine.Random.Range(1.3f, 1.5f);
        audioSource[randomIndex].Play();
    }
    bool DeletePart()
    {
        foreach (GameObject part in placedParts)
        {
            if (part.transform.position == GetMouseGridPosition())
            {
                placedParts.Remove(part);
                partStorage.Remove(GetMouseGridPosition());
                Destroy(part);
                PlayDeleteSound();
                return true;
            }
        }
        return false;
    }
    void PlayDeleteSound()
    {
        int randomIndex = UnityEngine.Random.Range(0, audioSource.Length);
        audioSource[randomIndex].pitch = UnityEngine.Random.Range(1.5f, 1.6f);
        audioSource[randomIndex].Play();
    }
    public void ChoosePart(KeyCode keyCode)
    {
        selectedPrefab = partPrefabs[partKeyCodes.IndexOf(keyCode)];
        Destroy(selectedPart);
        selectedPart = Instantiate(selectedPrefab, spacecraft.transform);
        selectedPart.transform.position = GetMouseGridPosition();

        selectedPartKeyCode = keyCode;

        UpdateSelectedPartText();
    }
    static void RotatePart(float degrees)
    {
        selectedPart.transform.Rotate(0f, 0f, degrees);
    }

    public static void Restart(bool fromDamage)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        if (fromDamage)
        {
            FindObjectOfType<EditorManager>().damageAlertText.SetActive(true);
        }
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

    bool CheckForThruster(bool causeAlert)
    {
        // Check for no thruster in partstorage
        if (!partStorage.Values.Any(tuple => tuple.Item1 == partKeyCodes[1]))
        {
            if (causeAlert)
            {
                noThrusterAlertText.SetActive(true);
            }
            return false;
        }
        noThrusterAlertText.SetActive(false);
        return true;
    }
}