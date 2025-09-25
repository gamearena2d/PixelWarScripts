using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Cards; // perché Card è in Game.Cards

public class CardProfileEntry : MonoBehaviour
{
    [Header("UI References")]
    public Image cardIcon;
    public Image borderBase;
    public Image borderLegendary;
    public Image borderDivine;
    public TMP_Text levelText;

    [Header("Colors")]
    public Color unlockedColor = Color.white;
    public Color lockedColor = new Color(0.3f, 0.3f, 0.3f, 1f);

    /// <summary>
    /// Setup di una carta nel profilo
    /// </summary>
    public void Setup(Card card, bool unlocked, int level)
    {
        if (card == null) return;

        // Bordi: solo quello corretto attivo
        borderBase.gameObject.SetActive(card.Rarity == CardRarity.COMMON);
        borderLegendary.gameObject.SetActive(card.Rarity == CardRarity.LEGENDARY);
        borderDivine.gameObject.SetActive(card.Rarity == CardRarity.DIVINE);

        // Icona
        cardIcon.sprite = card.CardIcon;
        cardIcon.color = unlocked ? unlockedColor : lockedColor;

        // Livello
        if (unlocked && level > 0)
        {
            levelText.text = FormatLevelNumber(level);
            levelText.color = unlockedColor;
        }
        else
        {
            levelText.text = "???";
            levelText.color = lockedColor;
        }
    }

    // Aggiunge spazi tra le cifre (es. 12 → "1 2")
    private string FormatLevelNumber(int level)
    {
        return string.Join(" ", level.ToString().ToCharArray());
    }
}
