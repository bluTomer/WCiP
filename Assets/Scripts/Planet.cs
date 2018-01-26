using UnityEngine;

public class Planet : MonoBehaviour
{
	public float RotationSpeed;

	private Vector3 _parentPos;
	
	private void Start()
	{
		_parentPos = transform.parent.position;
	}

	private void Update()
	{
		transform.RotateAround(_parentPos, Vector3.forward, RotationSpeed * Time.deltaTime);
		transform.RotateAround(transform.position, Vector3.forward, (30.0f - RotationSpeed) * Time.deltaTime);
	}
}
