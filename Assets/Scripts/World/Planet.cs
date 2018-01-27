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

	public enum State
	{
		Ongoing,
		Won,
		Lost
	}
	
	public float StepsToFailure = 8;
	public Sprite AlienSpriteGood;
	public Sprite AlienSpriteBad;
	public Sprite AlienSpriteNeutral;
	public State PlanetState;
	[Range(0.5f, 1.5f)] public float _hoverMod = 1.0f;

	[SerializeField] private ParticleSystem _loveParticles;
	[SerializeField] private ParticleSystem _hateParticles;
	[SerializeField] private GameObject _loveIcon;
	[SerializeField] private GameObject _hateIcon;
	[SerializeField] private AudioSource _loveSound;
	[SerializeField] private AudioSource _hateSound;
	[SerializeField] private AudioSource _neutralSound;
	[SerializeField] private AlienPanel _detailPanel;

	private Transmission.Data _goalTransmission;
	private Transmission.Data _lastTransmission;
	private Evaluation _lastEvaluation;
	private float _likeness;
	private float _angerMod;

	private Animator _animator;

	private void Awake()
	{
		_animator = GetComponent<Animator>();
	}
	
	private void Start()
	{
		_animator.SetFloat("SpeedMult", _hoverMod);
		RandomizeGoal();
		PlanetState = State.Ongoing;
		_likeness = 1.0f;
		_angerMod = 1.0f / StepsToFailure;
	}

	public void OnPanelClicked()
	{
		if (_lastTransmission == null)
		{
			// No transmissions yet
			return;
		}
		
		_detailPanel.Setup(AlienSpriteBad, _lastTransmission, _lastEvaluation, _likeness);
		_animator.SetTrigger("ShowPanel");
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
		switch (PlanetState)
		{
			case State.Ongoing:
				// Nothing - keep running
				break;
			case State.Won:
				_loveParticles.Play();
				_loveSound.Play();
				GameManager.ControlPanel.EnableUI();
				return;
			case State.Lost:
				_hateParticles.Play();
				_hateSound.Play();
				GameManager.ControlPanel.EnableUI();
				return;
			default:
				throw new ArgumentOutOfRangeException();
		}
		if (PlanetState != State.Ongoing)
		{
			// Interactions are only with Ongoing planets
			return;
		}
		
		var ray = other.gameObject.GetComponent<TransmissionRay>();
		if (ray == null)
		{
			return;
		}
		
		_lastTransmission = ray.Transmission;
		_lastEvaluation = EvaluateTransmission(ray.Transmission);

		if (_lastEvaluation.Score < 1.0f)
		{
			_likeness -= _angerMod;
		}
		
		GameManager.AlienPanel.Setup(GetAlienSprite(_lastEvaluation), _lastTransmission, _lastEvaluation, _likeness);
		GameManager.AlienPanel.Show(OnAlienPanelDone);

		if (_lastEvaluation.Score >= 3.9f)
		{
			OnPlanetWin();
		}
		else if (_likeness <= 0.0f)
		{
			OnPlanetLose();
		}
	}

	private void OnAlienPanelDone()
	{
		GameManager.ControlPanel.EnableUI();
	}

	private void OnPlanetWin()
	{
		PlanetState = State.Won;
		_loveParticles.Play();
		_loveIcon.SetActive(true);
	}
	
	private void OnPlanetLose()
	{
		PlanetState = State.Lost;
		_hateParticles.Play();
		_hateIcon.SetActive(true);
	}

	private Sprite GetAlienSprite(Evaluation evaluation)
	{
		if (evaluation.Score > 1.0f)
		{
			_loveSound.Play();
			return AlienSpriteGood;
		}
		if (evaluation.Score < -1.0f)
		{
			_hateSound.Play();
			return AlienSpriteBad;
		}
		
		_neutralSound.Play();
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
