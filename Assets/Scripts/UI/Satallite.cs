using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Satallite : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] private Transform _satalliteDish;
	
	private Camera _camera;

	private void Awake()
	{
		_camera = Camera.main;
	}
	
	public void OnDrag(PointerEventData eventData)
	{
		_satalliteDish.LookAt(_camera.ScreenToWorldPoint(eventData.position), Vector3.forward);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		_satalliteDish.LookAt(_camera.ScreenToWorldPoint(eventData.position), Vector3.forward);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		_satalliteDish.LookAt(_camera.ScreenToWorldPoint(eventData.position), Vector3.forward);
	}
}
