using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] GameObject indicator;

    private void Update()
    {
        RectTransform rectTransform = indicator.GetComponent<RectTransform>();
        Vector3 playerPos = player.transform.position;

        rectTransform.localPosition = new Vector3(Mathf.Round(playerPos.x / 5) * 10, Mathf.Round(playerPos.y / 5) * 10, 0);
    }
}
