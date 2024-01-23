using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler,IDragHandler, IBeginDragHandler, IEndDragHandler
{
    //todo GameObject = doi tuong can drag and Drop

    [SerializeField] private Canvas canvas;

    private RectTransform rectTransform; // move this.Gameobject thay doi PosX PosY
    private CanvasGroup canvasGroup; //? thay doi alpha interactable phat hien Ondrop iten in Slot

    private void Awake() {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

#region DragDrop
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("OnPointerDown"); //? click mouse vao GO chua this.mothod
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("OnBeginDrag");
        canvasGroup.alpha = 0.6f; // lam mo image
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("OnEndDrag");
        canvasGroup.alpha = 1; // lam mo image
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("OnDrag");
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor; //? move
    }


    #endregion DragDrop
}
