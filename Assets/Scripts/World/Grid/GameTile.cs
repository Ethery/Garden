using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "BaseGameTile", menuName = "2D/Tiles/Custom/GameTile", order = 1)]
public class GameTile : Tile
{
	public List<Item> Items;

	public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
	{
		go.transform.up = Vector3.up;
		return base.StartUp(position, tilemap, go);
	}
}
