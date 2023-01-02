using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace XXX.UI.Popup
{
    public class BasePopupManager : MonoBehaviour
    {
        private StackPopup _popups;
        public static BasePopupManager Instance;
        protected StackPopup Popups => _popups ?? (_popups = new StackPopup());

        [SerializeField] private Canvas _canvasToShow;

        [SerializeField] private PopupWin _popupWinPrefab;
        private IPopup _popupWinHandle;
        [SerializeField] private PopupLose _popupLosePrefab;
        private IPopup _popupLoseHandle;
        private void Start()
        {
            //if(Instance != null)
            //{
            //    //Destroy(gameObject);
            //}
            DontDestroyOnLoad(gameObject);
            Instance = this; 
        }

        public void ShowPopupWin()
        {
            ShowPopup(_popupWinPrefab, ref _popupWinHandle, null);
        }

        public void ShowPopupLose()
        {
            ShowPopup(_popupLosePrefab, ref _popupLoseHandle, null);
        }

        protected void ShowPopup<T>(T popup, ref IPopup handle, object data) where T : BasePopup
        {
            
            if (handle != null)
            {
                if (handle.ThisGameObject.activeSelf) return;
                Display(ref handle);
                return;
            }

            handle = Instantiate(popup, transform, false);
            Display(ref handle);

            void Display(ref IPopup handle)
            {
                var popup = (T)handle;
                popup.Initialized(data);
                Popups.Show(handle);
            }
        }

        private void Update()
        {
            _canvasToShow.gameObject.SetActive(true);
            
        }
    }

    public class StackPopup
    {
        private Stack<IPopup> _stacks = new Stack<IPopup>();
        public void Hide()
        {
            var popuphide = _stacks.Pop();
            popuphide.Close();
        }
        public void Show(IPopup uniPopupHandler)
        {
            //var lastOrder = 0;
            //if (_stacks.Count > 0)
            //{
            //    var top = _stacks.Peek();
            //    lastOrder = top.Canvas.sortingOrder;
            //}

            //uniPopupHandler.UpdateSortingOrder(lastOrder + 10);
            //_stacks.Push(uniPopupHandler);
            uniPopupHandler.Show(); // show
        }
        public void CloseAll()
        {
            foreach (var i in _stacks)
            {
                i.Close();
            }
            _stacks.Clear();
        }
    }

}