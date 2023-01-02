using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIControl : MonoBehaviour
{
	public enum AIState
	{
		Normal,
		Close,
		Damaged,
		Stop,
		Reset
	}

	private AIState state;
	private Animator ani;
	private Rigidbody2D rib;

	[Header ("healthy")]
	public int healthy = 100;
	[Header ("speed")]
	public float speedMove = 5, speedJump = 26;
	[Header ("damagedHealthy")]
	public int damagedHit = 10;
	public int damagedKick = 10;
	public int damagedKick2 = 15;
	public int damagedSuperHit = 40;
	public int damagedShoot = 5;
	[Header ("power")]
	public int power = 100;
	[Header ("damagedPower")]
	public int powerHit = 60;
	public int powerShoot = 40;
	[Header ("AI state")]
	[Range (0f, 5f)]
	public float normalRate = 0.5f, closeRate = 0.5f, stopRate = 3f, lifeRateAI = 1;
	private float timerNormal, timerClose, timerAI, timerStop;
	private int initAI;
	[Header ("k/c danh")]
	public float distanceAtt = 3f;
	private float scaleX, scaleY, gravity;
	public int curHealthyEnemy, curPowerEnemy;

	[Header ("speed move")]
	public float moveSpeed = 4;

	private PlayerControl playerScript;
	private static AIControl instance;

	private GameManager gameManager;


	public static AIControl GetInstance ()
	{
		return instance;
	}

	private AIControl ()
	{
		if (instance == null) {
			instance = this;
		}
	}
	// Use this for initialization
	void Start ()
	{
		state = AIState.Normal;

		//ani = transform.FindChild ("Animator").gameObject.GetComponent<Animator> ();
		ani = GetComponent<Animator> ();
		rib = GetComponent<Rigidbody2D> ();

		scaleX = transform.localScale.x;
		scaleY = transform.localScale.y;
		gravity = rib.gravityScale;

		curPowerEnemy = power;
		curHealthyEnemy = healthy;
		gameManager = GameManager.GetIntance ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (GameManager.GetIntance().state != GameState.Playing)
		{
			return;
		}
		if (playerScript == null) {
			playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControl> ();
			Physics2D.IgnoreCollision (playerScript.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
		}
		if (!isDamaged) {
			//kiem tra khoang cach de chon state close hoac normal
			CheckDistance ();
			//goi state
			State ();

			//dua vao state de attack hoac move
			AIManager ();
		}
		//khi bi damaged
		Blocking ();
		//
		DamagedCombo ();
		//animation
		ChangeAnimator ();

	}


	private void CheckDistance ()
	{
		float distancePE = Vector3.Distance (transform.position, playerScript.transform.position);

		if (distancePE > distanceAtt) {
			if (state != AIState.Reset && state != AIState.Damaged && !isJumping) {
				state = AIState.Normal;
				Movement ();

			}
		} else {
			if (state != AIState.Reset && state != AIState.Damaged && !isJumping) {
				state = AIState.Close;
				speed = 0;
			}
		}
	}


	private void State ()
	{
		switch (state) {
		case AIState.Normal:
			NormalState ();
			break;
		case AIState.Close:
			CloseState ();
			break;
		case AIState.Stop:
			StopState ();
			break;
		case AIState.Reset:
			ResetState ();
			break;
		}

		//tranh
		if (countDamaged > 5 && isGround && !isDamaged) {
			Dodge ();
			countDamaged = 0;
		}
	}

	private float timerAttack;
	public float timeAttack = 2f;

	private void AIManager ()
	{
//		if (initAI) {
//			state = AIState.Reset;
//			float rand = Random.value;
//			if (rand > 0.5f) {
//				//move
//				Movement ();
//			} else {
//				//attack
//				timerAttack += Time.deltaTime;
//				if (timerAttack > timeAttack) {
//					Attack ();
//					timerAttack = 0;
//				}
//			}
//
//		}
		float rand = Random.value;
		switch (initAI) {
		//normal
		case 0:
			{
				if (rand > 0.5f) {
					//move
					Movement ();
				} else {
					//attack
					timerAttack += Time.deltaTime;
					if (timerAttack > timeAttack) {
						if (rand < 0.25f)
							Attack ();
						else
							SpecicalAttack ();
						timerAttack = 0;
					}
				}
			}
			break;	
		//attack
		case 1:
			{
				timerAttack += Time.deltaTime;
				if (timerAttack > timeAttack) {
					Attack ();
					timerAttack = 0;
				}
			}
			break;
		//stop
		case 2:
			{
				SpecicalAttack ();
			}
			break;
		}

	}



	private void NormalState ()
	{
		timerNormal += Time.deltaTime;
		if (timerNormal > normalRate) {
			initAI = 0;
			timerNormal = 0;
		}
	}

	private void CloseState ()
	{
		timerClose += Time.deltaTime;
		if (timerClose > closeRate) {
			initAI = 1;
			timerClose = 0;
		}
	}

	private void StopState ()
	{
		timerStop += Time.deltaTime;
		if (timerStop > stopRate) {
			initAI = 2;
			timerStop = 0;
		}
	}

	private void ResetState ()
	{
		timerAI += Time.deltaTime;
		if (timerAI > lifeRateAI) {
			initAI = 3;
			timerAI = 0;
			float rand = Random.value;
			if (rand > 0.5f) {
				if (rand > 0.25f) {
					state = AIState.Normal;
				} else {
					state = AIState.Stop;
				}
			} else
				state = AIState.Close;
		}
	}



	private void Attack ()
	{
		int randAttack = Random.Range (0, 3);
		switch (randAttack) {
		case 0:
			{
				ani.SetTrigger ("hit");
				rib.velocity = Vector2.zero;
				rib.velocity = transform.position.x < playerScript.gameObject.transform.position.x 
					? new Vector2 (10, 0) : new Vector2 (-10, 0);
			}
			break;
		case 1:
			{
				ani.SetTrigger ("kick");
				rib.velocity = Vector2.zero;
				rib.velocity = transform.position.x < playerScript.gameObject.transform.position.x 
					? new Vector2 (10, 0) : new Vector2 (-10, 0);
			}
			break;
//		case 2:
//			if (curPower >= powerHit) {
//				curPower -= powerHit;
//				ani.SetTrigger ("super");
//			}
//			break;
		}
	}

	public bool isUsingSkillSpecial;


	private void SpecicalAttack ()
	{
		isUsingSkillSpecial = true;
		float rand = Random.value;
		//super hit
		if (rand > 0.5f) {
			if (curPowerEnemy >= powerHit) {
				curPowerEnemy -= powerHit;
				ani.SetTrigger ("superHit");
			}
		} 
		//shoot
		else {
			if (curPowerEnemy >= powerShoot) {
				curPowerEnemy -= powerShoot;
				ani.SetTrigger ("shoot");
			}
		}
	}

	private bool isGround, isPower;
	[HideInInspector]public bool isDamaged;
	private float timePress, speed;

	private void ChangeAnimator ()
	{
		ani.SetFloat ("speed", speed);
		ani.SetBool ("isGround", isGround);
		ani.SetBool ("isDamaged", isDamaged);
		ani.SetInteger ("power", curPowerEnemy);
		ani.SetBool ("isPower", isPower);
		// ani.SetFloat ("timePress", timePress);
	}

	private void Movement ()
	{
		if (!isUsingSkillSpecial && !isDamaged) {
			speed = 1;
			CheckRotation ();
			if (transform.position.x > playerScript.transform.position.x + distanceAtt) {
				transform.Translate (-Vector2.right * moveSpeed * Time.deltaTime);
			} else if (transform.position.x < playerScript.transform.position.x - distanceAtt) {
				transform.Translate (Vector2.right * moveSpeed * Time.deltaTime);
			}
		
		}
	}

	private void CheckRotation ()
	{
		if (transform.position.x > playerScript.gameObject.transform.position.x) {
			transform.localScale = new Vector3 (-scaleX, scaleY, 0);
		} else {
			transform.localScale = new Vector3 (scaleX, scaleY, 0);
		}
	}

	public float jumpSpeed = 28;

	private void Jump (float velocityX)
	{
		ani.SetTrigger ("jump");
		isJumping = true;
		rib.velocity = Vector2.zero;
		rib.velocity = new Vector2 (velocityX, jumpSpeed);
	}

	private float timerDamaged;
	private float damagedRate = 1;
	private bool isJumping;

	private void Blocking ()
	{
		if (isDamaged) {
			state = AIState.Damaged;
			timerDamaged += Time.deltaTime;
		
			if (rib.gravityScale < gravity) {
				rib.gravityScale += Time.deltaTime;
			}

			if (timerDamaged > damagedRate && isGround) {
				isDamaged = false;
				timerDamaged = 0;
				countDamagedCombo = 0;
				state = AIState.Normal;
				rib.gravityScale = gravity;
			}
		}
	}

	private void DamagedCombo ()
	{
		if (countDamagedCombo >= 3) {
			countDamagedCombo = 0;
			rib.velocity = Vector2.zero;
			if (transform.position.x > playerScript.gameObject.transform.position.x) {
				//rib.velocity = new Vector2 (6, 2);
				iTween.MoveBy (this.gameObject, new Vector2 (6, -3.5f - transform.position.y), 1f);
			} else {
				//rib.velocity = new Vector2 (-6, 2);
				iTween.MoveBy (this.gameObject, new Vector2 (-6, -3.5f - transform.position.y), 1f);
			}
		
		}
	}

	private int countDamagedCombo = 0;

	private void CreateEffectDamaged (float velocityX, float velocityY)
	{
		GameObject prefab = Instantiate (PrefabGO.GetInstance ().particalDamaged, transform.position, Quaternion.identity) as GameObject;
		prefab.transform.parent = this.transform;
		Destroy (prefab, 0.3f);
		rib.gravityScale = 0;
		//damaged combo
		countDamagedCombo++;
		//quay chieu
		rib.velocity = Vector2.zero;
		if (transform.position.x > playerScript.gameObject.transform.position.x) {
			transform.localScale = new Vector3 (-scaleX, scaleY, 0);
			rib.velocity = new Vector2 (velocityX, velocityY);
		} else {
			transform.localScale = new Vector3 (scaleX, scaleY, 0);
			rib.velocity = new Vector2 (-velocityX, velocityY);
		}
	}
	//tranh ne
	private void Dodge ()
	{
		if (transform.position.x > playerScript.gameObject.transform.position.x) {
			//jump
			Jump (8f);
			state = AIState.Stop;
		} else {
			Jump (-8f);
			state = AIState.Stop;
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.name == "ground") {
			isGround = true;
			isJumping = false;
		}
	}

	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.name == "ground") {
			isGround = false;
		}
	}

	private int countDamaged = 0;
	private bool isDamagedKick;
	private int countHit;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (!playerScript.isDamaged) {

			if (other.gameObject.CompareTag("hitP")) {
				isDamaged = true;
				damagedRate = 0.5f;
				CreateEffectDamaged (0.5f, 1f);
				countDamaged++;
				gameManager.UpdateHealthyEnemy (ref curHealthyEnemy, ref damagedHit,ref playerScript.curPower, playerScript.power, 1);
			
			} 
			if (other.gameObject.tag == "kickP") {
				isDamaged = true;
				damagedRate = 0.5f;
				CreateEffectDamaged (0.5f, 1f);
				countDamaged++;
				gameManager.UpdateHealthyEnemy (ref curHealthyEnemy, ref damagedKick,ref playerScript.curPower, playerScript.power, 1);

			}
			if (other.gameObject.tag == "scale") {
				isDamaged = true;
				damagedRate = 0.5f;
				CreateEffectDamaged (2, 3f);
				countDamaged++;
				gameManager.UpdateHealthyEnemy (ref curHealthyEnemy, ref damagedSuperHit,ref playerScript.curPower, playerScript.power, 1);

			}
			if (other.gameObject.name == "kick2") {
				isDamaged = true;
				damagedRate = 0.6f;
				CreateEffectDamaged (1f, 1f);
				countDamaged++;
				gameManager.UpdateHealthyEnemy (ref curHealthyEnemy, ref damagedKick2,ref playerScript.curPower, playerScript.power, 1);

			}

			if (other.gameObject.tag == "bulletPlayer") {
				isDamaged = true;
				damagedRate = 1f;
				gameManager.UpdateHealthyEnemy (ref curHealthyEnemy, ref damagedKick2,ref playerScript.curPower, playerScript.power, 0);
				Destroy (other.gameObject);
				CreateEffectDamaged (0.5f, 1f);
			}
		}

	}


}
