using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ShopManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject shopPanel;
    public Transform contentParent;
    public GameObject shopItemPrefab;

    [Header("Icons")]
    public Sprite goldIcon;
    public Sprite gemsIcon;

    [Header("Shop Config")]
    public List<CardPackAsset> cardPacks;

    private void Start()
    {
        OpenShop();
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        PopulateShop();
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);
    }

    private void PopulateShop()
    {
        foreach (var pack in cardPacks)
        {
            GameObject go = Instantiate(shopItemPrefab, contentParent);
            ShopItemUI ui = go.GetComponent<ShopItemUI>();
            if (ui != null)
                ui.Setup(pack, this, goldIcon, gemsIcon);
        }
    }

    public void BuyPack(CardPackAsset pack)
    {
        bool paid = false;

        if (CurrencyManager.Instance.SpendGold(pack.PriceGold))
            paid = true;
        else if (CurrencyManager.Instance.SpendGems(pack.PriceGems))
            paid = true;

        if (!paid)
        {
            Debug.Log("Valuta insufficiente!");
            return;
        }

        CardPackManager.Instance.OpenPack(pack);
    }
}
