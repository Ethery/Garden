using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class World
{
	public enum MapLayer
	{
		World,
		UI,
	}

	public Grid Grid => m_grid;

	public TimeManager TimeManager => m_timeManager;

	public WorldConfig Config => m_config;

	public Dictionary<MapLayer, Tilemap> TileMaps
	{
		get
		{
			if(m_tileMaps == null)
			{
				List<Tilemap> tilemaps = new List<Tilemap>();
				m_grid.GetComponentsInChildren(tilemaps);

				int i = 0;
				m_tileMaps = new Dictionary<MapLayer, Tilemap>();
				for(; i < tilemaps.Count; i++)
				{
					m_tileMaps.Add((MapLayer)i, tilemaps[i]);
				}

				Array layers = Enum.GetValues(typeof(MapLayer));
				while(m_tileMaps.Count <= layers.Length)
				{
					m_tileMaps.Add((MapLayer)i++, GameObject.Instantiate(m_config.TilemapPrefab, m_grid.transform));
				}

				if(layers.Length < m_tileMaps.Count)
				{
					Debug.LogWarning($"There is too much TileMaps in Grid. (Maybe add a value in {nameof(MapLayer)})", m_grid);
				}
			}
			return m_tileMaps;
		}
	}

	public World(Grid grid, WorldConfig config)
	{
		m_grid = grid;
		m_config = config;
	}

	public void Update(float deltaTime)
	{
		TimeManager.Update(deltaTime);
	}

	[NonSerialized]
	[HideInInspector]
	private Dictionary<MapLayer, Tilemap> m_tileMaps = null;
	[SerializeField]
	[HideInInspector]
	private WorldConfig m_config = null;

	[SerializeField]
	[HideInInspector]
	private TimeManager m_timeManager = new TimeManager();

	[SerializeField]
	[HideInInspector]
	private Grid m_grid = null;
}
