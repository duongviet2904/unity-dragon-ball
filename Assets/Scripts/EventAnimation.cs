using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAnimation : MonoBehaviour
{


	public void ScaleSkill (GameObject obj)
	{
		iTween.PunchScale (obj, obj.transform.localScale + new Vector3 (7, 0, 0), 2f);
	}


	public void FinishFlash ()
	{
		PlayerControl player = GetComponent<PlayerControl> ();
		if (player != null)
			player.isFinshFlash = false;
	}

	public void AddJumpForce (float speed)
	{
		PlayerControl player = GetComponent<PlayerControl> ();
		if (player != null)
			player.gameObject.GetComponent<Rigidbody2D> ().velocity = new Vector2 (0, speed);
	}

	public void CallBullet ()
	{
		PlayerControl player = GetComponent<PlayerControl> ();
		AIControl AI = GetComponent<AIControl> ();
		if (player != null) {
			CallBulletPlayer (player);
		}
		if (AI != null) {
			CallBulletAI (AI);
		}
	}

	public void CanUsingSkillSpecial ()
	{
		AIControl AI = GetComponent<AIControl> ();
		if (AI != null)
			AI.isUsingSkillSpecial = false;
	}

	public void CallBulletPlayer (PlayerControl player)
	{
		
		Transform pos = player.transform.Find ("posCallBullet");
		string nameBullet = player.gameObject.name;
		GameObject temp = SelectBullet (nameBullet);
		GameObject bullet = Instantiate (temp, pos.position, Quaternion.identity) as GameObject;
		bullet.tag = "bulletPlayer";
		Vector3 direc = player.transform.localScale.x > 0 ? new Vector3 (20, 0, 0) : new Vector3 (-20, 0, 0);
		iTween.MoveBy (bullet, direc, 2f);
		Destroy (bullet, 2);

	}

	public void CallBulletAI (AIControl AI)
	{
		Transform pos = AI.transform.Find ("posCallBullet");
		string nameBullet = AI.gameObject.name;
		GameObject temp = SelectBullet (nameBullet);
		GameObject bullet = Instantiate (temp, pos.position, Quaternion.identity) as GameObject;
		bullet.tag = "bulletEnemy";
		Vector3 direc = AI.transform.localScale.x > 0 ? new Vector3 (20, 0, 0) : new Vector3 (-20, 0, 0);
		iTween.MoveBy (bullet, direc, 2f);
		Destroy (bullet, 2);
	}

	private GameObject SelectBullet (string nameBullet)
	{
		PrefabGO prefabs = PrefabGO.GetInstance ();
		nameBullet = nameBullet.Replace ("AI", "");
		switch (nameBullet) {
		case "Songoku":
			return prefabs.bulletGoku;
			break;
		case "Buu":
			return prefabs.bulletBuu;
			break;
		case "FatBuu":
			return prefabs.bulletFatBuu;
			break;
		case "Gotenk":
			return prefabs.bulletGotenk;
			break;
		case "SuperSongoku":
			return prefabs.bulletSuperGoku;
			break;
		}
		return prefabs.bulletGoku;
	}

}
