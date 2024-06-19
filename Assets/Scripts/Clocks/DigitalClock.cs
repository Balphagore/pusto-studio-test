namespace PustoStudioTest.Clocks
{
	using PustoStudioTest.SceneStates;
	using System;
	using System.Collections;
	using TMPro;
	using UnityEngine;
	using VContainer;

	public class DigitalClock : MonoBehaviour
	{
		[Inject]
		private SceneStateService	_sceneStateService;
		[Inject]
		private Clock				_clock;

		[SerializeField]
		private TMP_InputField		_hoursInputField;
		[SerializeField]
		private TMP_InputField		_minutesInputField;
		[SerializeField]
		private TMP_InputField		_secondsInputField;

		private TimeSpan			_timePassed;
		private int					_lastHourExecuted	= -1;
		private int					_lastDayExecuted	= -1;
		private Coroutine			_updateTimeCoroutine;
		private bool				_isHandDragged;

#if UNITY_EDITOR
		[SerializeField, Range(0, 100)]
		private float				_timeScale			= 1;

		private void OnValidate()
		{
			Time.timeScale = _timeScale;
		}
#endif
		private void OnEnable()
		{
			_hoursInputField.onValueChanged.AddListener(OnHoursValueChanged);
			_minutesInputField.onValueChanged.AddListener(OnMinutesValueChanged);
			_secondsInputField.onValueChanged.AddListener(OnSecondsValueChanged);
			_clock.ClockHandDraggedEvent += OnClockHandDraggedEvent;
			_clock.ClockValueUpdatedEvent += OnClockValueUpdatedEvent;
		}

		private void OnDisable()
		{
			_hoursInputField.onValueChanged.RemoveListener(OnHoursValueChanged);
			_minutesInputField.onValueChanged.RemoveListener(OnMinutesValueChanged);
			_secondsInputField.onValueChanged.RemoveListener(OnSecondsValueChanged);
			_clock.ClockHandDraggedEvent -= OnClockHandDraggedEvent;
			_clock.ClockValueUpdatedEvent -= OnClockValueUpdatedEvent;
		}

		private void OnClockHandDraggedEvent(bool isHandDragged)
		{
			_isHandDragged = isHandDragged;
		}

		public void Initialize(TimeSpan startTime)
		{
			_timePassed = startTime;
			UpdateInputFields(_timePassed);
		}

		public void ActivateCountdown()
		{
			StopCountdown();
			_updateTimeCoroutine = StartCoroutine(UpdateTime());
		}

		public void StopCountdown()
		{
			if (_updateTimeCoroutine != null)
			{
				StopCoroutine(_updateTimeCoroutine);
			}
		}

		public void ActivateEdit()
		{
			_hoursInputField.interactable = true;
			_minutesInputField.interactable = true;
			_secondsInputField.interactable = true;
		}

		public void StopEdit()
		{
			_hoursInputField.interactable = false;
			_minutesInputField.interactable = false;
			_secondsInputField.interactable = false;
		}

		private void OnClockValueUpdatedEvent(int value, EClockValueType clockValueType)
		{
			switch (clockValueType)
			{
				case EClockValueType.Hours:

					_hoursInputField.text = value.ToString();

					break;

				case EClockValueType.Minutes:

					_minutesInputField.text = value.ToString();

					break;

				case EClockValueType.Seconds:

					_secondsInputField.text = value.ToString();

					break;

				default:

					Debug.LogWarning("Incorrect EClockValueType");

					break;
			}
		}

		private void OnHoursValueChanged(string value)
		{
			if (!_isHandDragged)
			{
				if (int.TryParse(value, out int hours) && hours >= 0 && hours < 24)
				{
					_timePassed = new TimeSpan(hours, _timePassed.Minutes, _timePassed.Seconds);
					_clock.UpdateHandPosition(hours, EClockValueType.Hours);
				}
			}
		}

		private void OnMinutesValueChanged(string value)
		{
			if (!_isHandDragged)
			{
				if (int.TryParse(value, out int minutes) && minutes >= 0 && minutes < 60)
				{
					_timePassed = new TimeSpan(_timePassed.Hours, minutes, _timePassed.Seconds);
					_clock.UpdateHandPosition(minutes, EClockValueType.Minutes);
				}
			}
		}

		private void OnSecondsValueChanged(string value)
		{
			if (!_isHandDragged)
			{
				if (int.TryParse(value, out int seconds) && seconds >= 0 && seconds < 60)
				{
					_timePassed = new TimeSpan(_timePassed.Hours, _timePassed.Minutes, seconds);
					_clock.UpdateHandPosition(seconds, EClockValueType.Seconds);
				}
			}
		}

		private IEnumerator UpdateTime()
		{
			bool isFirstUpdate = true;

			while (true)
			{
				_timePassed = _timePassed.Add(TimeSpan.FromSeconds(1));
				UpdateInputFields(_timePassed);

				int currentHour = _timePassed.Hours + _timePassed.Days * 24;

				if (_timePassed.Days != _lastDayExecuted)
				{
					if (!isFirstUpdate)
					{
						SyncTime();
					}
					_lastDayExecuted = _timePassed.Days;
					_lastHourExecuted = currentHour - 1;
				}

				if (currentHour != _lastHourExecuted)
				{
					if (!isFirstUpdate)
					{
						SyncTime();
					}
					_lastHourExecuted = currentHour;
				}

				if (isFirstUpdate)
				{
					isFirstUpdate = false;
				}

				yield return new WaitForSeconds(1);
			}
		}

		private void SyncTime()
		{
			_sceneStateService.ActivateState(ESceneState.TimeSyncState);
		}

		private void UpdateInputFields(TimeSpan time)
		{
			_hoursInputField.text = time.Hours.ToString("00");
			_minutesInputField.text = time.Minutes.ToString("00");
			_secondsInputField.text = time.Seconds.ToString("00");
		}
	}
}
