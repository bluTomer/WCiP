using System;
using UnityEngine;
using UnityEngine.UI;

public class Signal : MonoBehaviour
{
	public event Action<Signal> OnSignalClicked;
	
	public SignalKey Key { get; private set; }

	[SerializeField] private Image _image;
	[SerializeField] private Button _button;

	private AudioSource _audio;

	public void Setup(SignalKey key, Sprite sprite)
	{
		Key = key;
		_image.sprite = sprite;
		_button.onClick.AddListener(OnClick);
		_audio = GetComponent<AudioSource>();
	}

	private void OnDestroy()
	{
		_button.onClick.RemoveAllListeners();
	}

	public void DestroySelf()
	{
		Destroy(gameObject);
	}

	public void OnClick()
	{
		_audio.Play();
		if (OnSignalClicked != null)
		{
			OnSignalClicked(this);
		}
	}
}
