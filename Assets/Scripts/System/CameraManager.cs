using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class CameraManager : Singleton<CameraManager>  
{
	public float CameraSpeed = 5.0f;

	public float CameraSpeedMultiplier = 2.0f;
	private void OnEnable()
	{
		InputSystem.onActionChange += OnActionChanged;
	}

	public void OnActionChanged(object obj, InputActionChange actionChange) {
		if ( obj is InputAction action)
		{
			OnActionPerformed(action,actionChange);
		}
	}

	public void OnActionPerformed(InputAction action, InputActionChange actionChange)
	{
		if (actionChange == InputActionChange.ActionPerformed || actionChange == InputActionChange.ActionCanceled)
		{
			if (action.name == "CameraMovement")
			{
				wantedMovement = action.ReadValue<Vector2>();
			}
			if (action.name == "CameraSpeedUp")
			{
				speedUp = action.ReadValue<float>() > 0f;
			}
		}
		Debug.Log($"Action {action.name} : {actionChange}");
	}

	private void FixedUpdate()
	{
		if( wantedMovement != Vector2.zero) 
		{
			Vector3 newPosition = transform.position;


			newPosition.x += wantedMovement.x * Time.deltaTime * (speedUp ? CameraSpeedMultiplier : 1) ;
			newPosition.z += wantedMovement.y * Time.deltaTime * (speedUp ? CameraSpeedMultiplier : 1) ;

			transform.position = newPosition;
		}
	}

	private void OnDisable()
	{
		InputSystem.onActionChange -= OnActionChanged;
	}

	private Vector2 wantedMovement = Vector2.zero;
	private bool speedUp = false;
}
