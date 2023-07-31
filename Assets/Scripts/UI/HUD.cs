using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class HUD : Singleton<HUD>
{
	private void OnEnable()
	{
		InputManager.RegisterInput(InputManager.Input.MOUSE_CLICK, new InputManager.InputEvent(OnMouseClick, InputManager.EventType.Performed), true);
	}

	private void OnMouseClick(InputAction input)
	{
		Vector2 mousePosition = input.ReadValue<Vector2>();
		ray = new Ray(Camera.main.ScreenToWorldPoint(mousePosition), Camera.main.transform.forward);
		if(Physics.Raycast(ray, out hit, 150))
		{
			Debug.Log("click on " + hit.point);

			Vector3Int tpos = GameManager.Instance.World.TileMap.WorldToCell(hit.point);

			// Try to get a tile from cell position
			TileBase tile = GameManager.Instance.World.TileMap.GetTile(tpos);
		}
	}

	private void OnDrawGizmos()
	{
		if(hit.transform != null)
		{
			Gizmos.color = Color.blue;
			Gizmos.DrawSphere(hit.point, 0.2f);

			Color bMinus = Color.blue;
			bMinus.b -= 0.1f;
			Gizmos.color = bMinus;
			Gizmos.DrawLine(ray.origin, hit.point);

			Gizmos.DrawRay(ray);
		}
	}

	private RaycastHit hit;
	private Ray ray;
}
