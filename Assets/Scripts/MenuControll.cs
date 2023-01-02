using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControll : MonoBehaviour
{
	public static int indexChara { get; set; }
	private static MenuControll instance;

	public static MenuControll GetIntance()
	{
		return instance;
	}

	private MenuControll()
	{
		if (instance == null)
		{
			instance = this;
		}
	}

	[Header("mui ten khi chon")]
	public GameObject selectObj;
	[Header("mang hinh char")]
	public GameObject[] iconChar;

	public string nameCharacter { get; set; }

	// Use this for initialization
	void Start()
	{
		indexChara = 0;
		iconChar[0].SetActive(true);
		DontDestroyOnLoad(this);

	}

	// Update is called once per frame
	void Update()
	{

	}

	public void SelectCharacter()
	{
		var btnChar = EventSystem.current.currentSelectedGameObject;
		if (btnChar != null)
		{
			selectObj.transform.position = btnChar.transform.position + new Vector3(0, 64, 0);
			nameCharacter = btnChar.name;
			SetActiveIcon();
			switch (btnChar.name)
			{
				case "SuperGoku":
					indexChara = 0;
					iconChar[0].SetActive(true);
					break;
				case "Goku":
					indexChara = 1;
					iconChar[5].SetActive(true);
					break;
				//case "Cell":
				//	indexChara = 2;
				//	iconChar [2].SetActive (true);
				//break;
				case "Gotenk":
					indexChara = 2;
					iconChar[3].SetActive(true);
					break;
				case "FatBuu":
					indexChara = 3;
					iconChar[4].SetActive(true);
					break;
				case "Buu":
					indexChara = 4;
					iconChar[1].SetActive(true);
					break;
					//case "Android":
					//	indexChara = 6;
					//	iconChar [6].SetActive (true);
					//	break;
					//case "Gero":
					//	iconChar [7].SetActive (true);
					//	break;
					//case "Vegeta":
					//	iconChar [8].SetActive (true);
					//	break;
					//case "SuperGohan":
					//	iconChar [9].SetActive (true);
					//	break;
					//case "Trunk":
					//	iconChar [10].SetActive (true);
					//	break;
					//case "Frieza":
					//	iconChar [11].SetActive (true);
					//	break;
			}
		}
	}

	private void SetActiveIcon()
	{
		for (int i = 0; i < iconChar.Length; i++)
		{
			if (iconChar[i].activeInHierarchy)
			{
				iconChar[i].SetActive(false);
			}
		}
	}

	public GameObject bgLoading;
	public GameObject canvas;

	private IEnumerator Loading()
	{
		canvas.SetActive(false);
		bgLoading.gameObject.SetActive(true);
		for (float i = 0; i < 1f; i += 0.3f)
		{
			var colorTemp = bgLoading.GetComponent<SpriteRenderer>().color;
			colorTemp.a = i;
			bgLoading.GetComponent<SpriteRenderer>().color = colorTemp;
			yield return new WaitForSeconds(0.1f);
		}
		//bgLoading.gameObject.SetActive (false);
	}

	public void ClickBtnStart()
	{
		canvas.SetActive(false);
		bgLoading.SetActive(false);
		SceneManager.LoadSceneAsync("Map");
	}
}
