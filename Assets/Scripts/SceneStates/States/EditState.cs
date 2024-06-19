namespace PustoStudioTest.SceneStates
{
	using PustoStudioTest.Clocks;
	using System;
	using VContainer;

	public class EditState : IAwaitingState<ESceneState>
	{
		private readonly Clock          _clock;
		private readonly DigitalClock	_digitalClock;

		public ESceneState				StateValue			=> ESceneState.EditState;

		public event Action				StateFinishedEvent;

		[Inject]
		public EditState(IObjectResolver resolver)
		{
			_clock = resolver.Resolve<Clock>();
			_digitalClock = resolver.Resolve<DigitalClock>();
		}

		public void Enter()
		{
			_clock.ActivateEdit();
			_digitalClock.ActivateEdit();
		}

		public void Exit()
		{
			_clock.StopEdit();
			_digitalClock.StopEdit();
		}
	}
}
