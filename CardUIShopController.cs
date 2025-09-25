using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Cards;

public class CardUIShopController : MonoBehaviour
{
    [Header("UI Riferimenti")]
    public Image cardIcon;
    public TMP_Text cardNameText;
    public TMP_Text priceText;
    public Button buyButton;

    private Card cardData;
    private int price;

    public void SetCard(Card card, int cost)
    {
        cardData = card;
        price = cost;

        if (cardIcon && card.CardIcon != null)
            cardIcon.sprite = card.CardIcon;

        if (cardNameText)
            cardNameText.text = card.Name;

        if (priceText)
            priceText.text = $"{price} gold";

        if (buyButton)
            buyButton.onClick.AddListener(BuyCard);
    }

    private void BuyCard()
    {
        Debug.Log($"Hai comprato {cardData.Name} per {price} gold!");
        // TODO: togli oro al player e aggiungi copia della carta
        buyButton.interactable = false;
    }
}
