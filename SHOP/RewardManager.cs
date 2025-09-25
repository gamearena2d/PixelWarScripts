using UnityEngine;
using System.Collections.Generic;
using Game.Cards;

public class RewardManager : MonoBehaviour
{
    public static RewardManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    /// <summary>
    /// Restituisce una carta casuale dal pool (puoi filtrare per rarità)
    /// </summary>
    public Card GetRandomCard(CardRarity? rarityFilter = null)
    {
        Card[] allCards = Resources.LoadAll<Card>("Cards");

        List<Card> filtered = new List<Card>();
        foreach (var c in allCards)
        {
            if (rarityFilter == null || c.Rarity == rarityFilter)
                filtered.Add(c);
        }

        if (filtered.Count == 0) return null;

        return filtered[Random.Range(0, filtered.Count)];
    }

    /// <summary>
    /// Aggiunge al giocatore X copie di una carta specifica
    /// </summary>
    public void GiveCard(Card card, int copies)
    {
        if (card == null) return;
        CardProgressManager.Instance.AddCardCopies(card, copies);
        Debug.Log($"[Reward] Dato {copies}x {card.Name}");
    }

    /// <summary>
    /// Aggiunge al giocatore carte casuali
    /// </summary>
    public void GiveRandomCards(int count, CardRarity? rarityFilter = null)
    {
        for (int i = 0; i < count; i++)
        {
            Card c = GetRandomCard(rarityFilter);
            if (c != null)
                GiveCard(c, Random.Range(1, 5)); // es: 1-5 copie a caso
        }
    }
}
