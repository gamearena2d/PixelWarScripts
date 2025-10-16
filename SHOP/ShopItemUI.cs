using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopItemUI : MonoBehaviour
{
    public Image iconImage;
    public TMP_Text nameText;
    public TMP_Text priceText;
    public Image currencyIcon;
    public Button buyButton;

    private CardPackAsset currentPack;
    private ShopManager manager;

    public void Setup(CardPackAsset pack, ShopManager shop, Sprite goldIcon, Sprite gemsIcon)
    {
        currentPack = pack;
        manager = shop;

        nameText.text = pack.PackName;
        iconImage.sprite = pack.Icon;

        if (pack.PriceGold > 0)
        {
            priceText.text = pack.PriceGold.ToString();
            currencyIcon.sprite = goldIcon;
        }
        else
        {
            priceText.text = pack.PriceGems.ToString();
            currencyIcon.sprite = gemsIcon;
        }

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => manager.BuyPack(currentPack));
    }
}
