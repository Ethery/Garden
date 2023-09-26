using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "NewWorldConfig", menuName = "Game/World/Config")]
public class WorldConfig : ScriptableObject
{
	public RulesConfig Rules => m_rules;

	public InputActionAsset Inputs => m_inputsConfig;

	public TimeConfig TimeConfig => m_timeConfig;

	[SerializeField]
	private TimeConfig m_timeConfig;
	[SerializeField]
	private RulesConfig m_rules;
	[SerializeField]
	public InputActionAsset m_inputsConfig;
}