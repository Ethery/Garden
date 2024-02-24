using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GameManager : Singleton<GameManager>
{
	public World World;

	public GameTile baseTile;

	protected override void Awake()
	{
		base.Awake();
		World = new World(GetComponent<Grid>(), m_config);
		World.TileMaps[World.MapLayer.World].SetTile(new Vector3Int(0, 0, 0), Instantiate(baseTile));
	}

	private void Update()
	{
		World.Update(Time.deltaTime);
	}

	[SerializeField]
	private WorldConfig m_config;
}
