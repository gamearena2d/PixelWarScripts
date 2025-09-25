using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LeagueData
{
    public string Name;
    public int TrophyThreshold;

    public LeagueData(string name, int trophyThreshold)
    {
        Name = name;
        TrophyThreshold = trophyThreshold;
    }
}

public class LeagueManager : MonoBehaviour
{
    public static LeagueManager Instance;
    public List<LeagueData> leagues = new List<LeagueData>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        // League setup (example inspired by Clash Royale)
        leagues.Add(new LeagueData("No League", 0));
        leagues.Add(new LeagueData("Bronze", 300));
        leagues.Add(new LeagueData("Silver", 600));
        leagues.Add(new LeagueData("Gold", 1000));
        leagues.Add(new LeagueData("Crystal", 1400));
        leagues.Add(new LeagueData("Emerald", 2000));
        leagues.Add(new LeagueData("Diamond", 2600));
        leagues.Add(new LeagueData("Legendary", 3200));
        leagues.Add(new LeagueData("Master", 4000));
        leagues.Add(new LeagueData("Ultimate", 5000));
        leagues.Add(new LeagueData("Champion", 7000));
    }

    public LeagueData GetCurrentLeague(int trophies)
    {
        LeagueData current = leagues[0];
        foreach (var l in leagues)
        {
            if (trophies >= l.TrophyThreshold)
                current = l;
            else
                break;
        }
        return current;
    }
}
