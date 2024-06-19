namespace PustoStudioTest.SceneStates
{
	public interface IState<TState>
	{
		TState StateValue { get; }
		void Enter();
		void Exit();
	}
}
