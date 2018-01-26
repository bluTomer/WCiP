using UnityEngine;

public class TransmissionRay : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    
    public Transmission.Data Transmission { get; private set; }

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Setup(Transmission.Data transmission)
    {
        Transmission = transmission;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * _movementSpeed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GetComponentInChildren<Collider2D>().enabled = false;
        
        if (other.gameObject.CompareTag("deadzone"))
        {
            GameManager.ControlPanel.EnableUI();
        }
        _animator.SetTrigger("End");
    }

    public void OnFinishedEnding()
    {
        Destroy(gameObject);
    }
}
