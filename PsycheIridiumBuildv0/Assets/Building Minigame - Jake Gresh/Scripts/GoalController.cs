using UnityEngine;

public class GoalController : MonoBehaviour
{
    static LineRenderer lineRenderer;
    [SerializeField] GameObject spacecraft;

    // Start is called before the first frame update
    void Start()
    {
        // Randomize start position
        transform.position = Random.insideUnitCircle.normalized * 200;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetPosition(1, transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (!EditorManager.IsEditMode)
        {
            lineRenderer.enabled = true;
            lineRenderer.SetPosition(0, spacecraft.GetComponent<SpacecraftController>().center);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Time.timeScale = 0f;
    }
}
