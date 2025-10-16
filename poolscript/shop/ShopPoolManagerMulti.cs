using System.Collections.Generic;
using UnityEngine;
using Game.Shop;

public class ShopPoolManagerMulti : MonoBehaviour
{
    public static ShopPoolManagerMulti Instance;

    [System.Serializable]
    public class PoolEntry
    {
        public ShopOfferType type;       // Tipo di offerta
        public GameObject prefab;        // Prefab da usare
        public int initialSize = 5;      // Numero iniziale di oggetti nel pool
    }

    [Header("Pool Settings")]
    public List<PoolEntry> poolsConfig;
    public Transform poolParent;

    // Pool veri e propri
    private Dictionary<ShopOfferType, Queue<GameObject>> pools = new();
    private Dictionary<ShopOfferType, HashSet<GameObject>> activeItems = new();

    // Contatori per assegnare nomi univoci
    private Dictionary<ShopOfferType, int> prefabCounters = new();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        DontDestroyOnLoad(gameObject);

        InitializePools();
    }

    private void InitializePools()
    {
        foreach (var entry in poolsConfig)
        {
            if (!pools.ContainsKey(entry.type))
            {
                pools[entry.type] = new Queue<GameObject>();
                activeItems[entry.type] = new HashSet<GameObject>();
                prefabCounters[entry.type] = 0;
            }

            for (int i = 0; i < entry.initialSize; i++)
            {
                var obj = CreateNewItem(entry.prefab, entry.type);
                pools[entry.type].Enqueue(obj);
            }
        }
    }

    private GameObject CreateNewItem(GameObject prefab, ShopOfferType type)
    {
        GameObject obj = Instantiate(prefab, poolParent);
        obj.SetActive(false);

        // Assegna un nome univoco tipo CardPack_0, GoldPack_0...
        obj.name = $"{prefab.name}_{prefabCounters[type]}";
        prefabCounters[type]++;

        return obj;
    }

    public GameObject GetItemFromPool(ShopOfferType type)
    {
        if (!pools.ContainsKey(type))
        {
            Debug.LogWarning($"[ShopPool] Nessun pool per {type}");
            return null;
        }

        GameObject obj;
        if (pools[type].Count > 0) obj = pools[type].Dequeue();
        else
        {
            var prefab = poolsConfig.Find(p => p.type == type)?.prefab;
            if (prefab == null)
            {
                Debug.LogError($"[ShopPool] Prefab mancante per {type}");
                return null;
            }
            obj = CreateNewItem(prefab, type);
        }

        obj.SetActive(true);
        activeItems[type].Add(obj);
        return obj;
    }

    public void ReturnItemToPool(ShopOfferType type, GameObject obj)
    {
        if (obj == null || !pools.ContainsKey(type)) return;

        obj.SetActive(false);
        obj.transform.SetParent(poolParent);
        activeItems[type].Remove(obj);
        pools[type].Enqueue(obj);
    }

    public void ReturnAllToPool()
    {
        foreach (var kvp in activeItems)
        {
            foreach (var obj in kvp.Value)
            {
                if (obj != null)
                {
                    obj.SetActive(false);
                    obj.transform.SetParent(poolParent);
                    pools[kvp.Key].Enqueue(obj);
                }
            }
            kvp.Value.Clear();
        }
    }
}
