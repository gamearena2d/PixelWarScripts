using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Game.Cards;

public class CardProfileEntry : MonoBehaviour
{
    [Header("UI Riferimenti")]
    [SerializeField] private Image cardIcon;
    [SerializeField] private Image borderBase;
    [SerializeField] private Image borderLegendary;
    [SerializeField] private Image borderDivine;
    [SerializeField] private TextMeshProUGUI cardLevelText;

    private Card cardData;

    public void Setup(Card card, bool unlocked, int level)
    {
        cardData = card;

        if (unlocked)
        {
            cardIcon.sprite = card.CardIcon;
            cardIcon.color = Color.white;
            cardLevelText.text = $"Lv. {level}";
        }
        else
        {
            cardIcon.sprite = card.CardIcon;
            cardIcon.color = Color.gray;
            cardLevelText.text = "?";
        }

        // Mostra il bordo corretto in base alla rarità
        borderBase.gameObject.SetActive(card.Rarity == CardRarity.COMMON || card.Rarity == CardRarity.UNCOMMON);
        borderLegendary.gameObject.SetActive(card.Rarity == CardRarity.LEGENDARY);
        borderDivine.gameObject.SetActive(card.Rarity == CardRarity.DIVINE);
    }
}
