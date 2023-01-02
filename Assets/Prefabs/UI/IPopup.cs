using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPopup
{
    GameObject ThisGameObject { get; }
    Canvas Canvas { get; }
    void Show();
    void Close();
    void UpdateSortingOrder(int sortingOrder);
    void Initialized(object data = null);
}
