using UnityEngine;
using UnityEngine.UI;

public class DeckManagerUI : MonoBehaviour
{
    [Header("Deck References")]
    public GameObject[] deckPanels; // DECK1, DECK2, DECK3, DECK4, DECK5

    [Header("Deck Buttons")]
    public Button[] deckButtons; // ButtonDeck1, ButtonDeck2, ..., ButtonDeck5

    private int currentDeckIndex = 0;

    private void Start()
    {
        // Inizializza i listener dei bottoni
        for (int i = 0; i < deckButtons.Length; i++)
        {
            int index = i; // Evita closure bug
            deckButtons[i].onClick.AddListener(() => OnDeckButtonClicked(index));
        }

        ShowDeck(currentDeckIndex);
    }

    private void OnDeckButtonClicked(int index)
    {
        if (index == currentDeckIndex)
            return;

        ShowDeck(index);
    }

    private void ShowDeck(int index)
    {
        // Disattiva tutti i deck
        for (int i = 0; i < deckPanels.Length; i++)
        {
            deckPanels[i].SetActive(i == index);
        }

        currentDeckIndex = index;

        Debug.Log($"[DeckManagerUI] Deck attivo: {index + 1}");

        // (Opzionale) aggiorna UI selezione, colore pulsanti, ecc.
        UpdateButtonHighlights();
    }

    private void UpdateButtonHighlights()
    {
        for (int i = 0; i < deckButtons.Length; i++)
        {
            ColorBlock colors = deckButtons[i].colors;
            colors.normalColor = (i == currentDeckIndex) ? Color.green : Color.white;
            deckButtons[i].colors = colors;
        }
    }

    public int GetCurrentDeckIndex()
    {
        return currentDeckIndex;
    }
}
