using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]

	public bool bMovementAllowed = true;
	public float moveForce = 365f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 5f;				// The fastest the player can travel in the x axis.
    public float runMultiplyer = 2f;
	public float jumpForce = 1000f;			// Amount of force added when the player jumps.
	public bool jump = false;				// Condition for whether the player should jump.

    public float minSize = 0.5f;
    public float maxSize = 1.4f;
    public float sizeChangeDelta = .1f;

	public float deathSize = 2.3f;
	public bool deathState = false;
	public float deathTimer = 0f;
	public Color deathColor;
	public float DeathColorChangeTime = 3.0f;
	public float deathMoveTimer = 3.0f;
	
	
	public float groundCheckOffset = 0.2f;

	private CircleCollider2D circCollider;
	private bool grounded;
	private Transform groundCheck;
//	private Animator anim;
    private ArrayList particles;
	
 

	void Awake()
	{
		// Setting up references.
        //groundCollision = new ArrayList(3);
		particles = new ArrayList(transform.childCount);
		for(int i = 0; i < transform.childCount;++i)
		{
			particles.Add(transform.GetChild(i));
		}
      //  groundCollision.Add(gameObject.transform.parent.gameObject.transform.Find("GroundCheckL"));
       // groundCollision.Add(gameObject.transform.parent.gameObject.transform.Find("GroundCheckC"));
        //groundCollision.Add(gameObject.transform.parent.gameObject.transform.Find("GroundCheckR"));
		//groundCheck = transform.Find("groundCheck");
		groundCheck = GameObject.FindGameObjectWithTag("GroundCheckPlayer").transform;
		circCollider = GetComponent<CircleCollider2D>();
//		anim = GetComponent<Animator>();
	}

	// Use this for initialization
	void Start () {
	
		bMovementAllowed = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(deathState)
		{
			DeathAnimation();
			return;
		}
		else if(!bMovementAllowed)
			return;
		
		groundCheck.transform.position = ( new Vector3(transform.position.x ,
        						 transform.position.y - circCollider.radius * transform.localScale.x - groundCheckOffset ,
        						 0));

		if (Input.GetButtonDown("Jump") && grounded)
			jump = true;
        if (Input.GetButton("Run"))
            Shrink();
        else
            Grow();
		
		if(Input.GetKeyDown(KeyCode.Backspace))
		{
			Spawn();
		}
	}

	void FixedUpdate ()
	{
		
        grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            
		if (!bMovementAllowed)
			return;
		
		float h = Input.GetAxis("Horizontal");

        if (Input.GetButton("Run"))
        {
            h *= runMultiplyer;
            
        }
		
		GetComponent<Rigidbody2D>().velocity = new Vector2(h * maxSpeed, GetComponent<Rigidbody2D>().velocity.y);

        //if (h > 0 && !facingRight)
        //    Flip();
        //else if (h < 0 && facingRight)
        //    Flip();

		if (jump)
		{
			// Set the Jump animator trigger parameter.
			// Add a vertical force to the player.
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpForce));

			jump = false;
		}
	}

        void Shrink()
    {
        if(transform.localScale.x > minSize)
		{
			transform.localScale -= new Vector3(sizeChangeDelta * Time.deltaTime,
				sizeChangeDelta * Time.deltaTime,	
				0);
			foreach(Transform part in particles)
			{
				part.GetComponent<SpringJoint2D>().distance -= sizeChangeDelta *Time.deltaTime;
			}
		}
    }

    void Grow()
    {
      
		transform.localScale += new Vector3(sizeChangeDelta * Time.deltaTime,
			sizeChangeDelta * Time.deltaTime,	
			0);	
		
		foreach(Transform part in particles)
		{
			part.GetComponent<SpringJoint2D>().distance += sizeChangeDelta *Time.deltaTime;
		}
		
		if(transform.localScale.x > deathSize)
		{
			deathTimer = 0;
			deathState = true;
		}
    }
	
	void Spawn()
	{
		transform.localScale = new Vector3(1.0f, 1.0f, 0.0f);
		GetComponent<Rigidbody2D>().velocity = new Vector3();
		transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;	
		GetComponent<SpriteRenderer>().color = Color.white;
		
		bMovementAllowed = true;
		GetComponent<Rigidbody2D>().gravityScale = 1;
		circCollider.enabled = true;
		foreach(Transform part in particles)
		{
			part.GetComponent<SpriteRenderer>().color = Color.white;
			part.GetComponent<CircleCollider2D>().enabled = true;
			part.transform.position = GameObject.FindGameObjectWithTag("SpawnPoint").transform.position;	
		}
		
		deathTimer = 0;
		deathState = false;
	}

	void Flip()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public void EnableControls()
	{
		bMovementAllowed = true;
	}
	public void DisableControls()
	{
		bMovementAllowed = false;	
	}
	
	public void DeathAnimation()
	{
		bMovementAllowed = false;
		GetComponent<Rigidbody2D>().gravityScale = 0;
		foreach(Transform part in particles)
		{
			part.GetComponent<Rigidbody2D>().gravityScale = 0;
		}
		if(deathTimer < DeathColorChangeTime)
		{
			morphColor();
			GetComponent<Rigidbody2D>().velocity = new Vector2();
		}
		if(deathTimer > DeathColorChangeTime &&
			deathTimer < deathMoveTimer + DeathColorChangeTime)
		{
			circCollider.enabled = false;
			
			transform.position = (transform.position - new Vector3(0.0f, .8f * Time.deltaTime, 0));
		}
		else if(deathTimer > deathMoveTimer + DeathColorChangeTime )
		{
			Spawn();
			Application.LoadLevel(Application.loadedLevel);
		}
		deathTimer += Time.deltaTime;
	}
	
	void morphColor()
	{
		Color change = GetColor(deathTimer, .3f, 127f, 128);
			GetComponent<SpriteRenderer>().color = change;
			foreach(Transform part in particles)
			{
				part.GetComponent<SpriteRenderer>().color = change;
			
				part.GetComponent<CircleCollider2D>().enabled = false;
			}
	}
	
	static Color GetColor(float time, float freq1, float w, float c )
	{
		return getColorCycleFull(time, freq1, freq1, freq1, w, c);
	}
	
	static Color getColorCycleFull(float increment, float freqR , float freqG, float freqB , float amplitude, float center)
	{
		Color retCol = new Color((Mathf.Sin(freqR * increment + 0) * amplitude + center)/255.0f,
			(Mathf.Sin(freqG * increment + 2) * amplitude + center)/255.0f,
			(Mathf.Sin(freqB * increment + 4) * amplitude + center)/255.0f,
			1.0f);
		//retCol.r = Mathf.Sin(freqR * increment + 0) * amplitude + center;
		//retCol.b = Mathf.Sin(freqG * increment + 2) * amplitude + center;
		//retCol.g = Mathf.Sin(freqB * increment + 4) * amplitude + center;
		//retCol.a = 255;
		//Debug.Log(retCol);
		return retCol;
	}
}
