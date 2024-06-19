namespace PustoStudioTest.SceneStates
{
	using PustoStudioTest.Clocks;
	using System;
	using VContainer;

	public class CountdownState : IAwaitingState<ESceneState>
	{
		private readonly Clock			_clock;
		private readonly DigitalClock	_digitalClock;

		public ESceneState				StateValue			=> ESceneState.CountdownState;

		public event Action				StateFinishedEvent;

		[Inject]
		public CountdownState(IObjectResolver resolver)
		{
			_clock = resolver.Resolve<Clock>();
			_digitalClock = resolver.Resolve<DigitalClock>();
		}

		public void Enter()
		{
			_clock.ActivateCountdown();
			_digitalClock.ActivateCountdown();
		}

		public void Exit()
		{
			_clock.StopCountdown();
			_digitalClock.StopCountdown();
		}
	}
}
