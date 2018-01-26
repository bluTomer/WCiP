using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransmissionRay : MonoBehaviour
{
    [SerializeField] private float _movementSpeed;
    
    public Transmission.Data Transmission { get; private set; }

    public void Setup(Transmission.Data transmission)
    {
        Transmission = transmission;
    }

    private void Update()
    {
        transform.Translate(Vector3.up * _movementSpeed * Time.deltaTime);
    }
}
