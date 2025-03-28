using TMPro;
using UnityEngine;

public class SpacecraftController : MonoBehaviour
{
    public static Rigidbody2D rb;
    public static Vector3 CenterOfMass => 
        rb == null ? 
            Vector3.zero : 
            rb.worldCenterOfMass;

    static LineRenderer velocityIndicatorLine;
    [SerializeField] float velocityLineOffset = 1f;
    [SerializeField] GameObject velocityIndicatorTip;

    [SerializeField] UnityEngine.UI.Slider damageBar;
    [SerializeField] TMP_Text damageText;
    public static float damage;

    public static int activeThrusters;
    
    [SerializeField] GameObject damageAlertText;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        velocityIndicatorLine = GetComponent<LineRenderer>();

        damage = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(activeThrusters);

        // Update velocity indicator arrow
        if (EditorManager.IsEditMode || rb.velocity.magnitude == 0)
        {
            velocityIndicatorLine.enabled = false;

            velocityIndicatorTip.SetActive(false);
        }
        else
        {
            velocityIndicatorLine.enabled = true;
            velocityIndicatorLine.SetPosition(0, CenterOfMass);
            float lineLength = rb.velocity.magnitude - velocityLineOffset;
            Vector2 lineEnd = rb.velocity.normalized * lineLength;
            velocityIndicatorLine.SetPosition(1, CenterOfMass + new Vector3(lineEnd.x, lineEnd.y, 0f));

            velocityIndicatorTip.SetActive(true);
            velocityIndicatorTip.transform.SetPositionAndRotation(
                CenterOfMass + new Vector3(rb.velocity.x, rb.velocity.y, 0f),
                Quaternion.Euler(0f, 0f, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg - 90f));
        }

        // Update damage bar
        damageText.gameObject.SetActive(!EditorManager.IsEditMode);
        damageBar.value = damage;
        damageText.text = $"Damage Sustained: {(int)(damage * 100)}%";
        if (damage >= 1f)
        {
            Debug.Log("Game Over");
            EditorManager.Restart(true);
        }
    }
}
