using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MyClanUI : MonoBehaviour
{
    [Header("Clan Info")]
    public TMP_Text clanNameText;
    public TMP_Text clanTagText;
    public TMP_Text descriptionText;
    public TMP_Text trophiesText;
    public TMP_Text membersText;

    [Header("Members List")]
    public Transform memberListContent;
    public GameObject memberEntryPrefab;

    [Header("Buttons")]
    public Button leaveButton;

    private void OnEnable()
    {
        RefreshMyClanUI();
    }

    public void RefreshMyClanUI()
    {
        var clan = ClanManager.Instance.MyClan;
        if (clan == null) return;

        clanNameText.text = clan.ClanName;
        clanTagText.text = clan.ClanTag;
        descriptionText.text = clan.Description;
        trophiesText.text = clan.TotalTrophies.ToString();
        membersText.text = $"{clan.Members.Count}/50";

        foreach (Transform child in memberListContent)
            Destroy(child.gameObject);

        foreach (var member in clan.Members)
        {
            var go = Instantiate(memberEntryPrefab, memberListContent);
            go.GetComponent<MemberEntryUI>().Setup(member);
        }

        leaveButton.onClick.RemoveAllListeners();
        leaveButton.onClick.AddListener(() => LeaveClan());
    }

    private void LeaveClan()
    {
        Debug.Log("Left clan");
        ClanManager.Instance.MyClan = null;
        gameObject.SetActive(false);
    }
}
