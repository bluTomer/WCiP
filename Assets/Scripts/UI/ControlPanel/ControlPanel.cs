using System;
using UnityEngine;
using UnityEngine.UI;

public class ControlPanel : MonoBehaviour
{
    [SerializeField] private Button _launchButton;
    [SerializeField] private SignalPanel _signalPanel;
    [SerializeField] private Transmission _currentTransmission;

    public event Action<Transmission.Data> OnLaunchTransmission;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        _launchButton.onClick.AddListener(OnLaunchClicked);
        _signalPanel.OnAddSignal += OnSignalPanelSignalClicked;
        _currentTransmission.OnSignalChange += OnTransmissionChanged;
    }

    private void Start()
    {
        OnTransmissionChanged();
    }

    private void OnDestroy()
    {
        _launchButton.onClick.RemoveAllListeners();
        _signalPanel.OnAddSignal -= OnSignalPanelSignalClicked;
        _currentTransmission.OnSignalChange -= OnTransmissionChanged;
    }

    public void OnLaunchClicked()
    {
        Debug.Log("Launch Clicked");
        if (OnLaunchTransmission != null)
        {
            OnLaunchTransmission(_currentTransmission.GetData());
        }
        
        // Lock and clear until animation done
//        _currentTransmission.Clear();
        _canvasGroup.interactable = false;
    }

    public void EnableUI()
    {
        _canvasGroup.interactable = true;
    }

    private void OnSignalPanelSignalClicked(Signal signal)
    {
        _currentTransmission.AddSignal(signal.Key);
    }

    private void OnTransmissionChanged()
    {
        _launchButton.interactable = _currentTransmission.IsFull();
    }
}
