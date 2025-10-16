using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Cards;

public class DailyCardSlot : MonoBehaviour
{
    [Header("UI References")]
    public CardUIShop cardUI;      // Prefab della card (con script CardUIShop attaccato)
    public TMP_Text priceText;     // Testo prezzo
    public Button buyButton;       // Bottone acquisto

    private Card currentCard;      // Carta assegnata a questo slot
    private int price;             // Prezzo corrente
    private bool useGems;          // Se l'acquisto è in gemme o oro

    /// <summary>
    /// Imposta la carta nello slot
    /// </summary>
    /// <param name="card">Carta da visualizzare</param>
    /// <param name="cost">Prezzo</param>
    /// <param name="gems">true se si usa gemme, false se oro</param>
    public void Setup(Card card, int cost, bool gems)
    {
        currentCard = card;
        price = cost;
        useGems = gems;

        // Popola la CardUI con i dati della carta
        if (cardUI != null && card != null)
        {
            cardUI.gameObject.SetActive(true);
            cardUI.Setup(card);
        }

        // Aggiorna prezzo
        if (priceText != null)
            priceText.text = cost.ToString();

        // Configura bottone acquisto
        if (buyButton != null)
        {
            buyButton.onClick.RemoveAllListeners();
            buyButton.onClick.AddListener(OnBuyClicked);
            buyButton.interactable = true;
        }
    }

    /// <summary>
    /// Evento click su acquisto
    /// </summary>
    private void OnBuyClicked()
    {
        bool paid = useGems
            ? CurrencyManager.Instance.SpendGems(price)
            : CurrencyManager.Instance.SpendGold(price);

        if (!paid)
        {
            Debug.Log("Valuta insufficiente!");
            return;
        }

        if (currentCard != null)
            RewardManager.Instance.GiveCard(currentCard, 1);

        // Disabilita il bottone e aggiorna il testo
        if (buyButton != null) buyButton.interactable = false;
        if (priceText != null) priceText.text = "Acquistata";
    }
}
