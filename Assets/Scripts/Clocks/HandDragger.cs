namespace PustoStudioTest.Clocks
{
	using UnityEngine;
	using UnityEngine.EventSystems;

	public class HandDragger : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
	{
		[SerializeField]
		private Transform		_clockTransform;
		[SerializeField]
		private Clock			_clock;
		[SerializeField]
		private EClockValueType	_clockValueType;

		private Camera			_camera;
		private bool			_isEditMode;

		private void Awake()
		{
			_camera = Camera.main;
		}

		private void OnEnable()
		{
			_clock.EditModeSetEvent += OnEditModeSetEvent;
		}

		private void OnDisable()
		{
			_clock.EditModeSetEvent -= OnEditModeSetEvent;
		}

		private void OnEditModeSetEvent(bool isEditMode)
		{
			_isEditMode = isEditMode;
		}

		public void OnDrag(PointerEventData eventData)
		{
			if(_isEditMode)
			{
				Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(_camera, _clockTransform.position);
				float angle = CalculateAngle(screenPoint, eventData.position);
				transform.rotation = Quaternion.Euler(0, 0, angle);
				_clock.UpdateHandValue(angle, _clockValueType);
			}
		}

		public void OnBeginDrag(PointerEventData eventData)
		{
			_clock.SetHandDragged(true);
		}

		public void OnEndDrag(PointerEventData eventData)
		{
			_clock.SetHandDragged(false);
		}

		private float CalculateAngle(Vector2 from, Vector2 to)
		{
			Vector2 direction = to - from;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			angle -= 90;
			if (angle > 180) angle -= 360;
			return angle;
		}
	}
}
