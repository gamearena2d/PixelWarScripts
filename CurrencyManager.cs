using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance;

    public int Gold { get; private set; }
    public int Gems { get; private set; }

    const string KEY_GOLD = "CURRENCY_GOLD";
    const string KEY_GEMS = "CURRENCY_GEMS";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Gold = PlayerPrefs.GetInt(KEY_GOLD, 0);
        Gems = PlayerPrefs.GetInt(KEY_GEMS, 0);
    }

    public void AddGold(int amount)  { Gold += Mathf.Max(0, amount); Save(); }
    public void AddGems(int amount)  { Gems += Mathf.Max(0, amount); Save(); }

    public bool SpendGold(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount; Save(); return true;
    }

    public bool SpendGems(int amount)
    {
        if (Gems < amount) return false;
        Gems -= amount; Save(); return true;
    }

    private void Save()
    {
        PlayerPrefs.SetInt(KEY_GOLD, Gold);
        PlayerPrefs.SetInt(KEY_GEMS, Gems);
        PlayerPrefs.Save();
    }
}
