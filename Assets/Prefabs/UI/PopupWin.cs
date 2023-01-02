using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XXX.UI.Popup;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PopupWin : BasePopup
{
    public Button btn_nextLevel;
    public override void Initialized(object data = null)
    {
        Time.timeScale = 0;
        base.Initialized(data);
        btn_nextLevel.onClick.RemoveAllListeners();
        btn_nextLevel.onClick.AddListener(OnClickNextLevel);
        
    }
    private void OnClickNextLevel()
    {
        Close();
        Time.timeScale = 1;
        SceneManager.LoadScene("Map");
        
    }
}
