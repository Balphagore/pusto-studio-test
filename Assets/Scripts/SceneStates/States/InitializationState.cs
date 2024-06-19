namespace PustoStudioTest.SceneStates
{
	using System;
	using VContainer;

	public class InitializationState : IAwaitingState<ESceneState>
	{
		public ESceneState	StateValue			=> ESceneState.Initialization;

		public event Action	StateFinishedEvent;

		[Inject]
		public InitializationState(IObjectResolver resolver)
		{

		}

		public void Enter()
		{
			StateFinishedEvent?.Invoke();
		}

		public void Exit()
		{
			
		}
	}
}
