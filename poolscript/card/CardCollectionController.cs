using UnityEngine;
using System.Collections.Generic;
using Game.Cards;

public class CardCollectionController : MonoBehaviour
{
    public Transform contentParent;     // Content sotto Scroll View
    public CardDatabase cardDatabase;   // Il database con le 6 carte

    private List<GameObject> activeCards = new List<GameObject>();

    private void Start()
    {
        ShowAllCards();
    }

    public void ShowAllCards()
    {
        // Prima restituisci tutte le carte attive al pool
        CardPoolManager.Instance.ReturnAllToPool();

        // Pulisci la lista delle carte attive, così la tieni aggiornata
        activeCards.Clear();

        // Prendi tutte le carte dal database
        var allCards = cardDatabase.GetAllCards();

        foreach (var card in allCards)
        {
            GameObject cardGO = CardPoolManager.Instance.GetCardFromPool();

            if (cardGO == null)
            {
                Debug.LogWarning("[CardCollectionController] Pool esaurita!");
                break;
            }

            // Setta il parent mantenendo la scala e posizione locale
            cardGO.transform.SetParent(contentParent, false);

            // Attiva il GameObject della carta
            cardGO.SetActive(true);

            // Prendi lo script CardUI e assegna la carta da visualizzare
            CardUI ui = cardGO.GetComponent<CardUI>();
            if (ui != null)
            {
                ui.Setup(card);
                ui.DeselectCard(); // Se vuoi nascondere pannelli selezione ecc.
            }
            else
            {
                Debug.LogWarning("[CardCollectionController] CardUI non trovato sul prefab.");
            }

            // Aggiungi il GameObject attivo alla lista
            activeCards.Add(cardGO);
        }
    }
}
