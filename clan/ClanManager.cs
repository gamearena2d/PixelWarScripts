using System.Collections.Generic;
using UnityEngine;

public class ClanManager : MonoBehaviour
{
    public static ClanManager Instance;
    public List<ClanData> AllClans = new();
    public ClanData MyClan;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public ClanData CreateClan(string name, string desc, string lang, string loc, int trophies, int level, WarFrequency freq)
    {
        ClanData newClan = new ClanData
        {
            ClanName = name,
            ClanTag = "#" + Random.Range(100000, 999999).ToString(),
            Description = desc,
            Language = lang,
            Location = loc,
            RequiredTrophies = trophies,
            RequiredLevel = level,
            WarParticipation = false
        };

        newClan.Members.Add(new ClanMemberData
        {
            PlayerName = "You",
            Trophies = 0,
            Role = ClanRole.Leader
        });

        AllClans.Add(newClan);
        MyClan = newClan;
        return newClan;
    }

    public List<ClanData> SearchClans(string query, string lang = null, string loc = null, int minTrophies = 0)
    {
        List<ClanData> results = new();
        foreach (var clan in AllClans)
        {
            if (!string.IsNullOrEmpty(query) && !clan.ClanName.ToLower().Contains(query.ToLower())) continue;
            if (!string.IsNullOrEmpty(lang) && clan.Language != lang) continue;
            if (!string.IsNullOrEmpty(loc) && clan.Location != loc) continue;
            if (clan.TotalTrophies < minTrophies) continue;
            results.Add(clan);
        }
        return results;
    }

    // ✅ Shortcut utili per la UI
    public string ClanName => MyClan?.ClanName ?? "";
    public Sprite ClanLogo => MyClan?.ClanLogo;
    public bool WarParticipation
    {
        get => MyClan?.WarParticipation ?? false;
        set { if (MyClan != null) MyClan.WarParticipation = value; }
    }

    public int ClanPoints => MyClan?.ClanPoints ?? 0;
    public int BestClanPoints => MyClan?.BestClanPoints ?? 0;
    public int WarStars => MyClan?.WarStars ?? 0;
}
