using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Transmission : MonoBehaviour
{
    [Serializable]
    public class Data
    {
        public SignalKey[] Signals;

        public Data()
        {
            Signals = new SignalKey[NUMBER_OF_SIGNALS];
        }
    }
    
    public const int NUMBER_OF_SIGNALS = 4;
    public Signal[] Signals { get; private set; }
    public event Action OnSignalChange;

    [SerializeField] private Image[] _signalHolders;
    [SerializeField] private Sprite[] _evaluationSprites;

    private void Awake()
    {
        Signals = new Signal[NUMBER_OF_SIGNALS];
    }

    public bool IsFull()
    {
        for (int i = 0; i < NUMBER_OF_SIGNALS; i++)
        {
            if (Signals[i] == null)
            {
                return false;
            }
        }

        return true;
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

    public void Clear()
    {
        for (int i = 0; i < NUMBER_OF_SIGNALS; i++)
        {
            RemoveSignal(i);
        }

        if (OnSignalChange != null)
        {
            OnSignalChange();
        }
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
        
        if (OnSignalChange != null)
        {
            OnSignalChange();
        }
    }

    public void AddSignal(SignalKey key)
    {
        for (int i = 0; i < NUMBER_OF_SIGNALS; i++)
        {
            if (Signals[i] != null)
            {
                continue;
            }
            
            // Put signal here
            SetupSignal(i, key);
            break;
        }
    }

    public void SetEvaluation(Data transmissionData, Planet.Evaluation evaluation)
    {
        Clear();
        
        for (int i = 0; i < NUMBER_OF_SIGNALS; i++)
        {
            _signalHolders[i].sprite = _evaluationSprites[(int) evaluation.Results[i]];
        }
        foreach (var signal in transmissionData.Signals)
        {
            AddSignal(signal);
        }
    }

    private void SetupSignal(int index, SignalKey key)
    {
        var signal = SignalFactory.CreateSignal(key);
        Signals[index] = signal;
        signal.transform.SetParent(_signalHolders[index].transform, false);
        signal.OnSignalClicked += OnSignalClicked;
         
        if (OnSignalChange != null)
        {
            OnSignalChange();
        }
    }

    private void OnSignalClicked(Signal signal)
    {
        RemoveSignal(signal);
    }
}
