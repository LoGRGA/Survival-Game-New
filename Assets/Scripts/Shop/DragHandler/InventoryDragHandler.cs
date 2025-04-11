using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public Transform originalParent;

    public Slot originalSlotScript; //Modify The Slot

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalSlotScript = originalParent.GetComponent<Slot>(); // Save the Slot script

        transform.SetParent(transform.root); // Move to top-level UI
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(originalParent); // Reset if no drop
        transform.localPosition = Vector3.zero;
        canvasGroup.blocksRaycasts = true;
    }

    public void SetOriginalParent(Transform parent)
    {
        originalParent = parent;
    }
}
