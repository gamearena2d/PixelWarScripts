using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Clans;

public class ClanProfileUI : MonoBehaviour
{
    [Header("📌 Player Info")]
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private TMP_Text playerHashtagText;
    [SerializeField] private TMP_Text playerRoleText;

    [Header("🏰 Clan Info")]
    [SerializeField] private TMP_Text clanNameText;
    [SerializeField] private Image clanLogoImage;
    [SerializeField] private Button warPreferenceButton;
    [SerializeField] private TMP_Text warPreferenceText;

    [Header("📊 Points Info")]
    [SerializeField] private TMP_Text actualPointsText;
    [SerializeField] private TMP_Text bestPointsText;
    [SerializeField] private TMP_Text clanStarsText;

    [Header("📦 Donations Info")]
    [SerializeField] private TMP_Text cardDonationsText;
    [SerializeField] private TMP_Text cardReceivedText;

    [Header("🃏 Card Profile Pool")]
    [SerializeField] private CardProfilePoolSimple cardProfilePool;

    // 👉 Stato interno guerra
    private bool warParticipation = true;

    private void Start()
    {
        LoadClanProfile();
        warPreferenceButton.onClick.AddListener(ToggleWarPreference);
    }

    public void LoadClanProfile()
    {
        // 👤 Player info
        playerNameText.text = PlayerManager.Instance.playerData.playerName;
        playerHashtagText.text = "#ABC123";
        playerRoleText.text = "Elder";

        // 🏰 Clan info
        clanNameText.text = ClanManager.Instance.ClanName;
        clanLogoImage.sprite = ClanManager.Instance.ClanLogo;

        warParticipation = ClanManager.Instance.WarParticipation;
        UpdateWarButtonUI();

        // 📊 Punti
        actualPointsText.text = ClanManager.Instance.ClanPoints.ToString();
        bestPointsText.text = ClanManager.Instance.BestClanPoints.ToString();
        clanStarsText.text = ClanManager.Instance.WarStars.ToString();

        // 📦 Donazioni
        cardDonationsText.text = PlayerManager.Instance.playerData.TotalDonations.ToString();
        cardReceivedText.text = PlayerManager.Instance.playerData.TotalReceived.ToString();

        // 🃏 Aggiorna la lista carte profilo
        if (cardProfilePool != null)
            cardProfilePool.Refresh();
    }

    private void ToggleWarPreference()
    {
        warParticipation = !warParticipation;
        ClanManager.Instance.WarParticipation = warParticipation;
        UpdateWarButtonUI();
    }

    private void UpdateWarButtonUI()
    {
        warPreferenceText.text = warParticipation ? "YES" : "NO";
        var colors = warPreferenceButton.colors;
        colors.normalColor = warParticipation ? new Color(0.2f, 0.8f, 0.2f) : new Color(0.8f, 0.2f, 0.2f);
        warPreferenceButton.colors = colors;
    }
}
