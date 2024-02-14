using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class World
{
	public Grid Grid => m_grid;

	public List<Tilemap> TileMaps => m_tileMaps;

	public TimeManager TimeManager => m_timeManager;

	public World(Grid grid)
	{
		m_grid = grid;
		m_grid.GetComponentsInChildren(m_tileMaps);
	}

	public void Update(float deltaTime)
	{
		TimeManager.Update(deltaTime);
	}


	[SerializeField]
	[HideInInspector]
	private TimeManager m_timeManager = new TimeManager();
	[SerializeField]
	[HideInInspector]
	private List<Tilemap> m_tileMaps = null;

	[SerializeField]
	[HideInInspector]
	private Grid m_grid = null;
}
