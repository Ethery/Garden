using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
	#region Statics
	public class Group
	{
		public static Group Default = new Group("Default");

		public string Name => m_name;

		public Group(string v)
		{
			this.m_name = v;
		}

		private string m_name;
	}

	public class Input
	{
		public static Input CAMERA_MOVE = new Input("CameraMovement", Group.Default);
		public static Input CAMERA_SPEED_UP = new Input("CameraSpeedUp", Group.Default);
		public static Input MOUSE_CLICK = new Input("MouseClick", Group.Default);

		public Input(string name, Group group)
		{
			this.m_name = name;
			this.m_group = group;
		}

		public string Name => m_name;

		public Group Group => m_group;

		private string m_name;
		private Group m_group;
	}

	[Flags]
	public enum EventType
	{
		Started = (1 << 0),
		Performed = (1 << 1),
		Released = (1 << 2),
	}
	#endregion

	public class InputEvent
	{
		public Action<InputAction> Callback => m_callback;

		public EventType EventType => m_eventType;

		public InputEvent(Action<InputAction> callback, EventType eventType)
		{
			this.m_callback = callback;
			this.m_eventType = eventType;
		}

		private Action<InputAction> m_callback;
		private EventType m_eventType;
	}


	public static void RegisterInput(Input input, InputEvent eventToCall, bool register)
	{
		if(register)
		{
			if(Instance.m_events.ContainsKey(input))
			{
				if(!Instance.m_events[input].Add(eventToCall))
				{
					UnityEngine.Debug.Log($"An event with the same parameters already exists for {input.Group.Name}{input.Name}");
				}
			}
			else
			{
				Instance.m_events.Add(input, new HashSet<InputEvent> { eventToCall });
			}
		}
		else
		{
			if(Instance.m_events.ContainsKey(input))
			{
				if(Instance.m_events[input].Contains(eventToCall))
				{
					Instance.m_events[input].Remove(eventToCall);
					if(Instance.m_events[input].Count <= 0)
					{
						Instance.m_events.Remove(input);
					}
				}
				else
				{
					UnityEngine.Debug.LogError($"Can't remove an inexistant event in {input.Group.Name}{input.Name} event List");
				}
			}
			else
			{
				UnityEngine.Debug.LogError($"Can't remove an inexistant event for {input.Group.Name}{input.Name}");
			}
		}
	}

	#region Private
	protected override void Awake()
	{
		base.Awake();
		m_inputs.Clear();
		m_events.Clear();
		foreach(FieldInfo field in typeof(Input).GetFields(BindingFlags.Public | BindingFlags.Static))
		{
			if(field.DeclaringType == typeof(Input))
			{
				Input input = field.GetValue(null) as Input;
				m_inputs.Add(input.Name, input);
				Debug.Log($"Added Input {input.Name} to {nameof(InputManager)}.{nameof(m_inputs)}");
			}
		}

		InputSystem.onActionChange += OnActionChanged;
	}

	private void OnActionChanged(object obj, InputActionChange actionChange)
	{
		if(obj is InputAction action)
		{
			OnActionPerformed(action, actionChange);
		}
	}

	private void OnActionPerformed(InputAction action, InputActionChange actionChange)
	{
		if(m_inputs.ContainsKey(action.name))
		{
			Input input = m_inputs[action.name];
			if(m_events.ContainsKey(input))
			{
				foreach(InputEvent eventToCall in m_events[input])
				{
					bool call = false;
					switch(actionChange)
					{
						case InputActionChange.ActionStarted:
							call = eventToCall.EventType.HasFlag(EventType.Started);
							break;
						case InputActionChange.ActionPerformed:
							call = eventToCall.EventType.HasFlag(EventType.Performed);
							break;
						case InputActionChange.ActionCanceled:
							call = eventToCall.EventType.HasFlag(EventType.Released);
							break;
					}
					if(call)
					{
						eventToCall.Callback(action);
						UnityEngine.Debug.Log($"Called event for {input.Group.Name}.{input.Name}");
					}
				}
			}
		}
	}

	protected override void OnDestroy()
	{
		InputSystem.onActionChange -= OnActionChanged;
		m_inputs.Clear();
		m_events.Clear();
		base.OnDestroy();
	}


	private static Dictionary<string, Input> m_inputs = new Dictionary<string, Input>();

	private Dictionary<Input, HashSet<InputEvent>> m_events = new Dictionary<Input, HashSet<InputEvent>>();
	#endregion
}