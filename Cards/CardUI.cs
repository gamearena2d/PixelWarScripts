using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Game.Cards;

public class CardUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Image cardIcon;
    public TMP_Text nameText;
    public TMP_Text levelText;
	
	[Header("Mana Drop")]
    public Image manaDropIcon;       // L'immagine della goccia
    public TMP_Text manaCostText;    // Il testo dentro la goccia

    [Header("Border GameObjects")]
    public GameObject borderDefault;
    public GameObject borderLegendary;
    public GameObject borderDivine;

    [Header("Level Icon (separato dal bordo carta)")]
    public Image levelIcon; // trascina qui l'immagine del "level icon" (es. cornice livello)

    [Header("Level Panels")]
    public GameObject levelPanelFull;
    public GameObject levelPanelHole;
    public Slider levelProgressBar;
    public TMP_Text levelProgressText;

    [Header("Upgrade Arrow")]
    public Image upgradeArrow;
    public Color arrowBlue = Color.blue;
    public Color arrowGreen = Color.green;

    [Header("Click overlay")]
    [SerializeField] private Button cardselect; // trascina qui il child "cardselect"

    [Header("Action Panel")]
    public GameObject actionPanelPrefab; // prefab del pannello
    private GameObject currentActionPanel; // istanza corrente
    [SerializeField] private float extraPanelOffsetY = 20f; // offset verticale

    private Card cardData;

    // Comodo per il manager
    public RectTransform Rect => (RectTransform)transform;
    public Card GetCardData() => cardData;
    public float GetPanelOffset() => extraPanelOffsetY;

    private void Awake()
    {
        // Collego il click overlay
        if (cardselect != null)
        {
            cardselect.onClick.RemoveAllListeners();
            cardselect.onClick.AddListener(OnCardClicked);
        }
    }

    public void Setup(Card card)
    {
    cardData = card;
    if (cardData == null) return;

    if (card.CardIcon != null)
        cardIcon.sprite = card.CardIcon;

    nameText.text = card.Name;
    levelText.text = "Lv. " + card.BaseLevel;

    SetRarity(card.Rarity);
    UpdateLevelUI();

    // --- GOCCIA MANA ---
    if (manaDropIcon != null)
        manaDropIcon.gameObject.SetActive(card.ManaCost > 0);

    if (manaCostText != null)
        manaCostText.text = card.ManaCost.ToString();
    }

    // -----------------------------
    // Compatibilità con altri script
    // -----------------------------
    public void SelectCard()
    {
        // qui puoi aggiungere highlight visivo se vuoi
    }

    public void DeselectCard()
    {
        // qui puoi rimuovere highlight se vuoi
    }

    // -----------------------------
    // Mostra pannello globale
    // -----------------------------
    public void OnCardClicked()
    {
        CardActionPanelManager.Instance?.ShowPanel(this);
    }

    // -----------------------------
    // UI Rarity e Level
    // -----------------------------
    private void SetRarity(CardRarity rarity)
    {
        // Manteniamo la logica bordi come prima (non la tocchiamo)
        if (borderDefault) borderDefault.SetActive(false);
        if (borderLegendary) borderLegendary.SetActive(false);
        if (borderDivine) borderDivine.SetActive(false);

        switch (rarity)
        {
            case CardRarity.COMMON:   ActivateDefaultBorder(Color.white); break;
            case CardRarity.UNCOMMON: ActivateDefaultBorder(new Color(0.0627f, 0.7921f, 0.0f)); break;
            case CardRarity.RARE:     ActivateDefaultBorder(new Color(0.0f, 0.2470f, 1.0f)); break;
            case CardRarity.EPIC:     ActivateDefaultBorder(new Color(1.0f, 0.5294f, 0.0f)); break;
            case CardRarity.MYTHIC:   ActivateDefaultBorder(new Color(0.5921f, 0.0f, 1.0f)); break;
            case CardRarity.LEGENDARY: if (borderLegendary) borderLegendary.SetActive(true); break;
            case CardRarity.DIVINE:     if (borderDivine) borderDivine.SetActive(true); break;
            default: ActivateDefaultBorder(Color.white); break;
        }

        // ---- levelIcon: colore indipendente, applicato sempre se assegnato ----
        if (levelIcon != null)
        {
            levelIcon.color = GetLevelIconColor(rarity);
        }
    }

    private void ActivateDefaultBorder(Color color)
    {
        if (!borderDefault) return;
        borderDefault.SetActive(true);
        var img = borderDefault.GetComponent<Image>();
        if (img) img.color = color;
    }

    private void UpdateLevelUI()
    {
        if (cardData == null) return;

        bool canUpgrade = cardData.CurrentCount >= cardData.RequiredCount;

        if (levelProgressBar && cardData.RequiredCount > 0)
            levelProgressBar.value = Mathf.Clamp01((float)cardData.CurrentCount / cardData.RequiredCount);

        if (levelProgressText)
            levelProgressText.text = $"{cardData.CurrentCount}/{cardData.RequiredCount}";

        if (upgradeArrow)
            upgradeArrow.color = canUpgrade ? arrowGreen : arrowBlue;

        if (levelPanelFull && levelPanelHole)
        {
            levelPanelFull.SetActive(!canUpgrade);
            levelPanelHole.SetActive(canUpgrade);
        }
    }

    // Colori per il levelIcon (tutti i casi, compresi Legendary/Divine)
    private Color GetLevelIconColor(CardRarity rarity)
    {
        switch (rarity)
        {
            case CardRarity.COMMON:
                return Color.white;                                 // bianco
            case CardRarity.UNCOMMON:
                return new Color(0.0627f, 0.7921f, 0.0f);           // verde (#10CA00)
            case CardRarity.RARE:
                return new Color(0.0f, 0.2470f, 1.0f);              // blu (#003FFF)
            case CardRarity.EPIC:
                return new Color(1.0f, 0.5294f, 0.0f);              // arancione (#FF8700)
            case CardRarity.MYTHIC:
                return new Color(0.5921f, 0.0f, 1.0f);              // viola (#9700FF)
            case CardRarity.LEGENDARY:
                return new Color(0.0f, 0.8f, 1.0f);                 // celeste (personalizzabile)
            case CardRarity.DIVINE:
                return new Color(1.0f, 0.843f, 0.0f);               // oro (personalizzabile)
            default:
                return Color.white;
        }
    }
}
