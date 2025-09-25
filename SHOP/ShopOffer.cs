using UnityEngine;
using Game.Cards;
using Game.Shop;

[CreateAssetMenu(fileName = "NewShopOffer", menuName = "Shop/Create New Offer")]
public class ShopOfferAsset : ScriptableObject
{
    public string OfferName;
    public ShopOfferType Type;
    public ShopItemRarity Rarity;
    public ShopCurrency Currency;
    public int Price;

    [Header("DailyCard / SingleCard")]
    public Card CardRef;
    public int CardCopies;
    public int MaxPurchases;

    [Header("CardPack")]
    public int TotalCards;
    public int RareCount;
    public int EpicCount;
    public bool ChanceLegendary;

    [Header("Gold/Gems/Chest")]
    public int RewardGold;
    public int RewardGems;
    public GameObject ChestPrefab;

    public Sprite Icon;
}
