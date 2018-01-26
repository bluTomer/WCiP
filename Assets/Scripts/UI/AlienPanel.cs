using System;
using UnityEngine;
using UnityEngine.UI;

public class AlienPanel : MonoBehaviour
{
    [SerializeField] private Image _alienImage;
    [SerializeField] private Transmission _Evaluation;
    [SerializeField] private Slider _likenessMeter;

    private Animator _animator;
    private Action _callback;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Setup(Sprite alienSprite, Transmission.Data transmission, Planet.Evaluation evaluation, float likeness)
    {
        _alienImage.sprite = alienSprite;
        _Evaluation.SetEvaluation(transmission, evaluation);
        _likenessMeter.value = likeness;
    }

    public void Show(Action callback)
    {
        _callback = callback;
        _animator.SetTrigger("Show");
    }

    public void OnShowComplete()
    {
        if (_callback != null)
        {
            _callback();
        }
    }
}
