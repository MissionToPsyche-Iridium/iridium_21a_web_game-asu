using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PartIconController : MonoBehaviour
{
    int partIndex;
    EditorManager editorManager;

    // Start is called before the first frame update
    void Start()
    {
        // set partIndex based on object name
        int.TryParse(gameObject.name, out partIndex);
        editorManager = FindObjectOfType<EditorManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!EditorManager.IsEditMode)
        {
            gameObject.SetActive(false);
            return;
        }

        if ((int)EditorManager.selectedPartKeyCode - 48 == partIndex)
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1f);
            GetComponentsInChildren<SpriteRenderer>()[2].color = new Color(1, 1, 1, 1f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
            GetComponentsInChildren<SpriteRenderer>()[2].color = new Color(1, 1, 1, 0.5f);
        }
    }

    void OnMouseDown()
    {
        editorManager.ChoosePart((KeyCode)partIndex + 48);
    }
}
