using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ClanData
{
    public string ClanName;
    public string ClanTag;
    public string Description;
    public string Language;
    public string Location;
    public int RequiredTrophies;
    public int RequiredLevel;
    public Sprite ClanLogo;

    // 📊 Punteggi e statistiche
    public int ClanPoints;
    public int BestClanPoints;
    public int WarStars;

    // ⚔️ Partecipazione guerra clan
    public bool WarParticipation;

    // 👥 Membri del clan
    public List<ClanMemberData> Members = new();

    // 🏆 Totali trofei clan
    public int TotalTrophies
    {
        get
        {
            int total = 0;
            foreach (var m in Members) total += m.Trophies;
            return total;
        }
    }
}

[Serializable]
public class ClanMemberData
{
    public string PlayerName;
    public int Trophies;
    public ClanRole Role;

    // 📦 Carte donate e ricevute (giornaliere e totali)
    public int CardsDonatedToday;
    public int CardsReceivedToday;
    public int TotalCardsDonated;
    public int TotalCardsReceived;

    // 🖼️ Nuovo: Avatar del membro
    public Sprite AvatarIcon;

    // ⚔️ Nuovo: Partecipazione alla guerra
    public bool ParticipatesInWar;
}

public enum ClanRole
{
    Leader,
    CoLeader,
    Elder,
    Member
}

public enum WarFrequency
{
    Never,
    Rarely,
    Sometimes,
    Often,
    Always
}
