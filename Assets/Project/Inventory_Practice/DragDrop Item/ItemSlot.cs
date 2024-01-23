using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    // todo game object = o dat item vao
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("OnDrop");
        //? khi da buong chuot -> onDrop truoc EndDrag sau
        if(eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = this.gameObject.GetComponent<RectTransform>().anchoredPosition;
            //? gameobject dang duoc Ondrag se duoc gan vi tri cua slotItem vao
        }
    }
}
