using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Reflection;

namespace XXX.UI.Popup
{
    public class BasePopup : MonoBehaviour, IPopup
    {
        #region Properties
        [Header("BasePopup")]
        [SerializeField] private Canvas _canvas;
        #endregion

        #region Has Button Close
        [SerializeField] private bool _hasBtnClose;
        [SerializeField] private Button _btnClose;
        private void RegiterButtonClose()
        {
            if (!_hasBtnClose) return;
            _btnClose.onClick.RemoveAllListeners();
            _btnClose.onClick.AddListener(OnClickButtonClose);
        }
        private void OnClickButtonClose()
        {
            Close();
        }
        #endregion

        #region Has Animation Show
        [SerializeField] private bool _hasAnimationShow;
        [SerializeField] private GameObject _content;
        private void RegiterAnimationShow()
        {
            if (!_hasAnimationShow) return;
            //var rect = _content.GetComponent<RectTransform>();
            //rect.offsetMax = Vector2.zero;
            //rect.offsetMin = Vector2.zero;
            _content.transform.localScale = Vector3.one * 0.67f;
            _content.transform.DOScale(1, 0.33f);
        }
        #endregion

        #region Implement interface
        public Canvas Canvas => _canvas;
        public GameObject ThisGameObject => gameObject;
        public virtual void Close()
        {
            ThisGameObject.SetActive(false);
        }
        public virtual void Initialized(object data = null)
        {
            RegiterButtonClose();
            Show();
            RegiterAnimationShow();
        }
        public virtual void Show()
        {
            ThisGameObject.SetActive(true);
        }
        public virtual void UpdateSortingOrder(int sortingOrder)
        {
           // _canvas.sortingOrder = sortingOrder;
        }
    }
    #endregion
}



