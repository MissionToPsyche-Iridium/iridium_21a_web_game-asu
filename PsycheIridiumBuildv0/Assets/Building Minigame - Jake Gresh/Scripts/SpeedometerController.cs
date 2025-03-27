using TMPro;
using UnityEngine;

public class SpeedometerController : MonoBehaviour
{
    TMP_Text speedometer;
    Rigidbody2D RB => SpacecraftController.rb;

    // Start is called before the first frame update
    void Start()
    {
        speedometer = GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (EditorManager.IsEditMode)
        {
            speedometer.text = "";
        }
        else
        {
            speedometer.text = $"Speed: {RB.velocity.magnitude:F2} m/s";
        }
    }
}
