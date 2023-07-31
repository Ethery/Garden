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
		RegisterInputs(true);
	}

	private void RegisterInputs(bool register)
	{
		InputManager.RegisterInput(InputManager.Input.CAMERA_MOVE
									, new InputManager.InputEvent(OnCameraMovement, InputManager.EventType.Performed | InputManager.EventType.Released)
									, register);

		InputManager.RegisterInput(InputManager.Input.CAMERA_SPEED_UP,
									new InputManager.InputEvent(OnCameraSpeedUp, InputManager.EventType.Performed | InputManager.EventType.Released)
									, register);
	}


	public void OnCameraMovement(InputAction action)
	{
		wantedMovement = action.ReadValue<Vector2>();
	}
	public void OnCameraSpeedUp(InputAction action)
	{
		speedUp = action.ReadValue<float>() > 0f;
	}

	private void FixedUpdate()
	{
		if(wantedMovement != Vector2.zero)
		{
			Vector3 newPosition = transform.position;

			newPosition.x += wantedMovement.x * Time.deltaTime * (speedUp ? CameraSpeedMultiplier : 1);
			newPosition.z += wantedMovement.y * Time.deltaTime * (speedUp ? CameraSpeedMultiplier : 1);

			transform.position = newPosition;
		}
	}

	private void OnDisable()
	{
		RegisterInputs(false);
	}

	private Vector2 wantedMovement = Vector2.zero;
	private bool speedUp = false;
}
