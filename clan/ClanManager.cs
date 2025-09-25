using UnityEngine;
using System.Collections.Generic;

public class ClanManager : MonoBehaviour {
    public static ClanManager Instance;
    public List<ClanData> AllClans = new();
    public ClanData MyClan;

    private void Awake() {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public ClanData CreateClan(string name, string desc, string lang, string loc, int trophies, int level, WarFrequency freq) {
        ClanData newClan = new ClanData {
            ClanName = name,
            ClanTag = "#" + Random.Range(100000, 999999).ToString(),
            Description = desc,
            Language = lang,
            Location = loc,
            RequiredTrophies = trophies,
            RequiredLevel = level,
            WarFrequency = freq
        };
        newClan.Members.Add(new ClanMemberData { PlayerName = "You", Trophies = 0, Role = ClanRole.Leader });
        AllClans.Add(newClan);
        MyClan = newClan;
        return newClan;
    }

    public List<ClanData> SearchClans(string query, string lang = null, string loc = null, int minTrophies = 0) {
        List<ClanData> results = new();
        foreach (var clan in AllClans) {
            if (!string.IsNullOrEmpty(query) && !clan.ClanName.ToLower().Contains(query.ToLower())) continue;
            if (!string.IsNullOrEmpty(lang) && clan.Language != lang) continue;
            if (!string.IsNullOrEmpty(loc) && clan.Location != loc) continue;
            if (clan.TotalTrophies < minTrophies) continue;
            results.Add(clan);
        }
        return results;
    }
}
