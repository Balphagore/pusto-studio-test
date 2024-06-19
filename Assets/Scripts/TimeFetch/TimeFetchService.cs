namespace PustoStudioTest.TimeFetch
{
	using Newtonsoft.Json;
	using PustoStudioTest.DependencyInjection;
	using System;
	using System.Collections;
	using UnityEngine;
	using UnityEngine.Networking;
	using VContainer;

	public class TimeFetchService
	{
		private readonly string _timeServerURL = "https://yandex.com/time/sync.json?geo=213";

		private CoroutineRunner _coroutineRunner;

		[Inject]
		public TimeFetchService(IObjectResolver resolver)
		{
			_coroutineRunner = resolver.Resolve<CoroutineRunner>();
		}

		public void Initialize(Action<TimeSpan> onTimeFetched)
		{
			
		}

		public void GetRealTime(Action<TimeSpan> onTimeFetched)
		{
			_coroutineRunner.StartCoroutine(GetTimeFromServer(onTimeFetched));
		}

		private IEnumerator GetTimeFromServer(Action<TimeSpan> onTimeFetched)
		{
			UnityWebRequest request = UnityWebRequest.Get(_timeServerURL);
			yield return request.SendWebRequest();

			if (request.result != UnityWebRequest.Result.Success)
			{
				Debug.LogError("Error fetching time: " + request.error);
			}
			else
			{
				TimeSpan time = ParseTimeFromJson(request.downloadHandler.text);
				onTimeFetched?.Invoke(time);
			}
		}

		private TimeSpan ParseTimeFromJson(string json)
		{
			TimeSyncDataModel timeSyncData = JsonConvert.DeserializeObject<TimeSyncDataModel>(json);
			TownInfo townInfo = timeSyncData.Clocks.Town;
			DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(timeSyncData.Time + townInfo.Offset).DateTime;

			return dateTime.TimeOfDay;
		}
	}
}