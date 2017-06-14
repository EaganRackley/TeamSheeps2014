using UnityEngine;
using System.Collections;

public class VerticalParallax : MonoBehaviour {

	public SmoothCamera2D CameraObject;
	public float CameraOffset;

	private bool m_Enabled = false;
	private float m_InitialY;
	private float m_CameraOffset;
	private Vector3 m_StartingPosition;

	// Use this for initialization
	void Start () 
	{
        if (CameraObject == null) CameraObject = FindObjectOfType<SmoothCamera2D>();
		m_Enabled = Enabled;
		m_CameraOffset = CameraOffset;
		m_StartingPosition = this.transform.position;
		m_InitialY = CameraObject.transform.position.y;
	}

	public void SetRandomGravity()
	{
		Enabled = true;
	}

	bool Enabled
	{
		get 
		{ 
			return m_Enabled; 
		}
		set 
		{ 
			m_Enabled = value;
			if(m_Enabled == true)
			{
				m_InitialY = CameraObject.transform.position.y;
			}
		}
	}

	void EnableParallax()
	{
		Enabled = true;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (m_Enabled == true) 
		{
			Vector3 position = this.transform.position;
			position.y = m_StartingPosition.y + ((CameraObject.transform.position.y - m_StartingPosition.y ) / m_CameraOffset);
			this.transform.position = position;
		}
	}
}
