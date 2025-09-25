using UnityEngine;
using System.Collections.Generic;
using Game.Cards;

public class CardProgressManager : MonoBehaviour
{
    public static CardProgressManager Instance;

    // Dizionario: NomeCarta -> Copie possedute (persistente)
    private Dictionary<string, int> ownedCards = new Dictionary<string, int>();

    // Moltiplicatori/parametri globali (modificabili a piacere)
    [Header("Balance")]
    [Tooltip("Moltiplicatore per crescita copie richieste al livello successivo (es. 1.4 = +40%)")]
    public float copiesGrowthFactor = 1.4f;

    [Tooltip("Sconto applicato al costo in oro dell'upgrade (es. 0.9 = -10%)")]
    [Range(0.5f, 1f)]
    public float upgradeGoldDiscount = 0.9f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        LoadProgress();
    }

    #region --- Gestione Carte ---

    // Aggiunge copie di una carta (es. dal shop, da un baule, ricompensa)
    public void AddCardCopies(Card card, int amount)
    {
        if (card == null || amount <= 0) return;

        if (!ownedCards.ContainsKey(card.Name))
            ownedCards[card.Name] = 0;

        ownedCards[card.Name] += amount;
        card.CurrentCount = ownedCards[card.Name];

        Debug.Log($"[CardProgress] +{amount} copie per {card.Name}. Totale: {card.CurrentCount}");

        SaveProgress();
    }

    // Restituisce il costo effettivo in oro per l'upgrade (applica eventuale sconto globale)
    public int GetUpgradeGoldCost(Card card)
    {
        if (card == null) return 0;
        return Mathf.CeilToInt(card.GoldCost * upgradeGoldDiscount);
    }

    // Controlla se una carta è uppabile
    public bool CanUpgrade(Card card)
    {
        if (card == null) return false;

        bool hasEnoughCopies = card.CurrentCount >= card.RequiredCount;
        int upgradeCost = GetUpgradeGoldCost(card);
        bool hasEnoughGold = CurrencyManager.Instance.Gold >= upgradeCost;

        return hasEnoughCopies && hasEnoughGold;
    }

    // Esegue l'upgrade della carta (stat, abilità, minion)
    public bool UpgradeCard(Card card)
    {
        if (card == null) return false;
        if (!CanUpgrade(card)) return false;

        int upgradeCost = GetUpgradeGoldCost(card);

        // Scala oro
        if (!CurrencyManager.Instance.SpendGold(upgradeCost))
            return false;

        // Scala copie
        card.CurrentCount -= card.RequiredCount;
        ownedCards[card.Name] = card.CurrentCount;

        // Aumenta livello
        card.BaseLevel++;

        // Aggiorna RequiredCount per il prossimo livello (es. crescita 40%)
        int newRequired = Mathf.CeilToInt(card.RequiredCount * copiesGrowthFactor);
        // Assicura che cresca almeno di 1
        card.RequiredCount = Mathf.Max(1, newRequired);

        // Aggiorna stats base
        if (card.LevelProgression != null)
        {
            card.Damage += card.LevelProgression.DamagePerLevel;
            card.Health += card.LevelProgression.HealthPerLevel;
            card.AttackSpeed += card.LevelProgression.AttackSpeedPerLevel;
            card.MoveSpeed += card.LevelProgression.MoveSpeedPerLevel;
        }

        // Aggiorna abilità incrementabili (esempi; aggiusta valori come preferisci)
        UpgradeAbilities(card);

        // Aggiorna minion se presenti (usa i campi del Minion presente nella Card)
        UpgradeMinions(card);

        Debug.Log($"[CardProgress] {card.Name} uppata a livello {card.BaseLevel} (costo oro: {upgradeCost}). Copie rimanenti: {card.CurrentCount}");

        SaveProgress();
        return true;
    }

    private void UpgradeAbilities(Card card)
    {
        if (card.LevelProgression == null) return;

        // Se LevelProgression definisce incrementi per abilità (interi), usali
        // Esempi pratici:
        if (card.HasRegeneration && card.LevelProgression.RegenerationPerLevel > 0)
        {
            // incrementa rigenerazione di X (qui  RegenerationPerLevel come "unità")
            card.RegenerationPerSecond += card.LevelProgression.RegenerationPerLevel * 0.5f;
        }

        if (card.HasLifesteal && card.LevelProgression.LifestealPerLevel > 0)
        {
            // aggiunge percentuale lifesteal (es. +1% per unità)
            card.LifestealPercent += card.LevelProgression.LifestealPerLevel * 1f;
        }

        if (card.HasShield && card.LevelProgression.ShieldPerLevel > 0)
        {
            card.ShieldHp += card.LevelProgression.ShieldPerLevel * 5; // esempio: +5 HP shield per "unit"
        }

        if (card.HasDash && card.LevelProgression.DashPerLevel > 0)
        {
            card.DashDistance += card.LevelProgression.DashPerLevel * 0.1f;
        }

        if (card.HasJump && card.LevelProgression.JumpPerLevel > 0)
        {
            // esempio: riduci cooldown/ricarica o aumenta efficacia
        }

        // aggiungi altri incrementi se vuoi
    }

    private void UpgradeMinions(Card card)
    {
        // Nota: in base alla tua versione di Card, il blocco minion si chiama "Minion".
        // Qui assumiamo che 'card.SpawnsMinions' e 'card.Minion' esistano (come nella versione fornita).
        if (!card.SpawnsMinions) return;
        if (card.Minion == null) return;

        // Se il Minion ha campi "PerLevel", aumentali
        // (Nella struttura MinionData abbiamo HealthPerLevel, DamagePerLevel, ecc.)
        card.Minion.Health += card.Minion.HealthPerLevel;
        card.Minion.Damage += card.Minion.DamagePerLevel;
        card.Minion.AttackSpeed += card.Minion.AttackSpeedPerLevel;
        card.Minion.MoveSpeed += card.Minion.MoveSpeedPerLevel;

        // Esempio: potresti aumentare anche il numero massimo evocabile o ridurre cooldown:
        // card.Minion.MaxAlive = Mathf.Min(card.Minion.MaxAlive + 1, someCap);
        // card.Minion.SpawnCooldown = Mathf.Max(0.1f, card.Minion.SpawnCooldown * 0.95f);
    }

    #endregion

    #region --- Salvataggio ---
    private void SaveProgress()
    {
        foreach (var kvp in ownedCards)
        {
            PlayerPrefs.SetInt("CARD_" + kvp.Key, kvp.Value);
        }
        PlayerPrefs.Save();
    }

    private void LoadProgress()
    {
        ownedCards.Clear();
        var allCards = Resources.LoadAll<Card>("Cards");
        foreach (var card in allCards)
        {
            int count = PlayerPrefs.GetInt("CARD_" + card.Name, card.CurrentCount);
            ownedCards[card.Name] = count;
            card.CurrentCount = count;
        }
    }
    #endregion
}
