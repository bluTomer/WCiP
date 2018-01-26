using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldController : MonoBehaviour
{
    [SerializeField] private Planet[] _planets;
    [SerializeField] private Transform _firingPoint;
    [SerializeField] private TransmissionRay _rayPrefab;

    private void Start()
    {
        GameManager.ControlPanel.OnLaunchTransmission += ShootTransmission;
    }

    public void ShootTransmission(Transmission.Data transmission)
    {
        var ray = CreateTransmissionRay(transmission);
    }

    private TransmissionRay CreateTransmissionRay(Transmission.Data transmission)
    {
        var ray = Instantiate(_rayPrefab);
        ray.transform.position = _firingPoint.position;
        ray.transform.rotation = _firingPoint.rotation;
        ray.Setup(transmission);
        return ray;
    }
}
