using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Game.Systems.Inputs
{
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
			public static Input CAMERA_MOVE = new Input(nameof(CAMERA_MOVE), Group.Default);
			public static Input CAMERA_SPEED_UP = new Input(nameof(CAMERA_SPEED_UP), Group.Default);
			public static Input CAMERA_SPEED_CONTROL = new Input(nameof(CAMERA_SPEED_CONTROL), Group.Default);
			public static Input MOUSE_CLICK = new Input(nameof(MOUSE_CLICK), Group.Default);
			public static Input MOUSE_POS = new Input(nameof(MOUSE_POS), Group.Default);

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

		#endregion

		public class InputEvent
		{
			public Action<InputAction> Callback => m_callback;

			public InputActionChange EventType => m_eventType;

			public InputEvent(Action<InputAction> callback, InputActionChange eventType)
			{
				this.m_callback = callback;
				this.m_eventType = eventType;
			}

			private Action<InputAction> m_callback;
			private InputActionChange m_eventType;
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
			if(IsMouseOnUI())
				return;
			if(m_inputs.ContainsKey(action.name))
			{
				Input input = m_inputs[action.name];
				if(m_events.ContainsKey(input))
				{
					foreach(InputEvent eventToCall in m_events[input])
					{
						if(eventToCall.EventType.HasFlag(actionChange))
						{
							eventToCall.Callback(action);
							UnityEngine.Debug.Log($"Called event for {input.Group.Name}.{input.Name}");
						}
					}
				}
			}
		}

		private bool IsMouseOnUI()
		{
			GameObject gameObject = EventSystem.current?.currentSelectedGameObject;
			return gameObject != null && gameObject.GetComponentInParent<Canvas>() != null;
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
}