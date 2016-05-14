using UnityEngine;
using System.Collections;

public class Mob : MonoBehaviour {

	public float speed;
	public float range;
	public CharacterController controller;
	public Fighter hero;
	public LevelSystem playerLevel;
	public Transform player;
	private Animator anim;
	public int health;
	public int maxHealth;
	public int damage;
	private bool dead = false;
	private bool attacked = false;
	public int stunTime;
	public AudioSource attack1;
	public AudioSource attack2;
	public AudioSource attack3;
	public AudioSource pain1;
	public AudioSource pain2;
	public AudioSource pain3;
	public AudioSource death;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		hero = player.GetComponent<Fighter> ();
		health = maxHealth;
		// add ambient roars here, remember main camera ambiance and roars
	}
	
	// Update is called once per frame
	void Update () {

		if (!isDead ()) {
			//Debug.Log (stunTime);
			if (stunTime <= 0) {
				if (!inRange ()) {
					chase ();
				} else {
					anim.SetBool ("moving", false);
					if (!hero.isDead ()) {
						anim.SetTrigger ("attackEnemy");
						if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) {
							//Debug.Log (anim.GetCurrentAnimatorStateInfo (0).normalizedTime);
							if ((anim.GetCurrentAnimatorStateInfo (0).normalizedTime > .5 && !attacked) && inRange ()) {
								//animation finished and in range
								hero.getHit (damage);
								if(Random.Range(0f, 1f) < 0.33)
									attack1.Play ();
								else if(Random.Range(0f, 1f) < 0.66)
									attack2.Play ();
								else
									attack3.Play ();
								attacked = true;
							} else {
								//attacked = false;
							}
						} else {
							attacked = false;
						}
					}
				}
			}
		} 
		else {
			anim.SetTrigger ("dead");
			if (anim.GetCurrentAnimatorStateInfo (0).IsName ("Death")) {
				//Debug.Log (anim.GetCurrentAnimatorStateInfo (0).normalizedTime);
				if (anim.GetCurrentAnimatorStateInfo (0).normalizedTime > .5 && !dead) {
					//animation finished and in range
					death.Play();
					dieMethod();
					dead = true;
				} else {
					//attacked = false;
				}
			} else {
				dead = false;
			}
		}
	}

	bool inRange(){
		if (Vector3.Distance (transform.position, player.position) < range)
			return true;
		else
			return false;
	}

	bool isDead(){
		if (health <= 0) {
			health = 0;
			return true;
		}
		else
			return false;
	}

	void dieMethod()
	{
		playerLevel.exp = playerLevel.exp + 20;
		Destroy (gameObject, 5f);
	}

	public void getStunned(int seconds)
	{
		CancelInvoke ("stunCountDown");
		stunTime = stunTime + seconds;
		InvokeRepeating ("stunCountDown", 0f, 1f);
	}

	void stunCountDown()
	{
		stunTime = stunTime - 1;
		if (stunTime == 0)
			CancelInvoke ("stunCountDown");
	}

	void chase()
	{
		Vector3 lookAt = new Vector3 ( 0, controller.height, 0);
		lookAt = lookAt + player.position;
		transform.LookAt (lookAt);
		controller.SimpleMove (transform.forward * speed);
		anim.SetBool("moving", true);
	}

	void OnMouseOver()
	{
		player.GetComponent<Fighter>().opponent = gameObject;
	}

	public void getHit( double damage)
	{
		if (health > 0) {
			if(Random.Range(0f, 1f) < 0.33)
				pain1.Play ();
			else if(Random.Range(0f, 1f) < 0.66)
				pain2.Play ();
			else
				pain3.Play ();
			health = health - (int)damage;
		}
		//Debug.Log (health);
	}

}
