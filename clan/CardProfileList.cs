using UnityEngine;
using System.Collections.Generic;
using Game.Cards;

public class CardProfileList : MonoBehaviour
{
    [SerializeField] private Transform contentParent; // ScrollView Content
    private CardDatabase database;
    private CardProfilePoolManager poolManager;

    private void Awake()
    {
        // ✅ Nuovo metodo consigliato da Unity
        var dbHolder = Object.FindFirstObjectByType<CardDatabaseHolder>();
        if (dbHolder != null)
            database = dbHolder.cardDatabase;

        poolManager = Object.FindFirstObjectByType<CardProfilePoolManager>();

        if (database == null)
            Debug.LogError("[CardProfileList] ❌ Nessun CardDatabase trovato.");
        if (poolManager == null)
            Debug.LogError("[CardProfileList] ❌ Nessun CardProfilePoolManager trovato.");
    }

    private void OnEnable()
    {
        PopulateCardList();
    }

    private void PopulateCardList()
    {
        if (database == null || poolManager == null) return;

        List<Card> allCards = database.GetAllCards();

        foreach (Card card in allCards)
        {
            var entry = poolManager.GetFromPool(contentParent);

            bool unlocked = card.CurrentCount > 0;
            int level = unlocked ? card.BaseLevel : 0;

            entry.Setup(card, unlocked, level);
        }
    }
}
