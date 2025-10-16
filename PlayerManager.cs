using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerCardProgress
{
    public string cardName;
    public bool isUnlocked;
    public int level;
    public int copiesOwned;

    public PlayerCardProgress(string name, bool unlocked = false, int lvl = 1, int copies = 0)
    {
        cardName = name;
        isUnlocked = unlocked;
        level = lvl;
        copiesOwned = copies;
    }
}

[Serializable]
public class PlayerData
{
    // 🧑‍💼 Info giocatore
    public string playerName = "Player";
    public int gold = 0;
    public int gems = 0;
    public int trophies = 0;
    public string league = "Bronze"; // lega iniziale
    public Sprite playerIcon;

    // 📦 Collezione carte
    public List<PlayerCardProgress> cardCollection = new List<PlayerCardProgress>();

    // 📊 Nuovi contatori clan
    public int TotalDonations = 0;
    public int TotalReceived = 0;

    // 🔍 Utility carte
    public bool IsCardUnlocked(string cardName)
    {
        var card = cardCollection.Find(c => c.cardName == cardName);
        return card != null && card.isUnlocked;
    }

    public int GetCardLevel(string cardName)
    {
        var card = cardCollection.Find(c => c.cardName == cardName);
        return card != null ? card.level : 0;
    }

    public int GetCardCopies(string cardName)
    {
        var card = cardCollection.Find(c => c.cardName == cardName);
        return card != null ? card.copiesOwned : 0;
    }

    public void UnlockCard(string cardName)
    {
        var card = cardCollection.Find(c => c.cardName == cardName);
        if (card == null)
        {
            cardCollection.Add(new PlayerCardProgress(cardName, true, 1, 0));
        }
        else
        {
            card.isUnlocked = true;
        }
    }

    public void AddCardCopies(string cardName, int amount)
    {
        var card = cardCollection.Find(c => c.cardName == cardName);
        if (card == null)
        {
            cardCollection.Add(new PlayerCardProgress(cardName, true, 1, amount));
        }
        else
        {
            card.copiesOwned += amount;
        }
    }

    public bool TryUpgradeCard(string cardName, int requiredCopies)
    {
        var card = cardCollection.Find(c => c.cardName == cardName);
        if (card != null && card.copiesOwned >= requiredCopies)
        {
            card.copiesOwned -= requiredCopies;
            card.level++;
            return true;
        }
        return false;
    }

    // 🏆 Aggiorna la lega in base ai trofei
    public void UpdateLeague()
    {
        if (trophies < 300)
            league = "Bronze";
        else if (trophies < 800)
            league = "Silver";
        else if (trophies < 1600)
            league = "Gold";
        else if (trophies < 3000)
            league = "Crystal";
        else if (trophies < 5000)
            league = "Master";
        else
            league = "Legendary";
    }

    // 📦 Gestione donazioni e carte ricevute
    public void AddDonation(int amount)
    {
        TotalDonations += amount;
    }

    public void AddReceivedCards(int amount)
    {
        TotalReceived += amount;
    }
}

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public PlayerData playerData = new PlayerData();

    private string saveKey = "PLAYER_DATA_JSON";

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        LoadPlayerData();
    }

    // 💾 Salvataggio
    public void SavePlayerData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        PlayerPrefs.SetString(saveKey, json);
        PlayerPrefs.Save();
        Debug.Log("✅ PlayerData salvato!");
    }

    // 📥 Caricamento
    public void LoadPlayerData()
    {
        if (PlayerPrefs.HasKey(saveKey))
        {
            string json = PlayerPrefs.GetString(saveKey);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("📥 PlayerData caricato con successo.");
        }
        else
        {
            playerData = new PlayerData();
            Debug.Log("🆕 Nuovo profilo creato.");
            SavePlayerData();
        }

        // Aggiorna subito la lega dopo il caricamento
        playerData.UpdateLeague();
    }

    // 📈 Modifiche giocatore
    public void AddGold(int amount)
    {
        playerData.gold += amount;
        SavePlayerData();
    }

    public void AddGems(int amount)
    {
        playerData.gems += amount;
        SavePlayerData();
    }

    public void AddTrophies(int amount)
    {
        playerData.trophies += amount;
        playerData.UpdateLeague();
        SavePlayerData();
    }

    // 📉 Perdita trofei
    public void RemoveTrophies(int amount)
    {
        playerData.trophies = Mathf.Max(0, playerData.trophies - amount);
        playerData.UpdateLeague();
        SavePlayerData();
    }

    // 🧪 Test (clic destro su script in Inspector)
    [ContextMenu("Test Add Knight Card")]
    private void TestAddKnight()
    {
        playerData.UnlockCard("Knight");
        playerData.AddCardCopies("Knight", 10);
        SavePlayerData();
    }
}
