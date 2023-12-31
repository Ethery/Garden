using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class World
{
	public Grid Grid;
	public Tilemap TileMap => Grid.GetComponentInChildren<Tilemap>();
}
