using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName ="NewWorldConfig",menuName="Game/World/Config")]
public class WorldConfig : ScriptableObject
{
	public RulesConfig Rules => m_rules;


	[SerializeField]
	private RulesConfig m_rules;
}