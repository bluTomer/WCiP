using UnityEngine;

public class Planet : MonoBehaviour
{
	public class Evaluation
	{
		public enum Result
		{
			Perfect,
			Good,
			Bad,
		}
		
		public Result[] Results = new Result[Transmission.NUMBER_OF_SIGNALS];
	}
	
	public Transmission.Data GoalTransmission;
	public Transmission.Data LastTransmission;
	public Evaluation LastEvaluation;
	public float Likeness;
	public float RotationSpeed;

	private Vector3 _parentPos;
	
	private void Start()
	{
		_parentPos = transform.parent.position;
	}

	private void Update()
	{
		transform.RotateAround(_parentPos, Vector3.forward, RotationSpeed * Time.deltaTime);
		transform.RotateAround(transform.position, Vector3.forward, RotationSpeed * Time.deltaTime * -1.0f);
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		Debug.Log("Collision on planet");
		var ray = other.gameObject.GetComponent<TransmissionRay>();
		if (ray == null)
		{
			return;
		}
		
		EvaluateTransmission(ray.Transmission);
		Destroy(ray.gameObject);
	}

	private Evaluation EvaluateTransmission(Transmission.Data transmission)
	{
		LastTransmission = transmission;
		var eval = new Evaluation();

		for (int i = 0; i < Transmission.NUMBER_OF_SIGNALS; i++)
		{
			eval.Results[i] = Evaluation.Result.Bad;
		}

		for (int i = 0; i < Transmission.NUMBER_OF_SIGNALS; i++)
		{
			if (transmission.Signals[i] == GoalTransmission.Signals[i])
			{
				// Perfect hit!
				eval.Results[i] = Evaluation.Result.Perfect;
				continue;
			}

			// Check for good hit
			for (int j = 0; j < Transmission.NUMBER_OF_SIGNALS; j++)
			{
				if (transmission.Signals[i] == GoalTransmission.Signals[j])
				{
					// Found good hit
					eval.Results[i] = Evaluation.Result.Good;
					break;
				}
			}
		}

		return eval;
	}
}
