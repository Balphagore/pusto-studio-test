namespace PustoStudioTest.UI
{
	using PustoStudioTest.SceneStates;
	using UnityEngine;
    using UnityEngine.UI;
	using VContainer;

    public class UIService : MonoBehaviour
    {
		[Inject]
		private SceneStateService	_sceneStateService;

		[SerializeField]
        private Button				_editOnButton;
		[SerializeField]
		private Button              _editOffButton;

		private void OnEnable()
		{
			_editOnButton.onClick.AddListener(OnEditOnButtonClick);
			_editOffButton.onClick.AddListener(OnEditOffButtonClick);
		}

		private void OnDisable()
		{
			_editOnButton.onClick.RemoveListener(OnEditOnButtonClick);
			_editOffButton.onClick.AddListener(OnEditOffButtonClick);
		}

		private void OnEditOnButtonClick()
		{
			_sceneStateService.ActivateState(ESceneState.EditState);
			_editOnButton.gameObject.SetActive(false);
			_editOffButton.gameObject.SetActive(true);
		}

		private void OnEditOffButtonClick()
		{
			_sceneStateService.ActivateState(ESceneState.CountdownState);
			_editOnButton.gameObject.SetActive(true);
			_editOffButton.gameObject.SetActive(false);
		}
	}
}
