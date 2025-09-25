using UnityEngine;

public class CardProfilePoolManager : MonoBehaviour
{
    public static CardProfilePoolManager Instance;

    [SerializeField] private GameObject cardProfileEntryPrefab;
    [SerializeField] private Transform poolParent;
    [SerializeField] private int initialPoolSize = 20;

    private readonly System.Collections.Generic.Queue<CardProfileEntry> pool =
        new System.Collections.Generic.Queue<CardProfileEntry>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        // Pre-popoliamo il pool
        for (int i = 0; i < initialPoolSize; i++)
        {
            CreateNewEntry();
        }
    }

    private CardProfileEntry CreateNewEntry()
    {
        var obj = Instantiate(cardProfileEntryPrefab, poolParent);
        obj.SetActive(false);
        var entry = obj.GetComponent<CardProfileEntry>();
        pool.Enqueue(entry);
        return entry;
    }

    public CardProfileEntry GetFromPool(Transform parent)
    {
        if (pool.Count == 0)
            CreateNewEntry();

        var entry = pool.Dequeue();
        entry.gameObject.SetActive(true);
        entry.transform.SetParent(parent, false);
        return entry;
    }

    public void ReturnToPool(CardProfileEntry entry)
    {
        entry.gameObject.SetActive(false);
        entry.transform.SetParent(poolParent, false);
        pool.Enqueue(entry);
    }
}
