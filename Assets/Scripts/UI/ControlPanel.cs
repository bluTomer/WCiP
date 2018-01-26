using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private Button _launchButton;
    [SerializeField] private SignalPanel _signalPanel;
    [SerializeField] private Transmission _currentTransmission;

    public event Action<Transmission.Data> OnLaunchTransmission;

    private void Awake()
    {
        _launchButton.onClick.AddListener(OnLaunchClicked);
        _signalPanel.OnAddSignal += OnSignalPanelSignalClicked;
    }

    private void OnDestroy()
    {
        _launchButton.onClick.RemoveAllListeners();
        _signalPanel.OnAddSignal -= OnSignalPanelSignalClicked;
    }

    public void OnLaunchClicked()
    {
        Debug.Log("Launch Clicked");
        if (OnLaunchTransmission != null)
        {
            OnLaunchTransmission(_currentTransmission.GetData());
        }
        _currentTransmission.Clear();
    }

    public void OnSignalPanelSignalClicked(Signal signal)
    {
        _currentTransmission.AddSignal(signal.Key);
    }
}
