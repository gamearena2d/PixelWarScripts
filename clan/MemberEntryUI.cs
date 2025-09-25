using UnityEngine;
using TMPro;

public class MemberEntryUI : MonoBehaviour
{
    public TMP_Text playerNameText;
    public TMP_Text roleText;
    public TMP_Text trophiesText;
    public TMP_Text donationsText;

    public void Setup(ClanMemberData member)
    {
        playerNameText.text = member.PlayerName;
        roleText.text = member.Role.ToString();
        trophiesText.text = member.Trophies.ToString();
        donationsText.text = $"Donate: {member.CardsDonatedToday} | Ricevute: {member.CardsReceivedToday}";
    }
}
