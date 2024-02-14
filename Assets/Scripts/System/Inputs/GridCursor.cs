
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

		private void Update()
		{
			if(m_oldMousePosition != m_currentMousePosition)
			{
				GameManager.Instance.World.TileMap.SetTile(m_oldMousePosition, null);
				GameManager.Instance.World.TileMap.SetTile(m_currentMousePosition, m_cursorTile);
				m_oldMousePosition = m_currentMousePosition;
			}
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
				//hitPoint = Grid.Swizzle(GameManager.Instance.World.Grid.cellSwizzle, hitPoint);
				m_currentMousePosition = GameManager.Instance.World.TileMap.WorldToCell(hitPoint);
			}
		}

		[NonSerialized]
		private Vector3Int m_currentMousePosition;
		[NonSerialized]
		private Vector3Int m_oldMousePosition;
		[NonSerialized]
		private Plane m_plane;

		[SerializeField]
		private Tile m_cursorTile;
	}
}