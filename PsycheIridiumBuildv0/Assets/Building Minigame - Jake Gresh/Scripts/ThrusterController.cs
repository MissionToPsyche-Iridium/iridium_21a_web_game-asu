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
                SpacecraftController.activeThrusters += 1;
            }
            else
            {
                if (!EditorManager.IsEditMode)
                {
                    audioSource.PlayOneShot(offSound);
                    if (SpacecraftController.activeThrusters > 0)
                        SpacecraftController.activeThrusters -= 1;
                }
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
            // add more thrust if speed is low
            spacecraft.AddForce(transform.up/(spacecraft.velocity.magnitude + 0.1f) + transform.up);
            //spacecraft.AddForce(transform.up * 1f);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        IsOn = !IsOn;
    }
}