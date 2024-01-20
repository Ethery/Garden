using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class SunLight : MonoBehaviour
{
	// Start is called before the first frame update
	void Start()
	{
		m_light = GetComponent<Light>();
	}

	private void Update()
	{
		if(m_light != null)
		{
			Vector3 sunPosition = m_sunDirection * GameManager.Instance.World.TimeManager.TimeInDay01;
			m_light.transform.rotation = Quaternion.Euler(GameManager.Instance.World.TimeManager.TimeInDay01 * 360, -90, -90);
		}
	}


	private Vector2 m_sunDirection;
	private float m_zenithalAngle;

	private Light m_light;
}
