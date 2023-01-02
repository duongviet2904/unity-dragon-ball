using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabGO : MonoBehaviour
{
	
	public GameObject particalDamaged;
	[Header ("char player")]
	public GameObject[] players;
	[Header ("char AI")]
	public GameObject[] AIs;
	[Header ("bullet")]
	public GameObject bulletGoku, bulletBuu, bulletFatBuu, bulletSuperGoku, bulletGotenk;

	private static PrefabGO instance;

	public static PrefabGO GetInstance ()
	{
		return instance;
	}

	private PrefabGO ()
	{
		if (instance == null) {
			instance = this;
		}
	}
}
