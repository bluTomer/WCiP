using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Planet : MonoBehaviour
{
	public class Evaluation
	{
		public enum Result
		{
			Perfect = 0,
			Good = 1,
			Bad = 2,
		}

		public float Score = 0;
		public Result[] Results = new Result[Transmission.NUMBER_OF_SIGNALS];
	}
	
	public float RotationSpeed;
	public float StepsToFailure = 8;
	public Sprite AlienSpriteGood;
	public Sprite AlienSpriteBad;
	public Sprite AlienSpriteNeutral;

	private Transmission.Data _goalTransmission;
	private Transmission.Data _lastTransmission;
	private Evaluation _lastEvaluation;
	private Vector3 _parentPos;
	private float _likeness;
	private float _angerMod;
	
	private void Start()
	{
		_parentPos = transform.parent.position;
		RandomizeGoal();
		_likeness = 1.0f;
		_angerMod = 1.0f / StepsToFailure;
	}
	
	private void Update()
	{
		transform.RotateAround(_parentPos, Vector3.forward, RotationSpeed * Time.deltaTime);
		transform.RotateAround(transform.position, Vector3.forward, RotationSpeed * Time.deltaTime * -1.0f);
	}

	private void RandomizeGoal()
	{
		_goalTransmission = new Transmission.Data();
		for (int i = 0; i < Transmission.NUMBER_OF_SIGNALS; i++)
		{
			var enumValues = Enum.GetValues(typeof(SignalKey));
			var randomValue = (SignalKey)enumValues.GetValue(Random.Range(0, enumValues.Length));
			_goalTransmission.Signals[i] = randomValue;
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		var ray = other.gameObject.GetComponent<TransmissionRay>();
		if (ray == null)
		{
			return;
		}
		
		_lastTransmission = ray.Transmission;
		_lastEvaluation = EvaluateTransmission(ray.Transmission);

		if (_lastEvaluation.Score < 0.0f)
		{
			_likeness -= 0.1f;
		}
		
		GameManager.AlienPanel.Setup(GetAlienSprite(_lastEvaluation), _lastTransmission, _lastEvaluation, _likeness);
		GameManager.AlienPanel.Show(OnAlienPanelDone);
		Destroy(ray.gameObject);

		if (_lastEvaluation.Score >= 3.9f)
		{
			// Planet won
		}
		else if (_likeness <= 0.0f)
		{
			// Planet lost
		}
	}

	private void OnAlienPanelDone()
	{
		GameManager.ControlPanel.EnableUI();
	}

	private Sprite GetAlienSprite(Evaluation evaluation)
	{
		if (evaluation.Score > 1.0f)
		{
			return AlienSpriteGood;
		}
		if (evaluation.Score < -1.0f)
		{
			return AlienSpriteBad;
		}
		
		return AlienSpriteNeutral;
	}

	private Evaluation EvaluateTransmission(Transmission.Data transmission)
	{
		var eval = new Evaluation();

		for (int i = 0; i < Transmission.NUMBER_OF_SIGNALS; i++)
		{
			eval.Results[i] = Evaluation.Result.Bad;
		}

		for (int i = 0; i < Transmission.NUMBER_OF_SIGNALS; i++)
		{
			if (transmission.Signals[i] == _goalTransmission.Signals[i])
			{
				// Perfect hit!
				eval.Results[i] = Evaluation.Result.Perfect;
				eval.Score += 1.0f;
				continue;
			}

			// Check for good hit
			for (int j = 0; j < Transmission.NUMBER_OF_SIGNALS; j++)
			{
				if (transmission.Signals[i] == _goalTransmission.Signals[j])
				{
					// Found good hit
					eval.Results[i] = Evaluation.Result.Good;
					eval.Score += 1.0f;
					break;
				}
			}

			eval.Score -= 1.0f;
		}

		return eval;
	}
}
