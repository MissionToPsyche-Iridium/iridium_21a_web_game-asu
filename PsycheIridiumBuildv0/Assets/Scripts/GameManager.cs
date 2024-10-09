using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    bool isEditMode;

    GameObject selectedPart;
    List<GameObject> partPrefabs = new();
    List<Vector3> partPositions = new();
    [SerializeField] GameObject part1Prefab;
    [SerializeField] GameObject part2Prefab;
    [SerializeField] GameObject spacecraft;

    List<KeyCode> keyCodes = new();

    // Start is called before the first frame update
    void Start()
    {
        partPrefabs.Add(part1Prefab);
        partPrefabs.Add(part2Prefab);

        keyCodes.Add(KeyCode.Alpha1);
        keyCodes.Add(KeyCode.Alpha2);

        isEditMode = true;
        selectedPart = Instantiate(part1Prefab, spacecraft.transform);
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        // End edit mode
        if (Input.GetKeyDown(KeyCode.Space))
        {
            isEditMode = false;
            Time.timeScale = 1f;
            Destroy(selectedPart);
        }

        if (isEditMode)
        {
            // Move the selected object to the mouse position
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 gridPosition = new Vector3(Mathf.Round(mousePosition.x), Mathf.Round(mousePosition.y), 0f);
            selectedPart.transform.position = gridPosition;

            // Place the selected object
            if (Input.GetMouseButtonDown(0) && !partPositions.Contains(gridPosition))
            {
                selectedPart = Instantiate(part1Prefab, spacecraft.transform);
                partPositions.Add(gridPosition);
            }

            ChangeSelectedPart();
            RotateSelectedPart();

    
            Debug.Log(isEditMode + " " + Input.mousePosition + " s2w " + selectedPart.transform.position);
        }
    }

    void ChangeSelectedPart()
    {
        foreach (KeyCode keyCode in keyCodes)
        {
            if (Input.GetKeyDown(keyCode))
            {
                Destroy(selectedPart);
                selectedPart = Instantiate(partPrefabs[keyCodes.IndexOf(keyCode)], spacecraft.transform);
            }
        }
    }

    void RotateSelectedPart()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            selectedPart.transform.Rotate(0f, 0f, -90f);
        }
    }
}