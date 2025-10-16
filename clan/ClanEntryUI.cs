using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClanEntryUI : MonoBehaviour
{
    [Header("UI References")]
    public TMP_Text clanNameText;
    public TMP_Text clanTagText;
    public TMP_Text membersText;
    public TMP_Text trophiesText;
    public TMP_Text descriptionText;
    public Button joinButton;

    private ClanData currentClan;

    public void Setup(ClanData clan)
    {
        currentClan = clan;
        clanNameText.text = clan.ClanName;
        clanTagText.text = clan.ClanTag;
        membersText.text = $"{clan.Members.Count}/50";
        trophiesText.text = clan.TotalTrophies.ToString();
        descriptionText.text = clan.Description;

        joinButton.onClick.RemoveAllListeners();
        joinButton.onClick.AddListener(OnJoinClicked);
    }

    private void OnJoinClicked()
    {
        Debug.Log($"Joining clan {currentClan.ClanName}");
        ClanManager.Instance.MyClan = currentClan;
        // Qui puoi aprire direttamente il pannello "Il mio clan"
    }
}
