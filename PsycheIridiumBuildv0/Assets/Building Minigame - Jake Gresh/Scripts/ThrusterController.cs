using UnityEngine;
using UnityEngine.EventSystems;

public class ThrusterController : MonoBehaviour, IPointerClickHandler
{
    Rigidbody2D spacecraft;
    [SerializeField] GameObject thrust;
    bool _IsOn;
    bool IsOn
    {
        get => _IsOn;
        set
        {
            _IsOn = value;
            thrust.SetActive(value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        spacecraft = GetComponentInParent<Rigidbody2D>();
        IsOn = false;
    }

    // FixedUpdate is called once per physics frame
    void FixedUpdate()
    {
        if (IsOn)
        {
            spacecraft.AddForce(transform.up * 1f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsOn = !IsOn;
    }
}