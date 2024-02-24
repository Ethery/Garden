using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "NewWorldConfig", menuName = "Game/World/Config")]
public class WorldConfig : ScriptableObject
{
	public RulesConfig Rules => m_rules;

	public InputActionAsset Inputs => m_inputsConfig;

	public TimeConfig TimeConfig => m_timeConfig;

	public Tilemap TilemapPrefab => m_tilemapPrefab;

	[SerializeField]
	private TimeConfig m_timeConfig = new TimeConfig();
	[SerializeField]
	private RulesConfig m_rules;
	[SerializeField]
	public InputActionAsset m_inputsConfig;

	[SerializeField]
	private Tilemap m_tilemapPrefab;
}