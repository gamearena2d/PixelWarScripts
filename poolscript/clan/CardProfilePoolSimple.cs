using System.Collections.Generic;
using UnityEngine;
using Game.Cards;

[AddComponentMenu("UI/Card Profile Pool Simple")]
public class CardProfilePoolSimple : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform contentParent;               // Viewport/Content
    [SerializeField] private CardProfileEntry cardProfilePrefab;    // Prefab con CardProfileEntry.cs
    [SerializeField] private CardDatabase cardDatabase;             // Dragga il tuo CardDatabase.asset

    private readonly List<CardProfileEntry> activeEntries = new List<CardProfileEntry>();
    private PlayerManager playerManager;

    private void Reset()
    {
        if (contentParent == null) contentParent = transform; // se attacchi lo script al Content
    }

    private void Awake()
    {
        // Carica DB se non assegnato in Inspector
        if (cardDatabase == null)
        {
            cardDatabase = Resources.Load<CardDatabase>("Cards/CardDatabase");
            if (cardDatabase == null)
                Debug.LogError("❌ CardDatabase non trovato. Assegna l'asset in Inspector o posizionalo in Resources/Cards/CardDatabase.asset");
        }

        // Trova PlayerManager senza usare API deprecate
#if UNITY_2023_1_OR_NEWER
        playerManager = Object.FindFirstObjectByType<PlayerManager>();
#else
        playerManager = Object.FindObjectOfType<PlayerManager>();
#endif
        if (playerManager == null)
            Debug.LogWarning("⚠️ PlayerManager non trovato in scena. Le carte verranno mostrate come bloccate.");
    }

    private void OnEnable()
    {
        Populate();
    }

    public void Populate()
    {
        if (contentParent == null) { Debug.LogError("❌ contentParent non assegnato."); return; }
        if (cardProfilePrefab == null) { Debug.LogError("❌ cardProfilePrefab non assegnato."); return; }
        if (cardDatabase == null) { Debug.LogError("❌ cardDatabase non assegnato."); return; }

        Clear();

        var allCards = cardDatabase.GetAllCards();
        if (allCards == null || allCards.Count == 0)
        {
            Debug.LogWarning("⚠️ Nessuna carta trovata nel database.");
            return;
        }

        foreach (var card in allCards)
        {
            var entry = Instantiate(cardProfilePrefab, contentParent);
            activeEntries.Add(entry);

            bool unlocked = false;
            int level = 0;

            if (playerManager != null && playerManager.playerData != null)
            {
                unlocked = playerManager.playerData.IsCardUnlocked(card.Name);
                level = playerManager.playerData.GetCardLevel(card.Name);
                if (!unlocked) level = 0;
            }

            entry.Setup(card, unlocked, level);
            entry.gameObject.SetActive(true);
        }

        Debug.Log($"✅ CardProfilePoolSimple: generate {activeEntries.Count} carte.");
    }

    public void Clear()
    {
        for (int i = 0; i < activeEntries.Count; i++)
        {
            if (activeEntries[i] != null)
                Destroy(activeEntries[i].gameObject);
        }
        activeEntries.Clear();
    }

    public void Refresh() => Populate();
}
