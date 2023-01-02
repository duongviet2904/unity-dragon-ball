using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapControll : MonoBehaviour
{

    private static MapControll instance;

    public static MapControll GetIntance()
    {
        return instance;
    }

    private MapControll()
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

    public string nameMap { get; set; }

    // Use this for initialization
    void Start()
    {
        iconChar[0].SetActive(true);
        DontDestroyOnLoad(this);

    }

    // Update is called once per frame
    void Update()
    {

    }
    Vector3 vector3 = new Vector3(0, 20, 0);
    private string level = "Level1";
    public void SelectCharacter()
    {
        var btnChar = EventSystem.current.currentSelectedGameObject;
        if (btnChar != null)
        {
            selectObj.transform.localPosition = btnChar.transform.localPosition + new Vector3(0,20,0);
            nameMap = btnChar.name;
            /*SetActiveIcon();*/
            switch (btnChar.name)
            {
                case "Level1":
                    iconChar[0].SetActive(true);
                    level = btnChar.name;
                    break;
                case "Level2":
                    iconChar[1].SetActive(true);
                    level = btnChar.name;
                    break;
                case "Level3":
                    iconChar[2].SetActive(true);
                    level = btnChar.name;
                    break;
                case "Level4":
                    iconChar[3].SetActive(true);
                    level = btnChar.name;
                    break;
                default:
                    level = "Level1";
                    break;
            }
        }
    }

    /*private void SetActiveIcon()
    {
        for (int i = 0; i < iconChar.Length; i++)
        {
            if (iconChar[i].activeInHierarchy)
            {
                iconChar[i].SetActive(false);
            }
        }
    }*/

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
        SceneManager.LoadSceneAsync(level);
    }
}
