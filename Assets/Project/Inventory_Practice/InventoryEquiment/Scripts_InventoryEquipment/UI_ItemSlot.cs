using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot : MonoBehaviour, IDropHandler {

    private Action onDropAction;

    public void SetOnDropAction(Action onDropAction) {
        this.onDropAction = onDropAction;
    }

    public void OnDrop(PointerEventData eventData) {
        UI_ItemDrag.Instance.Hide();
        onDropAction();
    }
}
