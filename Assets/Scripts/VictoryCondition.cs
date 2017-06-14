using UnityEngine;
using System.Collections;

public class VictoryCondition : MonoBehaviour {
	public moveWorld m_moveWorld;

	// Use this for initialization
	void Start () {
	
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" )
		{
			m_moveWorld.RotateCameraTo(180);
			//Camera.main.backgroundColor = new Color(0,136/255.0f,164/255.0f,1);
			BroadcastMessage("EnableFollow");
			BroadcastMessage("EnableParallax");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
