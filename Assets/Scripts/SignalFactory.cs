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

    [SerializeField] private SignalData[] _signals;
    [SerializeField] private Signal _signalPrefab;

    private Dictionary<SignalKey, SignalData> _signalDictionary;

    private void Awake()
    {
        _signalDictionary = new Dictionary<SignalKey, SignalData>();
        foreach (var signalData in _signals)
        {
            _signalDictionary.Add(signalData.Key, signalData);
        }
    }
    
    public Signal CreateSignal(SignalKey key)
    {
        SignalData data;
        if (!_signalDictionary.TryGetValue(key, out data))
        {
            Debug.LogErrorFormat("Couldn't find signal with key [{0}]", key);
            return null;
        }

        var signal = Instantiate(_signalPrefab);
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