using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlotDropHandler : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;

        if (dropped != null)
        {
            InventoryDragHandler dragHandler = dropped.GetComponent<InventoryDragHandler>();

            Slot oldSlot = dragHandler.originalSlotScript;
            Slot newSlot = GetComponent<Slot>();

            if (oldSlot == null || newSlot == null)
                return;

            if (oldSlot == newSlot)
                return; // dragged to same slot

            // Transfer the item
            dropped.transform.SetParent(transform);
            dropped.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            dragHandler.SetOriginalParent(transform);
            dragHandler.originalSlotScript = newSlot;

            // Update amounts
            newSlot.amount += oldSlot.amount;
            oldSlot.amount = 0;

            // Remove item from old slot visually (if needed)
            if (oldSlot.amount == 0)
            {
                // Keep UI intact, just clear texts and visuals
                oldSlot.itemName.text = "";
                oldSlot.amountText.text = "";

                // Optionally, disable the image or icon inside the item
                // Example: if the icon is in a child called "ItemImage"
                Transform icon = oldSlot.transform.Find("ItemImage");
                if (icon != null)
                {
                    icon.gameObject.SetActive(false);
                }
            }

            // Update InventoryController's isFull array
            InventoryController inventory = FindObjectOfType<InventoryController>();
            inventory.isFull[oldSlot.i] = false;
            inventory.isFull[newSlot.i] = true;
        }
    }
}
