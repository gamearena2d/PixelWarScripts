using UnityEngine;
using System.Collections.Generic;
using Game.Cards;

public class CardPackManager : MonoBehaviour
{
    public static CardPackManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    public void OpenPack(CardPackAsset pack)
    {
        List<Card> allCards = new List<Card>(Resources.LoadAll<Card>("Cards"));
        List<Card> selectedCards = new List<Card>();

        int commons = pack.TotalCards - pack.RareCount - pack.EpicCount - (pack.ChanceLegendary ? 1 : 0);

        // Aggiungi comuni
        for (int i = 0; i < commons; i++)
            selectedCards.Add(GetRandomCard(allCards, CardRarity.COMMON));

        // Rari
        for (int i = 0; i < pack.RareCount; i++)
            selectedCards.Add(GetRandomCard(allCards, CardRarity.RARE));

        // Epici
        for (int i = 0; i < pack.EpicCount; i++)
            selectedCards.Add(GetRandomCard(allCards, CardRarity.EPIC));

        // Leggendari (chance)
        if (pack.ChanceLegendary && Random.value < 0.2f)
            selectedCards.Add(GetRandomCard(allCards, CardRarity.LEGENDARY));

        // Dai le carte al giocatore
        foreach (var card in selectedCards)
        {
            if (card != null)
                RewardManager.Instance.GiveCard(card, 1);
        }

        Debug.Log($"Pacchetto {pack.PackName} aperto: {selectedCards.Count} carte assegnate.");
    }

    private Card GetRandomCard(List<Card> pool, CardRarity rarity)
    {
        var filtered = pool.FindAll(c => c.Rarity == rarity);
        if (filtered.Count == 0) return null;
        return filtered[Random.Range(0, filtered.Count)];
    }
}
