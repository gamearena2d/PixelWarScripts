using UnityEngine;
using System.Collections.Generic;
using Game.Cards;

public class DailyCardsManager : MonoBehaviour
{
    public List<DailyCardSlot> slots;
    public float refreshTimeHours = 24f;

    private float nextRefresh;

    private void Start()
    {
        RefreshCards();
    }

    private void RefreshCards()
    {
        nextRefresh = Time.time + refreshTimeHours * 3600f;

        List<Card> allCards = new List<Card>(Resources.LoadAll<Card>("Cards"));

        for (int i = 0; i < slots.Count; i++)
        {
            Card c = allCards[Random.Range(0, allCards.Count)];
            int price = c.Rarity == CardRarity.COMMON ? 50 :
                        c.Rarity == CardRarity.RARE ? 200 :
                        c.Rarity == CardRarity.EPIC ? 1000 : 5000;
            bool gems = (c.Rarity == CardRarity.LEGENDARY); // esempio: leggendaria solo con gemme

            slots[i].Setup(c, price, gems);
        }
    }

    private void Update()
    {
        if (Time.time >= nextRefresh)
            RefreshCards();
    }
}
