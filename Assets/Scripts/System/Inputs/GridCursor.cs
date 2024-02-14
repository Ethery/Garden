
using JetBrains.Annotations;
using System;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Game.Systems.Inputs
{
	public class GridCursor : Singleton<GridCursor>
	{
		private void OnEnable()
		{
			m_plane = new Plane(GameManager.Instance.World.TileMap.transform.up
				, GameManager.Instance.World.TileMap.transform.position.magnitude);
			RegisterInputs(true);
		}

		private void OnDisable()
		{
			RegisterInputs(false);
		}

		private void RegisterInputs(bool register)
		{
			InputManager.RegisterInput(InputManager.Input.MOUSE_POS, new InputManager.InputEvent(OnMousePosPerf, UnityEngine.InputSystem.InputActionChange.ActionPerformed), register);
		}

		private void OnMousePosPerf(UnityEngine.InputSystem.InputAction obj)
		{
			Ray ray = Camera.main.ScreenPointToRay(obj.ReadValue<Vector2>());
			float distanceToPlane;

			if(m_plane.Raycast(ray, out distanceToPlane))
			{
				Vector3 hitPoint = ray.GetPoint(distanceToPlane);
				Vector3Int hitOnTilemap = hitPoint.ToVector3Int();
				GameManager.Instance.World.TileMap.SetTile(hitOnTilemap, m_cursorTile);

				Debug.Log($"hit ({hitPoint}/{hitOnTilemap})");
			}
		}

		[NonSerialized]
		private Plane m_plane;
		[SerializeField]
		private Tile m_cursorTile;
	}
}