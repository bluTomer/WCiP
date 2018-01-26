using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SignalPanel : MonoBehaviour
{
    public event Action<Signal> OnAddSignal;

    [SerializeField] private Transform _signalsParent;

    private List<Signal> _signals;

    private void Start()
    {
        InitSignals();
    }

    private void OnDestroy()
    {
        foreach (var signal in _signals)
        {
            signal.OnSignalClicked -= OnSignalClicked;
        }
    }

    private void InitSignals()
    {
        _signals = SignalFactory.CreateAllSignals();

        foreach (var signal in _signals)
        {
            SetupSignal(signal);
        }
    }
    
    private void SetupSignal(Signal signal)
    {
        signal.transform.SetParent(_signalsParent);
        signal.transform.localScale = Vector3.one;
        signal.OnSignalClicked += OnSignalClicked;
        // Maybe adjust rect transform here
    }

    public void OnSignalClicked(Signal signal)
    {
        Debug.LogFormat("Signal Panel signal {0} clicked", signal.Key);
        if (OnAddSignal != null)
        {
            OnAddSignal(signal);
        }
    }
}
