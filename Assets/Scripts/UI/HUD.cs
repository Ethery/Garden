using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

public class HUD : Singleton<HUD>
{
	public void OnPlaceTile(GameTile tile)
	{
        GameManager.Instance.World.TileMap.SetTile(new Vector3Int(0, 0, 0), Instantiate(tile));
	}
}
