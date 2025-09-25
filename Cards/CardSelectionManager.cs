using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using Game.Cards;

public class CardSelectionManager : MonoBehaviour
{
    public static CardSelectionManager Instance;

    [Header("Deck Display Panels")]
    public GameObject[] deckPanels; // DECK1–DECK5

    [Header("Deck Selector Buttons")]
    public Button[] deckButtons; // ButtonDeck1–5

    private int currentDeckIndex = 0;
    private List<Card>[] decks = new List<Card>[5]; // 5 mazzi separati
    private DeckSlotUI[] currentDeckSlots;
    private CardUI selectedCard;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Inizializza i mazzi
        for (int i = 0; i < decks.Length; i++)
            decks[i] = new List<Card>();

        // Assegna listener ai pulsanti deck
        for (int i = 0; i < deckButtons.Length; i++)
        {
            int index = i;
            deckButtons[i].onClick.AddListener(() => SwitchDeck(index));
        }

        // Mostra il primo deck
        SwitchDeck(0);
    }

    public void SwitchDeck(int deckIndex)
    {
        for (int i = 0; i < deckPanels.Length; i++)
            deckPanels[i].SetActive(i == deckIndex);

        currentDeckIndex = deckIndex;
        currentDeckSlots = deckPanels[deckIndex].GetComponentsInChildren<DeckSlotUI>();
        RefreshCurrentDeckUI();
        Debug.Log($"[DeckManager] Switched to deck {deckIndex + 1}");
    }

    private void RefreshCurrentDeckUI()
    {
        var cards = decks[currentDeckIndex];

        for (int i = 0; i < currentDeckSlots.Length; i++)
        {
            if (i < cards.Count)
                currentDeckSlots[i].SetCard(cards[i]);
            else
                currentDeckSlots[i].ClearSlot();
        }
    }

    public void TryAddCardToDeck(Card card)
    {
        var current = decks[currentDeckIndex];

        if (current.Exists(c => c.Name == card.Name))
        {
            Debug.Log($"[DeckManager] '{card.Name}' è già presente nel mazzo.");
            return;
        }

        if (current.Count >= 8)
        {
            Debug.LogWarning("[DeckManager] Deck pieno. Sostituzione della prima carta.");
            current[0] = card;
        }
        else
        {
            current.Add(card);
        }

        RefreshCurrentDeckUI();
    }

    public void RemoveCardFromDeck(Card card)
    {
        var current = decks[currentDeckIndex];
        if (current.Remove(card))
            RefreshCurrentDeckUI();
    }

    public bool IsCardInCurrentDeck(Card card) => decks[currentDeckIndex].Contains(card);
    public List<Card> GetCurrentDeckCards() => decks[currentDeckIndex];

    public void SelectCard(CardUI newCard)
    {
        if (selectedCard != null && selectedCard != newCard)
            selectedCard.DeselectCard(); // chiudi pannello precedente

        if (selectedCard == newCard)
        {
            selectedCard.DeselectCard();
            selectedCard = null;
            return;
        }

        selectedCard = newCard;
        selectedCard.SelectCard(); // apri pannello nuovo
    }

    public void DeselectCurrent()
    {
        if (selectedCard != null)
        {
            selectedCard.DeselectCard();
            selectedCard = null;
        }
    }

    public CardUI GetSelectedCard() => selectedCard;
}
