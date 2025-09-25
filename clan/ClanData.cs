using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClanData {
    public string ClanName;
    public string ClanTag;
    public string Description;
    public string Language;
    public string Location;
    public int RequiredTrophies;
    public int RequiredLevel;
    public WarFrequency WarFrequency;
    public List<ClanMemberData> Members = new();
    public int TotalTrophies => CalculateTotalTrophies();

    private int CalculateTotalTrophies() {
        int sum = 0;
        foreach (var m in Members) sum += m.Trophies;
        return sum;
    }
}

[Serializable]
public class ClanMemberData {
    public string PlayerName;
    public int Trophies;
    public ClanRole Role;
    public int CardsDonatedToday;
    public int CardsReceivedToday;
}

public enum ClanRole { Leader, CoLeader, Elder, Member }
public enum WarFrequency { Never, Rarely, Sometimes, Often, Always }
