namespace PustoStudioTest.SceneStates
{
	using UnityEngine;
	using VContainer;

	public class SceneStateService : MonoBehaviour
	{
		[SerializeField]
		private ESceneState         _initialState;

		private IState<ESceneState>	_currentState;
		private IObjectResolver		_resolver;

		[Inject]
		public void Construct(IObjectResolver resolver)
		{
			_resolver = resolver;
		}

		private void Start()
		{
			ActivateState(_initialState);
		}

		public void ActivateState(ESceneState state)
		{
			switch (state)
			{
				case ESceneState.None:

					if (_currentState != null)
					{
						_currentState.Exit();
						_currentState = null;
					}
					break;

				case ESceneState.Initialization:

					IAwaitingState<ESceneState> initializationState = new InitializationState(_resolver);
					initializationState.StateFinishedEvent += OnStateFinishedEvent;
					_currentState = initializationState;
					_currentState.Enter();

					break;

				case ESceneState.CountdownState:

					_currentState.Exit();
					IAwaitingState<ESceneState> connectionState = new CountdownState(_resolver);
					connectionState.StateFinishedEvent += OnStateFinishedEvent;
					_currentState = connectionState;
					_currentState.Enter();

					break;

				case ESceneState.TimeSyncState:

					_currentState.Exit();
					IAwaitingState<ESceneState> timeSyncState = new TimeSyncState(_resolver);
					timeSyncState.StateFinishedEvent += OnStateFinishedEvent;
					_currentState = timeSyncState;
					_currentState.Enter();

					break;

				case ESceneState.EditState:

					_currentState.Exit();
					IAwaitingState<ESceneState> editState = new EditState(_resolver);
					editState.StateFinishedEvent += OnStateFinishedEvent;
					_currentState = editState;
					_currentState.Enter();

					break;

				default:

					Debug.LogError("Invalid state");

					break;
			}
		}

		private void OnStateFinishedEvent()
		{
			switch (_currentState.StateValue)
			{
				case ESceneState.Initialization:

					( _currentState as IAwaitingState<ESceneState> ).StateFinishedEvent -= OnStateFinishedEvent;
					ActivateState(ESceneState.TimeSyncState);

					break;

				case ESceneState.TimeSyncState:

					( _currentState as IAwaitingState<ESceneState> ).StateFinishedEvent -= OnStateFinishedEvent;
					_currentState.Exit();
					ActivateState(ESceneState.CountdownState);

					break;

				case ESceneState.EditState:

					( _currentState as IAwaitingState<ESceneState> ).StateFinishedEvent -= OnStateFinishedEvent;
					_currentState.Exit();
					ActivateState(ESceneState.CountdownState);

					break;

				default:

					Debug.LogError("Invalid state");

					break;
			}
		}
	}
}
