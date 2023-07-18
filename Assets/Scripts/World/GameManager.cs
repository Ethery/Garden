using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Grid))]
public class GameManager : Singleton<GameManager>
{
	public World World;
	public GameTile baseTile;
	private void Awake()
	{
		World = new World();
		World.Grid = GetComponent<Grid>();
		World.TileMap.SetTile(new Vector3Int(0, 0, 0), Instantiate(baseTile));
	}
}
