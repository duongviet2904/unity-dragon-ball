using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XXX.UI.Popup;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupLose : BasePopup
{
    //public Button btn_nextLevel;
    public override void Initialized(object data = null)
    {

        base.Initialized(data);
        StartCoroutine(WaitShowMenu());
       // btn_nextLevel.onClick.RemoveAllListeners();
        //btn_nextLevel.onClick.AddListener(OnClickNextLevel);
        
    }
    //private void OnClickNextLevel()
    //{
    //    Close();
    //    Time.timeScale = 1;
    //    SceneManager.LoadScene("Map");
        
    //}
    private IEnumerator WaitShowMenu()
    {
        yield return new WaitForSeconds(1);
        Close();
        SceneManager.LoadScene("Menu");

    }
}
