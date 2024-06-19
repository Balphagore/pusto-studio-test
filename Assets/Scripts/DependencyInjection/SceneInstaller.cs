namespace PustoStudioTest.DependencyInjection
{
	using PustoStudioTest.Clocks;
	using PustoStudioTest.SceneStates;
	using PustoStudioTest.TimeFetch;
	using UnityEngine;
	using VContainer;
	using VContainer.Unity;

	public class SceneInstaller : LifetimeScope
	{
		[SerializeField] 
		private CoroutineRunner _coroutineRunnerPrefab;

		protected override void Configure(IContainerBuilder builder)
		{
			builder.RegisterComponentInNewPrefab(_coroutineRunnerPrefab, Lifetime.Singleton);

			builder.RegisterComponentInHierarchy<SceneStateService>().AsSelf();
			builder.RegisterComponentInHierarchy<Clock>().AsSelf();
			builder.RegisterComponentInHierarchy<DigitalClock>().AsSelf();

			builder.RegisterEntryPoint<TimeFetchService>().AsSelf();
		}
	}
}
