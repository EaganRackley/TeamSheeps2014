using UnityEngine;
using System.Collections;

public class VictoryCondition : MonoBehaviour {
	public Camera m_camera;

	// Use this for initialization
	void Start () {
        m_camera.GetComponent<AccelerometerRotate>().enabled = true;
    }

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" )
		{
            m_camera.GetComponent<moveWorld>().RotateCameraTo(180);
            m_camera.GetComponent<AccelerometerRotate>().enabled = false;
            //Camera.main.backgroundColor = new Color(0,136/255.0f,164/255.0f,1);
            BroadcastMessage("EnableFollow");
			BroadcastMessage("EnableParallax");
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
