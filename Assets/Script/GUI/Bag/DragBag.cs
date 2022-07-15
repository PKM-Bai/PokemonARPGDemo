using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragBag : MonoBehaviour, IDragHandler
{
    public GameObject bag; 
    RectTransform currentRect;  // UI Canvas的RectTransform

    private void Awake() {
        currentRect = bag.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 背包的中心坐标跟随鼠标的轻微移动
        currentRect.anchoredPosition += eventData.delta;
    }

}
