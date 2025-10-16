using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MemberEntryUI : MonoBehaviour
{
    [Header("📛 Basic Info")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text gradeText;
    [SerializeField] private TMP_Text trophyText;

    [Header("📈 Donation Info")]
    [SerializeField] private TMP_Text donatedText;
    [SerializeField] private TMP_Text receivedText;

    [Header("📊 Other")]
    [SerializeField] private Image warIcon;
    [SerializeField] private Image avatarImage;
    [SerializeField] private Image levelIcon;

    public void Setup(ClanMemberData member)
    {
        nameText.text = member.PlayerName;
        gradeText.text = member.Role.ToString();
        trophyText.text = member.Trophies.ToString();

        donatedText.text = member.CardsDonatedToday.ToString();
        receivedText.text = member.CardsReceivedToday.ToString();

        if (avatarImage) avatarImage.sprite = member.AvatarIcon;
        if (warIcon) warIcon.color = member.ParticipatesInWar ? Color.green : Color.red;
    }
}
