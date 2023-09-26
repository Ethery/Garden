using System;
using UnityEngine;

[Serializable]
public class TimeManager
{
	public TimeConfig Config => GameManager.Instance.WorldConfig.TimeConfig;

	public int CurrentDay => Mathf.RoundToInt(m_timeSinceGameStart / Config.TimePerDay);

	public float TimeInDay => m_timeSinceGameStart % Config.TimePerDay;

	public float TimeInDay01 => TimeInDay / Config.TimePerDay;

	public void Update (float deltaTime)
	{
		m_timeSinceGameStart += deltaTime;
	}

	[SerializeField]
	private float m_timeSinceGameStart = 0;
}
