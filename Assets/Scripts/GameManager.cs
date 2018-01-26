using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;

    [SerializeField] private WorldController _worldController;
    public static WorldController WorldController { get { return _instance._worldController; }}

    [SerializeField] private ControlPanel _controlPanel;
    public static ControlPanel ControlPanel { get { return _instance._controlPanel; }}

    [SerializeField] private AlienPanel _alienPanel;
    public static AlienPanel AlienPanel { get { return _instance._alienPanel; }}

    private void Awake()
    {
        _instance = this;
    }
}
