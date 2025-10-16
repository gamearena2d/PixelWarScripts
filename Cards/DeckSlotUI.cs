using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Cards;

public class DeckSlotUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image cardIcon;
    public TMP_Text cardNameText;

    private Card assignedCard;

    // Imposta la carta nello slot
    public void SetCard(Card card)
    {
        assignedCard = card;

        if (cardIcon != null) cardIcon.sprite = card.CardIcon;
        if (cardNameText != null) cardNameText.text = card.Name;

        gameObject.SetActive(true);
    }

    // Ritorna true se c'è una carta
    public bool HasCard()
    {
        return assignedCard != null;
    }

    // Pulisce lo slot
    public void ClearSlot()
    {
        assignedCard = null;

        if (cardIcon != null) cardIcon.sprite = null;
        if (cardNameText != null) cardNameText.text = "";

        gameObject.SetActive(false);
    }

    // Opzionale: restituisce la carta assegnata
    public Card GetCard()
    {
        return assignedCard;
    }

    // Se vuoi anche rimuoverla da click
    public void OnClickRemove()
    {
        ClearSlot();
    }
}
