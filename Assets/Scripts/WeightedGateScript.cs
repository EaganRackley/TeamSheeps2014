using UnityEngine;
using System.Collections;

public class WeightedGateScript : MonoBehaviour 
{
	public CircleCollider2D ColliderA;
	public CircleCollider2D ColliderB;
	public CircleCollider2D ColliderC;
	public CircleCollider2D ColliderD;
	public PlayerController PlayerObject;
	public bool playedWobbleSound = false;
	public bool playedCollapseSound = false;

	private bool m_Triggered = false;

	void Start()
	{
		m_Triggered = false;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player") 
		{
			m_Triggered = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player") 
		{
			m_Triggered = false;
		}
	}

	void Update()
	{
		if (m_Triggered) 
		{
			//print ("Player Scale: " + PlayerObject.transform.localScale.x);
			Animator animator = GetComponent<Animator>();
			if(PlayerObject.transform.localScale.x >= 1.2f && PlayerObject.transform.localScale.x < 1.4f)
			{
				animator.SetBool("Wobble", true);
				if (playedWobbleSound == false) {
					//transform.GetComponentsInChildren<AudioSource>()[0].audio.Play();
					playedWobbleSound = true;
				}
			}
			else if(PlayerObject.transform.localScale.x >= 1.4f)
			{
				animator.SetBool("Wobble", false);
				animator.SetBool("Collapsed", true);
				ColliderA.isTrigger = true;
				ColliderB.isTrigger = true;
				ColliderC.isTrigger = true;
				ColliderD.isTrigger = true;
				if (playedCollapseSound == false) {
					transform.GetComponentsInChildren<AudioSource>()[0].GetComponent<AudioSource>().Play();
					playedCollapseSound = true;
				}
			}
			else if(PlayerObject.transform.localScale.x >= 0.2f)
			{
				animator.SetBool("Collapsed", false);
				animator.SetBool("Wobble", false);
			}
		}
	}


}
