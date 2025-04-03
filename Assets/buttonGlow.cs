using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Vector3 originalScale;
    private Image buttonImage;
    private Color originalColor;
    public float scaleMultiplier = 1.2f;  // Adjust the scaling factor
    public Color glowColor = new Color(1f, 1f, 1f, 1.5f);  // Slightly brighter white

    void Start()
    {
        originalScale = transform.localScale;
        buttonImage = GetComponent<Image>();  // Get the button's image component
        originalColor = buttonImage.color;  // Store the original button color
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = originalScale * scaleMultiplier;
        buttonImage.color = glowColor;  // Change to glow color
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = originalScale;
        buttonImage.color = originalColor;  // Reset color
    }
}


