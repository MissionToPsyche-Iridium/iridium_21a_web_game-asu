using UnityEngine;

public class AnimateThrust : MonoBehaviour
{
    [SerializeField] int rotationSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = transform.rotation * Quaternion.Euler(rotationSpeed * Time.deltaTime, 0f, 0f);
    } 
}
