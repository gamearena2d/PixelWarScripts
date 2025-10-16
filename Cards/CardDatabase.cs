using System.Collections.Generic;
using UnityEngine;
using Game.Cards;  // Assicurati che la classe Card sia nello stesso namespace

[CreateAssetMenu(fileName = "CardDatabase", menuName = "Card/Create Card Database")]
public class CardDatabase : ScriptableObject
{
    [SerializeField] private List<Card> allCards;  // Lista popolata dinamicamente

    private Dictionary<string, Card> cardLookup;

    // 🔄 Carica tutte le carte dalla cartella Resources/Cards
    public void LoadAllCards()
    {
        allCards = new List<Card>(Resources.LoadAll<Card>("Cards")); // Cerca tutte le carte
        InitLookup();
        Debug.Log($"CardDatabase: caricate {allCards.Count} carte.");
    }

    // 🧠 Inizializza il dizionario per accesso rapido
    private void InitLookup()
    {
        cardLookup = new Dictionary<string, Card>();
        foreach (var card in allCards)
        {
            if (!cardLookup.ContainsKey(card.Name))
                cardLookup.Add(card.Name, card);
        }
    }

    // 🔍 Trova carta per nome
    public Card GetCardByName(string cardName)
    {
        if (cardLookup == null || cardLookup.Count == 0)
            InitLookup();

        cardLookup.TryGetValue(cardName, out var card);
        return card;
    }

    // 📜 Ritorna tutte le carte caricate
    public List<Card> GetAllCards()
    {
        return allCards;
    }
}
