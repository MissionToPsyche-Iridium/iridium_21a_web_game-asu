using UnityEngine;
using UnityEngine.EventSystems;

public class ThrusterController : MonoBehaviour, IPointerClickHandler
{
    Rigidbody2D spacecraft;
    [SerializeField] GameObject thrust;
    [SerializeField] GameObject poweredSprite;
    bool _IsOn;

    AudioSource audioSource;
    [SerializeField] AudioClip onSound;
    [SerializeField] AudioClip offSound;

    bool IsOn
    {
        get => _IsOn;
        set
        {
            _IsOn = value;
            thrust.SetActive(value);
            poweredSprite.SetActive(value);
            if (value)
            {
                audioSource.PlayOneShot(onSound);
            }
            else
            {
                if (!EditorManager.IsEditMode)
                    audioSource.PlayOneShot(offSound);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

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