using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using Game.Cards;

public class CardDetailPanel : MonoBehaviour
{
    public static CardDetailPanel Instance;

    [Header("Header")]
    public Image cardImage;

    [Header("Mana Cost UI")]
    public Image manaDropIcon;
    public TMP_Text manaCostText;

    [Header("Borders")]
    public GameObject borderDefault;
    public GameObject borderLegendary;
    public GameObject borderDivine;

    [Header("Base Images")]
    public Image baseImageDefault;
    public GameObject baseImageLegendary;
    public GameObject baseImageDivine;

    public TMP_Text cardName;
    public TMP_Text rarityLabel;
    public TMP_Text rarityValue;
    public TMP_Text targetLabel;
    public TMP_Text targetValue;
    public TMP_Text typeLabel;
    public TMP_Text typeValue;

    [Header("Level Panels")]
    public Slider levelProgressBar;
    public TMP_Text levelProgressText;

    [Header("Video")]
    public VideoPlayer cardVideo;
    public RawImage videoDisplay;

    [Header("Stats Page")]
    public GameObject statRowPrefab;
    public Transform leftColumn;
    public Transform rightColumn;

    [Header("Icons per Statistiche")]
    public Sprite hpIcon;
    public Sprite manaIcon;
    public Sprite attackIcon;
    public Sprite speedIcon;
    public Sprite atkSpeedIcon;
    public Sprite rangeIcon;
    public Sprite goldIcon;
    public Sprite gemIcon;
    public Sprite trophyIcon;
    public Sprite copiesIcon;

    [Header("Icons per Abilità")]
    public Sprite shieldIcon;
    public Sprite jumpIcon;
    public Sprite dashIcon;
    public Sprite lifestealIcon;
    public Sprite regenIcon;
    public Sprite summonIcon;
    public Sprite splashIcon;

    [Header("Ability Page")]
    public TMP_Text abilityText;

    [Header("UI")]
    public Button closeButton;

    [Header("Paging System")]
    public ScrollRect scrollRect;
    public Image[] pageIndicators;
    public Color activeIndicatorColor = Color.white;
    public Color inactiveIndicatorColor = Color.gray;

    private int currentPage = 0;

    private void UpdateLevelUI()
    {
        // Aggiorna eventuali barre o testi di livello, se necessario.
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        gameObject.SetActive(false);

        if (closeButton != null)
            closeButton.onClick.AddListener(() => gameObject.SetActive(false));
    }

    private void Update()
    {
        if (!gameObject.activeSelf) return;

        float normalized = scrollRect.horizontalNormalizedPosition;
        int newPage = Mathf.RoundToInt(normalized * (pageIndicators.Length - 1));

        if (newPage != currentPage)
        {
            currentPage = newPage;
            UpdatePageIndicators();
        }
    }

    private void UpdatePageIndicators()
    {
        if (pageIndicators == null) return;

        for (int i = 0; i < pageIndicators.Length; i++)
            pageIndicators[i].color = (i == currentPage) ? activeIndicatorColor : inactiveIndicatorColor;
    }

    public void ShowCard(Card card)
    {
        if (card == null) return;

        // HEADER
        cardImage.sprite = card.CardIcon;
        cardName.text = card.Name;

        if (rarityLabel) rarityLabel.text = "Rarity";
        if (rarityValue) rarityValue.text = card.Rarity.ToString();
        if (targetLabel) targetLabel.text = "Target";
        if (targetValue) targetValue.text = card.TargetType.ToString();
        if (typeLabel) typeLabel.text = "Type";
        if (typeValue) typeValue.text = card.Type.ToString();

        // --- AGGIORNA I BORDI E LO SFONDO ---
        SetRarity(card.Rarity);

        // Mostra la goccia del costo mana
        if (manaDropIcon != null)
            manaDropIcon.gameObject.SetActive(card.ManaCost > 0);

        if (manaCostText != null)
            manaCostText.text = card.ManaCost.ToString();

        // LEVEL PANEL
        if (levelProgressBar && card.RequiredCount > 0)
            levelProgressBar.value = (float)card.CurrentCount / card.RequiredCount;
        if (levelProgressText)
            levelProgressText.text = $"{card.CurrentCount} / {card.RequiredCount}";

        // VIDEO
        if (cardVideo != null && card.CardPreviewVideo != null)
        {
            cardVideo.Stop();
            cardVideo.clip = card.CardPreviewVideo;
            cardVideo.Play();
        }
        else if (videoDisplay != null)
        {
            videoDisplay.texture = null;
        }

        // PULISCE LE STATISTICHE
        ClearStats();

        // AGGIUNGE LE STATISTICHE
        AddStat(hpIcon, "HP", card.Health, 0);
        AddStat(manaIcon, "Mana", card.ManaCost, 1);
        AddStat(attackIcon, "Attack", card.Damage, 2);
        AddStat(speedIcon, "Move Speed", card.MoveSpeed, 3);
        AddStat(atkSpeedIcon, "Attack Speed", card.AttackSpeed, 4);
        AddStat(rangeIcon, "Range", card.Range, 5);
        AddStat(goldIcon, "Gold Cost", card.GoldCost, 6);
        AddStat(gemIcon, "Gem Cost", card.GemCost, 7);
        AddStat(trophyIcon, "Required Trophies", card.RequiredTrophies, 8);
        AddStat(copiesIcon, "Owned Copies", card.CurrentCount, 9);

        // ABILITÀ SPECIALI
        int abilityIndex = 10;
        if (card.HasShield) AddStat(shieldIcon, "Scudo", 1, abilityIndex++);
        if (card.HasJump) AddStat(jumpIcon, "Salto", 1, abilityIndex++);
        if (card.HasDash) AddStat(dashIcon, "Scatto", 1, abilityIndex++);
        if (card.SpawnsMinions) AddStat(summonIcon, "Evoca Minions", 1, abilityIndex++);
        if (card.HasLifesteal) AddStat(lifestealIcon, "Lifesteal", 1, abilityIndex++);
        if (card.HasRegeneration) AddStat(regenIcon, "Rigenerazione", 1, abilityIndex++);
        if (card.HasSplashDamage) AddStat(splashIcon, "Splash Damage", 1, abilityIndex++);

        // DESCRIZIONE ABILITÀ
        abilityText.text = !string.IsNullOrEmpty(card.AbilityDescription)
            ? card.AbilityDescription
            : "Nessuna abilità speciale.";

        scrollRect.horizontalNormalizedPosition = 0f;
        currentPage = 0;
        UpdatePageIndicators();

        gameObject.SetActive(true);
    }

    private void AddStat(Sprite icon, string label, float value, int index)
    {
        if (statRowPrefab == null) return;

        Transform parent = index < 5 ? leftColumn : rightColumn;

        GameObject rowObj = Instantiate(statRowPrefab, parent);
        rowObj.SetActive(true);

        StatRow row = rowObj.GetComponent<StatRow>();
        if (row != null)
        {
            if (row.icon) row.icon.sprite = icon;
            if (row.labelText) row.labelText.text = label;
            if (row.valueText) row.valueText.text = value > 1 ? value.ToString() : "";
        }
    }

    private void ClearStats()
    {
        foreach (Transform child in leftColumn) Destroy(child.gameObject);
        foreach (Transform child in rightColumn) Destroy(child.gameObject);
    }

    // ---------- RARITY COLOR ----------
    private void SetRarity(CardRarity rarity)
    {
        // Disattiva tutti i bordi e le immagini
        borderDefault?.SetActive(false);
        borderLegendary?.SetActive(false);
        borderDivine?.SetActive(false);
        baseImageDefault?.gameObject.SetActive(false);
        baseImageLegendary?.SetActive(false);
        baseImageDivine?.SetActive(false);

        switch (rarity)
        {
            case CardRarity.LEGENDARY:
                borderLegendary?.SetActive(true);
                baseImageLegendary?.SetActive(true);
                break;

            case CardRarity.DIVINE:
                borderDivine?.SetActive(true);
                baseImageDivine?.SetActive(true);
                break;

            default:
                borderDefault?.SetActive(true);
                var img = borderDefault.GetComponent<Image>();
                if (img) img.color = GetBorderColor(rarity);

                baseImageDefault?.gameObject.SetActive(true);
                if (baseImageDefault) baseImageDefault.color = GetBaseColor(rarity);
                break;
        }
    }

    private Color GetBorderColor(CardRarity rarity)
    {
        return rarity switch
        {
            CardRarity.COMMON => Color.white,
            CardRarity.UNCOMMON => new Color(0.282f, 0.619f, 0.294f),
            CardRarity.RARE => new Color(0.235f, 0.388f, 0.855f),
            CardRarity.EPIC => new Color(0.823f, 0.569f, 0.286f),
            CardRarity.MYTHIC => new Color(0.490f, 0.247f, 0.863f),
            _ => Color.white
        };
    }

    private Color GetBaseColor(CardRarity rarity)
    {
        return rarity switch
        {
            CardRarity.COMMON => Color.white,
            CardRarity.UNCOMMON => new Color(0.18f, 0.44f, 0.18f),
            CardRarity.RARE => new Color(0.882f, 0.557f, 0.255f),
            CardRarity.EPIC => new Color(0.66f, 0.40f, 0.15f),
            CardRarity.MYTHIC => new Color(0.36f, 0.18f, 0.66f),
            _ => Color.white
        };
    }
}
