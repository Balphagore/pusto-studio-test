namespace PustoStudioTest.Clocks
{
	using DG.Tweening;
	using System;
	using UnityEngine;

	public class Clock : MonoBehaviour
	{
		[SerializeField]
		private Transform							_hoursHand;
		[SerializeField]
		private Transform							_minutesHand;
		[SerializeField]
		private Transform							_secondsHand;

		private float								_initialHoursAngle;
		private float								_initialMinutesAngle;
		private float								_initialSecondsAngle;

		public event Action<bool>					ClockHandDraggedEvent;
		public event Action<int, EClockValueType>	ClockValueUpdatedEvent;
		public event Action<bool>					EditModeSetEvent;

		public void Initialize(TimeSpan time)
		{
			_initialHoursAngle = SetHandPosition(_hoursHand, time.TotalHours, 30f);
			_initialMinutesAngle = SetHandPosition(_minutesHand, time.TotalMinutes, 6f);
			_initialSecondsAngle = SetHandPosition(_secondsHand, time.TotalSeconds, 6f);
		}

		public void ActivateCountdown()
		{
			StopCountdown();

			AnimateHand(_hoursHand, _initialHoursAngle, 360f / 12f, 43200f);
			AnimateHand(_minutesHand, _initialMinutesAngle, 360f, 3600f);
			AnimateHand(_secondsHand, _initialSecondsAngle, 360f, 60f);
		}

		public void StopCountdown()
		{
			_hoursHand.DOKill();
			_minutesHand.DOKill();
			_secondsHand.DOKill();
		}

		public void ActivateEdit()
		{
			_hoursHand.DOKill();
			_minutesHand.DOKill();
			_secondsHand.DOKill();

			EditModeSetEvent?.Invoke(true);
		}

		public void StopEdit()
		{
			EditModeSetEvent?.Invoke(false);
		}

		public void SetHandDragged(bool isDragged)
		{
			ClockHandDraggedEvent?.Invoke(isDragged);
		}

		public void UpdateHandValue(float value, EClockValueType clockValueType)
		{
			ClockValueUpdatedEvent?.Invoke(ConvertAngleToTimeValue(value, clockValueType), clockValueType);
		}

		public void UpdateHandPosition(int value, EClockValueType clockValueType)
		{
			switch (clockValueType)
			{
				case EClockValueType.Hours:

					if (value >= 0 && value < 24)
					{
						float newAngle = SetHandPosition(_hoursHand, value, 30f);
						_initialHoursAngle = newAngle;
					}
					else
					{
						Debug.LogError("Некорректное значение часов");
					}
					break;

				case EClockValueType.Minutes:

					if (value >= 0 && value < 60)
					{
						float newAngle = SetHandPosition(_minutesHand, value, 6f);
						_initialMinutesAngle = newAngle;
					}
					else
					{
						Debug.LogError("Некорректное значение минут");
					}
					break;

				case EClockValueType.Seconds:

					if (value >= 0 && value < 60)
					{
						float newAngle = SetHandPosition(_secondsHand, value, 6f);
						_initialSecondsAngle = newAngle;
					}
					else
					{
						Debug.LogError("Некорректное значение секунд");
					}
					break;

				default:

					Debug.LogWarning("Incorrect EClockValueType");

					break;
			}
		}

		private int ConvertAngleToTimeValue(float angle, EClockValueType clockValueType)
		{
			angle = angle % 360f;
			if (angle < 0) angle += 360f;

			int timeValue = 0;
			switch (clockValueType)
			{
				case EClockValueType.Hours:
					timeValue = 11 - ( ( int ) ( angle / 30f ) % 12 );
					return timeValue;
				case EClockValueType.Minutes:
				case EClockValueType.Seconds:
					timeValue = 59 - ( ( int ) ( angle / 6f ) % 60 );
					return timeValue;
				default:
					return 0;
			}
		}

		private float SetHandPosition(Transform hand, double timeValue, float anglePerUnit)
		{
			float angle = (float)(-timeValue * anglePerUnit) % 360f;
			hand.localRotation = Quaternion.Euler(0f, 0f, angle);
			return angle;
		}

		private void AnimateHand(Transform hand, float initialAngle, float anglePerCycle, float cycleDuration)
		{
			float endValue = initialAngle - anglePerCycle;
			hand.DOLocalRotate(new Vector3(0f, 0f, endValue), cycleDuration * 2, RotateMode.FastBeyond360)
				.SetEase(Ease.Linear)
				.SetLoops(-1, LoopType.Incremental)
				.SetUpdate(true);
		}
	}
}
