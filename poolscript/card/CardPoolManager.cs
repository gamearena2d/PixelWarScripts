using System.Collections.Generic;
using UnityEngine;

public class CardPoolManager : MonoBehaviour
{
    public static CardPoolManager Instance;

    [Header("Pool Settings")]
    [SerializeField] private GameObject cardUIPrefab;
    [SerializeField] private int initialPoolSize = 20;
    [SerializeField] private int maxPoolSize = 80;
    [SerializeField] private Transform poolParent; // Es. CardUIPool sotto la canvas

    private Queue<GameObject> cardPool = new Queue<GameObject>();
    private HashSet<GameObject> activeCards = new HashSet<GameObject>();

    private int totalCreated = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < initialPoolSize; i++)
            CreateNewCard();
    }

    private GameObject CreateNewCard()
    {
        if (totalCreated >= maxPoolSize)
        {
            Debug.LogWarning("[CardPool] Pool overflow: limite massimo raggiunto!");
            FileLogger.Log("[CardPool] Pool overflow");
            return null;
        }

        GameObject card = Instantiate(cardUIPrefab, poolParent);
        card.SetActive(false);
        cardPool.Enqueue(card);
        totalCreated++;
        FileLogger.Log("[CardPool] Created card #" + totalCreated);
        return card;
    }

    public GameObject GetCardFromPool()
    {
        GameObject card;

        if (cardPool.Count > 0)
        {
            card = cardPool.Dequeue();
            FileLogger.Log("[CardPool] Dequeued card. Pool size: " + cardPool.Count);
        }
        else
        {
            card = CreateNewCard();
            if (card == null)
            {
                FileLogger.Log("[CardPool] Pool exhausted!");
                return null;
            }
        }

        card.SetActive(true);
        activeCards.Add(card);
        FileLogger.Log("[CardPool] Card activated. Active count: " + activeCards.Count);
        return card;
    }

    public void ReturnCardToPool(GameObject card)
    {
        if (card == null) return;

        card.SetActive(false);
        card.transform.SetParent(poolParent);
        activeCards.Remove(card);
        cardPool.Enqueue(card);
    }

    /// <summary>
    /// Restituisce tutte le carte alla pool, da poolParent o da Content.
    /// </summary>
    public void ReturnAllToPool()
    {
        // Prima restituisce quelli da activeCards
        foreach (var card in activeCards)
        {
            if (card != null)
            {
                card.SetActive(false);
                card.transform.SetParent(poolParent);
                cardPool.Enqueue(card);
            }
        }

        activeCards.Clear();

        // Temporaneamente rimuovo la scansione di contentTransform per evitare doppioni
        /*
        Transform contentTransform = poolParent.parent.Find("Scroll View/Viewport/Content");
        if (contentTransform != null)
        {
            foreach (Transform child in contentTransform)
            {
                if (child.gameObject.activeSelf)
                {
                    ReturnCardToPool(child.gameObject);
                }
            }
        }
        else
        {
            Debug.LogWarning("[CardPool] Contenitore Content non trovato. Controlla la gerarchia.");
        }
        */
    }

    public int GetActiveCardCount() => activeCards.Count;
    public int GetPoolSize() => cardPool.Count;
}
