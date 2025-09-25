using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Cards;

public class CardUIShop : MonoBehaviour
{
    public Image cardIcon;
    public TMP_Text cardName;
    public TMP_Text levelText;

    public void Setup(Card card)
    {
        if (card == null) return;

        if (cardIcon != null) cardIcon.sprite = card.CardIcon;        // Icona
        if (cardName != null) cardName.text = card.Name;
        if (levelText != null) levelText.text = "Lvl " + card.BaseLevel.ToString(); // Livello base
    }
}
