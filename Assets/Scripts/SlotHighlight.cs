using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SlotHighlight : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image panelImage;
    private Color originalColor;
    public Color hoverColor; // Color when hovered
    public Color clickColor; // Color when clicked down

    public GameManager gmScript;

    void Start()
    {
        gmScript = GameObject.Find("GameManager").GetComponent<GameManager>();
        panelImage = GetComponent<Image>();
        if (panelImage != null)
        {
            originalColor = panelImage.color; // Store the original color
        }
    }

    private void Update()
    {
        if (!IsPointerOverUIElement())
        {
            panelImage.color = originalColor; // Reset color when pointer is not over any UI element
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (panelImage != null)
        {
            panelImage.color = hoverColor; // Change color on hover
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (panelImage != null)
        {
            panelImage.color = originalColor; // Revert color when mouse leaves
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        // Check which mouse button was clicked
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log("Left click on slot!");

            switch (eventData.pointerPress.tag)
            {
                case "Slot1":
                    if (gmScript.slot1Full)
                    {
                        gmScript.Item1Text();
                    }
                    break;
                case "Slot2":
                    if (gmScript.slot2Full)
                    {
                        gmScript.Item2Text();
                    }
                    break;
                case "Slot3":
                    if (gmScript.slot3Full)
                    {
                        gmScript.Item3Text();
                    }
                    break;
                case "Slot4":
                    if (gmScript.slot4Full)
                    {
                        gmScript.Item4Text();
                    }
                    break;
                // Add more cases as needed
                default:
                    Debug.LogWarning("No corresponding slot found for this tag or slot is not full.");
                    gmScript.NullSlotClick();
                    break;
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log("Right click on slot!");
            // Handle right-click events here
            // For example, you might want to show a context menu or perform a different action
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (panelImage != null)
        {
            panelImage.color = clickColor; // Change color on pointer down
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (panelImage != null)
        {
            panelImage.color = originalColor; // Revert color on pointer up
        }
    }

    private bool IsPointerOverUIElement()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}
