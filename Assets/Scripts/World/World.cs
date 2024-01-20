using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class World
{
	public Grid Grid => m_grid;

	public Tilemap TileMap => m_tileMap;

	public TimeManager TimeManager => m_timeManager;

	public World(Grid grid)
	{
		m_grid = grid;
		m_tileMap = m_grid.GetComponentInChildren<Tilemap>();
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
	private Tilemap m_tileMap = null;

	[SerializeField]
	[HideInInspector]
	private Grid m_grid = null;
}
