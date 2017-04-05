using UnityEngine;
using System.Collections;

public class RailCart : MonoBehaviour 
{

	public StaticWaypoint StartingWaypoint;
	public float MaxSpeed;
	public SmoothCamera2D cameraPoint;
	public GameObject fixedCameraPoint;
	public float mainCameraOrth;
	public bool ChangeCameraTarget = false;
	public float CameraSizeSpeed = 1.0f;
	public float MaxCameraSize = 25.0f;

	// Private attributes
	private bool m_RailCartActivated = false;
	private bool m_RailCartFinished = false;
	private float m_Velocity = 0.0f;
	private float m_MaxVelocity = 0.0f;
	private bool m_HasEndedRail = false;

	// Private associations
	private StaticWaypoint m_CurrentWaypoint;
	private GameObject m_PlayerObject;

	public void ResetRailcartSettings(float newMaxSpeed, float newMaxCameraSize, float newCameraSizeSpeed)
	{
		m_MaxVelocity = newMaxSpeed;
		CameraSizeSpeed = newCameraSizeSpeed;
		MaxCameraSize = newMaxCameraSize;
	}

	// Use this for initialization
	void Awake () {
		this.transform.position = StartingWaypoint.transform.position;
		m_CurrentWaypoint = StartingWaypoint;
		m_RailCartActivated = false;
		m_RailCartFinished = false;
		m_HasEndedRail = false;
		m_MaxVelocity = MaxSpeed;
		mainCameraOrth = Camera.main.orthographicSize;
	}
	
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" && m_RailCartActivated == false && m_RailCartFinished == false) 
		{
			print ("Trigger Entered: " + other.gameObject.name.ToString ());

			m_RailCartActivated = true;
			m_RailCartFinished = false;
			m_PlayerObject = other.gameObject;
			m_Velocity = other.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude;
		}
	}

	void HandleReturningCameraSize ()
	{
		if ((ChangeCameraTarget == true) && (m_HasEndedRail == true))
		{
			if (Camera.main.orthographicSize > mainCameraOrth) 
			{
				Camera.main.orthographicSize -= 0.5f;
				if( Camera.main.orthographicSize == mainCameraOrth )
				{
					Awake();
				}
			}
		}
	}

	void HandleCameraAssignment ()
	{
		if(ChangeCameraTarget == true) 
		{
			if (Camera.main.orthographicSize < MaxCameraSize) 
			{
				Camera.main.orthographicSize += CameraSizeSpeed * Time.deltaTime;
				if (Camera.main.orthographicSize >= MaxCameraSize)
				{
					Camera.main.orthographicSize = MaxCameraSize;
				}
			}
			cameraPoint.target = fixedCameraPoint.transform;
			cameraPoint.smoothTime = 0.5f;
		}
	}

	// Update is called once per frame
	void Update() 
	{

		HandleReturningCameraSize ();
		
		if (m_RailCartActivated == true && m_RailCartFinished == false) 
		{
			
			m_PlayerObject.GetComponent<PlayerController>().DisableControls();
			HandleCameraAssignment();

			m_PlayerObject.transform.position = this.transform.position;

			if (m_Velocity < m_MaxVelocity) 
			{
				m_Velocity += 10.0f * Time.deltaTime;
			}
			else if (m_Velocity > m_MaxVelocity)
			{
				m_Velocity -= 10.0f * Time.deltaTime;
			}

			Vector3 position = this.transform.position;
			position = Vector3.MoveTowards(this.transform.position, m_CurrentWaypoint.transform.position, m_Velocity * Time.deltaTime );
			this.transform.position = position;

			if(this.transform.position == m_CurrentWaypoint.transform.position)
			{
				if (m_CurrentWaypoint.IsFinalWaypoint == false)
				{
					m_CurrentWaypoint = m_CurrentWaypoint.ChildWaypoint;
				}
				else if( m_CurrentWaypoint.IsFinalWaypoint == true )
				{
					m_RailCartFinished = true;
					m_CurrentWaypoint = StartingWaypoint;
					this.transform.position = StartingWaypoint.transform.position;
					cameraPoint.target = m_PlayerObject.transform;
					m_HasEndedRail = true;
					m_PlayerObject.GetComponent<PlayerController>().EnableControls();
					
				}
			}
		}

	}
}
