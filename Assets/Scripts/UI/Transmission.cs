using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Transmission : MonoBehaviour
{
    public class Data
    {
        public SignalKey[] Signals;
    }
    
    public const int NUMBER_OF_SIGNALS = 4;
    public Signal[] Signals { get; private set; }

    [SerializeField] private Transform[] SignalHolders;

    private void Awake()
    {
        Signals = new Signal[NUMBER_OF_SIGNALS];
    }

    public Data GetData()
    {
        var data = new Data { Signals = new SignalKey[NUMBER_OF_SIGNALS] };
        
        for (int i = 0; i < NUMBER_OF_SIGNALS; i++)
        {
            data.Signals[i] = Signals[i].Key;
        }

        return data;
    }
    
    public void RemoveSignal(int index)
    {
        if (Signals[index] != null)
        {
            RemoveSignal(Signals[index]);
        }
    }

    public void RemoveSignal(Signal signal)
    {
        var index = Signals.ToList().FindIndex(signal1 => signal == signal1);
        signal.DestroySelf();
        Signals[index] = null;
    }

    public bool AddSignal(SignalKey key)
    {
        for (int i = 0; i < NUMBER_OF_SIGNALS; i++)
        {
            if (Signals[i] == null)
            {
                // Put signal here
                SetupSignal(i, key);
                return true;
            }
        }

        return false;
    }

    public void CopyTransmission(Transmission destinationTransmission)
    {
        for (int i = 0; i < NUMBER_OF_SIGNALS; i++)
        {
            destinationTransmission.AddSignal(Signals[i].Key);
        }
    }

    private void SetupSignal(int index, SignalKey key)
    {
        var signal = SignalFactory.CreateSignal(key);
        Signals[index] = signal;
        signal.transform.SetParent(SignalHolders[index], false);
        signal.OnSignalClicked += OnSignalClicked;
        // Maybe setup rect transform here
    }

    private void OnSignalClicked(Signal signal)
    {
        RemoveSignal(signal);
    }
}
