namespace PustoStudioTest.SceneStates
{
	using PustoStudioTest.Clocks;
	using PustoStudioTest.TimeFetch;
	using System;
	using VContainer;

	public class TimeSyncState : IAwaitingState<ESceneState>
	{
		private readonly TimeFetchService	_timeFetchService;
		private readonly Clock				_clock;
		private readonly DigitalClock		_digitalClock;

		public ESceneState					StateValue			=> ESceneState.TimeSyncState;

		public event Action					StateFinishedEvent;

		[Inject]
		public TimeSyncState(IObjectResolver resolver)
		{
			_timeFetchService = resolver.Resolve<TimeFetchService>();
			_clock = resolver.Resolve<Clock>();
			_digitalClock = resolver.Resolve<DigitalClock>();
		}

		public void Enter()
		{
			_timeFetchService.GetRealTime(TimeFetchedCallback);
		}

		public void Exit()
		{
			
		}

		private void TimeFetchedCallback(TimeSpan time)
		{
			_clock.Initialize(time);
			_digitalClock.Initialize(time);

			StateFinishedEvent.Invoke();
		}
	}
}
