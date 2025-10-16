using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Cards;

public class CardUIShop : MonoBehaviour
{
    [Header("UI Elements")]
    public Image cardIcon;
    public TMP_Text cardName;
    public TMP_Text levelText;

    [Header("Borders")]
    public GameObject borderBase;
    public GameObject borderLegendary;
    public GameObject borderDivine;

    public void Setup(Card card)
    {
        if (card == null) return;

        // Icona, nome e livello
        if (cardIcon != null) cardIcon.sprite = card.CardIcon;
        if (cardName != null) cardName.text = card.Name;
        if (levelText != null) levelText.text = "Lvl " + card.BaseLevel;

        // Aggiorna bordi in base alla rarità
        SetRarity(card.Rarity);
    }

    private void SetRarity(CardRarity rarity)
    {
        // Disattiva tutti i bordi
        if (borderBase) borderBase.SetActive(false);
        if (borderLegendary) borderLegendary.SetActive(false);
        if (borderDivine) borderDivine.SetActive(false);

        switch (rarity)
        {
            case CardRarity.COMMON:
                ActivateBaseBorder(Color.white);
                break;
            case CardRarity.UNCOMMON:
                ActivateBaseBorder(new Color(0.0627f, 0.7921f, 0.0f)); // verde
                break;
            case CardRarity.RARE:
                ActivateBaseBorder(new Color(0.0f, 0.2470f, 1.0f)); // blu
                break;
            case CardRarity.EPIC:
                ActivateBaseBorder(new Color(1.0f, 0.5294f, 0.0f)); // arancione
                break;
            case CardRarity.MYTHIC:
                ActivateBaseBorder(new Color(0.5921f, 0.0f, 1.0f)); // viola
                break;
            case CardRarity.LEGENDARY:
                if (borderLegendary) borderLegendary.SetActive(true);
                break;
            case CardRarity.DIVINE:
                if (borderDivine) borderDivine.SetActive(true);
                break;
            default:
                ActivateBaseBorder(Color.white);
                break;
        }
    }

    private void ActivateBaseBorder(Color color)
    {
        if (!borderBase) return;
        borderBase.SetActive(true);
        var img = borderBase.GetComponent<Image>();
        if (img) img.color = color;
    }
}
