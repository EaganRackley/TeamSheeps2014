using UnityEngine;
using System.Collections;

public class AssignRandomGravity : MonoBehaviour {

	private bool m_GravitySet = false;

	void Start()
	{
		m_GravitySet = false;
	}

	public void SetRandomGravity()
	{
		float randomNumber = Random.Range (0.25f, 1.0f);
		this.GetComponent<Rigidbody2D>().gravityScale = randomNumber;
		m_GravitySet = true;
	}

	void Update()
	{
		if (m_GravitySet) 
		{
			//Vector3 rotation = new Vector3(10.0f * Time.deltaTime, 0, 0);
			//transform.Rotate(rotation);
			//transform.Rotate(new Vector3(0f, 0f, 0.1f));
		}
	}

}
