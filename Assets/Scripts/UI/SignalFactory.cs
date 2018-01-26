using System;
using System.Collections.Generic;
using UnityEngine;

public class SignalFactory : MonoBehaviour 
{
    [Serializable]
    public struct SignalData
    {
        public SignalKey Key;
        public Sprite Sprite;
    }

    private static SignalFactory _instance; 

    [SerializeField] private SignalData[] _signals;
    [SerializeField] private Signal _signalPrefab;

    private Dictionary<SignalKey, SignalData> _signalDictionary;

    private void Awake()
    {
        _instance = this;
        
        _signalDictionary = new Dictionary<SignalKey, SignalData>();
        foreach (var signalData in _signals)
        {
            _signalDictionary.Add(signalData.Key, signalData);
        }
    }

    public static List<Signal> CreateAllSignals()
    {
        var result = new List<Signal>();
        foreach (var signal in _instance._signals)
        {
            result.Add(CreateSignal(signal.Key));
        }

        return result;
    }
    
    public static Signal CreateSignal(SignalKey key)
    {
        SignalData data;
        if (!_instance._signalDictionary.TryGetValue(key, out data))
        {
            Debug.LogErrorFormat("Couldn't find signal with key [{0}]", key);
            return null;
        }

        var signal = Instantiate(_instance._signalPrefab);
        signal.gameObject.name = key.ToString();
        signal.Setup(key, data.Sprite);
        return signal;
    }
}

public enum SignalKey
{
    Nature,
    Music,
    Money,
    Physics,
    Chemistry,
    Food,
    Love,
    Wealth,
    War,
    Death,
}