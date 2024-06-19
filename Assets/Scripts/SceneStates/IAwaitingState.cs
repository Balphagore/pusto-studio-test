namespace PustoStudioTest.SceneStates
{
	using System;

	public interface IAwaitingState<TState> : IState<TState>
	{
		event Action StateFinishedEvent;
	}
}
