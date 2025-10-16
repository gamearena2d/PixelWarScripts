using UnityEngine;
using System.Collections.Generic;

public class CardUIManager : MonoBehaviour
{
    public static CardUIManager Instance;

    private CardUI currentSelected;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void SelectCard(CardUI card)
    {
        if (currentSelected != null && currentSelected != card)
            currentSelected.DeselectCard();

        currentSelected = card;
        card.SelectCard();
    }

    public void DeselectCurrent()
    {
        if (currentSelected != null)
        {
            currentSelected.DeselectCard();
            currentSelected = null;
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Se clicco fuori dalla carta
            if (currentSelected != null)
            {
                if (!RectTransformUtility.RectangleContainsScreenPoint(
                    currentSelected.GetComponent<RectTransform>(), 
                    Input.mousePosition, 
                    null))
                {
                    DeselectCurrent();
                }
            }
        }
    }
}
