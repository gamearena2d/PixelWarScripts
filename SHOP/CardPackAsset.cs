using UnityEngine;
using Game.Cards;

[CreateAssetMenu(fileName = "NewCardPack", menuName = "Shop/CardPack")]
public class CardPackAsset : ScriptableObject
{
    public string PackName;
    public Sprite Icon;
    public int PriceGold;
    public int PriceGems;

    [Header("Pack Contents")]
    public int TotalCards = 5;
    public int RareCount = 1;
    public int EpicCount = 1;
    public bool ChanceLegendary = false;
}
