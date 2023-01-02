using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StateEnemy
{
	normal,
	close,
	damaged
}

public class EnemyControl : MonoBehaviour
{

	public StateEnemy stateEnemy{ get; set; }

	private PlayerControl playerScript;

	private static EnemyControl instance;

	public static EnemyControl GetInstance ()
	{
		return instance;
	}

	private EnemyControl ()
	{
		if (instance == null) {
			instance = this;
		}
	}

	private Animator aniAI;
	private Rigidbody2D rigAI;
	private int curPower, curHealthy;

	private float distancePE;
	public float distanceToAtt;
	private float scaleX, scaleY, gravity;
	private bool isGround, isDamaged, isPower;
	private bool isAttack;
	private float timePress;

	
	// Use this for initialization
	void Start ()
	{
		playerScript = PlayerControl.GetInstance ();
		stateEnemy = StateEnemy.normal;

		scaleX = transform.localScale.x;
		scaleY = transform.localScale.y;
		//curPower = Config.powerEnemy;
		//curHealthy = Config.healthyEnemy;

		distancePE = Vector3.Distance (playerScript.gameObject.transform.position, this.transform.position);
		aniAI = GetComponent<Animator> ();
		rigAI = GetComponent<Rigidbody2D> ();
		gravity = rigAI.gravityScale;
		//loai bo va cham
		Physics2D.IgnoreCollision (playerScript.gameObject.GetComponent<Collider2D> (), GetComponent<Collider2D> ());
		//kiem tra huong va khoang cach
		InvokeRepeating ("CheckRotation", 1f, 1f);
		InvokeRepeating ("CheckDistance", 1f, 1f);
	}

	private void CheckDistance ()
	{
		if (stateEnemy != StateEnemy.damaged) {
			distancePE = Vector3.Distance (playerScript.gameObject.transform.position, this.transform.position);
			if (stateEnemy != StateEnemy.damaged && isGround) {
				if (distancePE < distanceToAtt) {
					stateEnemy = StateEnemy.close;
					aniAI.SetFloat ("speed", 0);
					//StartCoroutine (AttackAI ());
				} else {
					stateEnemy = StateEnemy.normal;
				}
			}
		}
	}

	public Transform posCallDamaged;

	//kiem tra da chay vao ResetDamaged() chua
	private bool checkCallResetDamaged;

	private void Damaged (float x, float y)
	{
		isDamaged = true;
		stateEnemy = StateEnemy.damaged;
		//healthy
		//curHealthyP -= Config.damagedEnemy;
		//iTween.ScaleTo (sliderHealthy, sliderHealthy.transform.localScale + new Vector3 (-curHealthyP * 2.6f / 100, 0, 0), 2f);
		//quay chieu
		GameObject prefab = Instantiate (PrefabGO.GetInstance ().particalDamaged, posCallDamaged.position, Quaternion.identity) as GameObject;
		prefab.transform.parent = posCallDamaged.transform;
		Destroy (prefab, 0.3f);

		if (transform.position.x > playerScript.gameObject.transform.position.x) {
			transform.localScale = new Vector3 (-scaleX, scaleY, 0);
			rigAI.gravityScale = 0.08f;
			rigAI.velocity = Vector2.zero;
			rigAI.velocity = new Vector2 (x, y);

		} else {
			transform.localScale = new Vector3 (scaleX, scaleY, 0);
			rigAI.gravityScale = 0.08f;
			rigAI.velocity = Vector2.zero;
			rigAI.velocity = new Vector2 (-x, y);
		}
		//ani
		if (!checkCallResetDamaged) {
			Invoke ("ResetDamaged", 1f);
		}
	}

	private void ResetDamaged ()
	{
		checkCallResetDamaged = true;
		isDamaged = false;
		rigAI.velocity = Vector2.zero;
		rigAI.gravityScale = gravity;
		stateEnemy = StateEnemy.normal;
		checkCallResetDamaged = false;
	}

	private void CheckRotation ()
	{

		//khi khong bi thuong thi huong ve player
		if (stateEnemy != StateEnemy.damaged && isGround) {
			if (transform.position.x > playerScript.gameObject.transform.position.x) {
				transform.localScale = new Vector3 (-scaleX, scaleY, 0);
			} else {
				transform.localScale = new Vector3 (scaleX, scaleY, 0);
			}
		}
	}

	//	private IEnumerator AttackAI ()
	//	{
	//		//neu power du su dung ki nang thi dung
	//		if (curPower < Config.powerDamaged1) {
	//			int randAttack = Random.Range (0, 3);
	//			switch (randAttack) {
	//			case 0:
	//				{
	//					aniAI.SetTrigger ("hit");
	//					rigAI.velocity = transform.position.x < playerScript.gameObject.transform.position.x
	//				? new Vector2 (10, rigAI.velocity.y) : new Vector2 (-10, rigAI.velocity.y);
	//					if (isDamaged) {
	//						yield  break;
	//					}
	//					yield return new WaitForSeconds (1f);
	//					aniAI.SetTrigger ("hit");
	//					rigAI.velocity = transform.position.x < playerScript.gameObject.transform.position.x
	//				? new Vector2 (10, rigAI.velocity.y) : new Vector2 (-10, rigAI.velocity.y);
	//					if (isDamaged) {
	//						yield  break;
	//					}
	//					yield return new WaitForSeconds (1f);
	//					aniAI.SetTrigger ("hit");
	//					rigAI.velocity = transform.position.x < playerScript.gameObject.transform.position.x
	//				? new Vector2 (10, rigAI.velocity.y) : new Vector2 (-10, rigAI.velocity.y);
	//				}
	//				break;
	//			case 1:
	//				{
	//					aniAI.SetTrigger ("kick");
	//					rigAI.velocity = transform.position.x < playerScript.gameObject.transform.position.x
	//				? new Vector2 (10, rigAI.velocity.y) : new Vector2 (-10, rigAI.velocity.y);
	//					if (isDamaged) {
	//						yield  break;
	//					}
	//					yield return new WaitForSeconds (1f);
	//					aniAI.SetTrigger ("kick");
	//					rigAI.velocity = transform.position.x < playerScript.gameObject.transform.position.x
	//				? new Vector2 (10, rigAI.velocity.y) : new Vector2 (-10, rigAI.velocity.y);
	//					if (isDamaged) {
	//						yield  break;
	//					}
	//					yield return new WaitForSeconds (1f);
	//					aniAI.SetTrigger ("kick");
	//					rigAI.velocity = transform.position.x < playerScript.gameObject.transform.position.x
	//				? new Vector2 (10, rigAI.velocity.y) : new Vector2 (-10, rigAI.velocity.y);
	//				}
	//				break;
	//			case 2:
	//				if (isDamaged) {
	//					yield  break;
	//				}
	//				if (curPower >= Config.powerDamaged2) {
	//					curPower -= Config.powerDamaged2;
	//					aniAI.SetTrigger ("superKick");
	//				}
	//				break;
	//			}
	//		}
	//		//uu tien dung power
	//		else {
	//			if (isDamaged) {
	//				yield  break;
	//			}
	//			float randPower = Random.value;
	//			if (randPower > 0.5f) {
	//				if (curPower >= Config.powerDamaged1) {
	//					curPower -= Config.powerDamaged1;
	//					aniAI.SetTrigger ("superHit");
	//				}
	//			} else {
	//				isPower = true;
	//			}
	//		}
	//	}

	private void Move ()
	{
		aniAI.SetFloat ("speed", 1);
		CheckRotation ();
		//transform.position = Vector3.MoveTowards (this.transform.position, playerScript.gameObject.transform.position, 4 * Time.deltaTime);
		if ((transform.position.x > playerScript.transform.position.x + distanceToAtt) && !isDamaged && isGround) {
			if (isDamaged || !isGround) {
				return;
			}
			transform.Translate (-Vector2.right * 4 * Time.deltaTime);
		} else if ((transform.position.x < playerScript.transform.position.x - distanceToAtt) && !isDamaged && isGround) {
			if (isDamaged || !isGround) {
				return;
			}
			transform.Translate (Vector2.right * 4 * Time.deltaTime);
		}
	}


	public float jumpX, jumpY;

	private void Jump ()
	{
		isDamaged = false;
		aniAI.SetBool ("isDamaged", isDamaged);
		aniAI.SetTrigger ("jump");
		//kiem tra xem player huong nao de lui lai
		rigAI.velocity = Vector2.zero;
		rigAI.velocity = transform.position.x < playerScript.gameObject.transform.position.x ? new Vector2 (jumpX, jumpY) : new Vector2 (-jumpX, jumpY);
	}

	private float timeNormal;

	//	void Update ()
	//	{
	//		aniAI.SetBool ("isGround", isGround);
	//		aniAI.SetBool ("isDamaged", isDamaged);
	//		aniAI.SetInteger ("power", curPower);
	//		aniAI.SetBool ("isPower", isPower);
	//		aniAI.SetFloat ("timePress", timePress);
	//
	//		if (countDamaged >= 4 && isGround) {
	//			countDamaged = 0;
	//			//nhay
	//			Jump ();
	//		}
	//		if (stateEnemy == StateEnemy.normal && isGround) {
	//			timeNormal += Time.deltaTime;
	//		}
	//		if (timeNormal > 2f) {
	//			Move ();
	//			timePress = 0;
	//		}
	//		if (isPower) {
	//			timePress += Time.deltaTime;
	//		}
	//		if (timePress > 3f) {
	//			if (curPower >= Config.powerDamaged1) {
	//				curPower -= Config.powerDamaged1;
	//			}
	//			isPower = false;
	//			timePress = 0;
	//		}
	//	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.name == "ground") {
			isGround = true;
		}
	}

	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.name == "ground") {
			isGround = false;
		}
	}

	private int countDamaged = 0;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.name == "hitP") {
			Damaged (0.5f, 1f);
			countDamaged++;
		} 
		if (other.gameObject.name == "kickP") {
			Damaged (0.5f, 1f);
			countDamaged++;
		}
		if (other.gameObject.name == "Scale") {
			Damaged (2, 3);
			countDamaged++;
		}
		if (other.gameObject.name == "superKick") {
			playerScript.rib.velocity = new Vector2 (0, 20);
			playerScript.ani.SetTrigger ("jump");
			//playerScript.rib.gravityScale = 0;
			Damaged (0, 20);
			//jump
			countDamaged++;
		}

	}

	public Transform posCallBullet;

	public void InstantiateBullet ()
	{
		GameObject bullet = Instantiate (PrefabGO.GetInstance ().bulletBuu, posCallBullet.position, Quaternion.identity) as GameObject;
		Vector3 dire = EnemyControl.GetInstance ().transform.localScale.x < 0 ? new Vector3 (transform.position.x - 25f, 0, 0) : new  Vector3 (transform.position.x + 25f, 0, 0);
		iTween.MoveBy (bullet, dire, 4);
		Destroy (bullet, 4f);
	}

}
