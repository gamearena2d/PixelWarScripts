using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CardActionPanelManager : MonoBehaviour
{
    public static CardActionPanelManager Instance;

    [Header("Refs")]
    public RectTransform panelRect;      // Prefab del pannello nel Canvas
    public Button buttonINFO;
    public Button buttonUSE;

    [Header("Manual Offsets")]
    [SerializeField] private float manualOffsetX = 0f;
    [SerializeField] private float manualOffsetY = -20f;

    private CardUI currentCard;
    private Canvas parentCanvas;
    private Camera cam;

    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (panelRect != null)
            panelRect.gameObject.SetActive(false);

        parentCanvas = panelRect.GetComponentInParent<Canvas>();
        cam = parentCanvas != null ? parentCanvas.worldCamera : Camera.main;
    }

    private void Update()
    {
        if (currentCard == null || !panelRect.gameObject.activeSelf)
            return;

        // Mantieni il pannello sopra la carta selezionata
        FollowCard();

        // Se clicchi col tasto sinistro
        if (Input.GetMouseButtonDown(0))
        {
            // Controlla se il click è sul pannello
            bool clickedOnPanel = RectTransformUtility.RectangleContainsScreenPoint(panelRect, Input.mousePosition, cam);

            // Se il click NON è sul pannello
            if (!clickedOnPanel)
            {
                // Controlla se hai cliccato su una carta
                var clickedObj = EventSystem.current.currentSelectedGameObject;
                bool clickedOnCard = clickedObj != null && clickedObj.GetComponent<CardUI>() != null;

                // Se NON hai cliccato su una carta → chiudi il pannello
                if (!clickedOnCard)
                    Hide();
            }
        }
    }

    private void FollowCard()
    {
        if (parentCanvas == null || currentCard == null)
            return;

        Vector2 localPos;
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, currentCard.Rect.position);

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            screenPoint,
            cam,
            out localPos
        );

        // Applica offset per posizionare il pannello sopra la carta
        localPos.x += manualOffsetX;
        localPos.y += currentCard.Rect.rect.height * 0.5f + currentCard.GetPanelOffset() + manualOffsetY;

        panelRect.anchoredPosition = localPos;
    }

    public void ShowPanel(CardUI card)
    {
        if (card == null || panelRect == null)
            return;

        // Se riclicchi sulla stessa carta → chiudi
        if (currentCard == card)
        {
            Hide();
            return;
        }

        currentCard = card;

        // Porta il pannello davanti
        panelRect.SetAsLastSibling();
        panelRect.gameObject.SetActive(true);

        // Posiziona subito il pannello sulla carta
        FollowCard();

        // Configura i pulsanti
        buttonINFO.onClick.RemoveAllListeners();
        buttonUSE.onClick.RemoveAllListeners();

        buttonINFO.onClick.AddListener(() =>
        {
            if (card.GetCardData() != null)
                CardDetailPanel.Instance?.ShowCard(card.GetCardData());
            Hide();
        });

        buttonUSE.onClick.AddListener(() =>
        {
            if (card.GetCardData() != null)
                CardSelectionManager.Instance?.TryAddCardToDeck(card.GetCardData());
            Hide();
        });
    }

    public void Hide()
    {
        if (panelRect != null)
            panelRect.gameObject.SetActive(false);

        currentCard = null;
    }

    private void OnDisable()
    {
        // Se il cardpanel viene disattivato → chiudi automaticamente il pannello
        Hide();
    }
}
