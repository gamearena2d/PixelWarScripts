using System.Collections.Generic;
using UnityEngine;
using Game.Cards;

public static class UpgradeRequirements
{
    private static readonly Dictionary<CardRarity, int> baseLevels = new()
    {
        { CardRarity.COMMON,     1 },
        { CardRarity.UNCOMMON,   2 },
        { CardRarity.RARE,       3 },
        { CardRarity.EPIC,       4 },
        { CardRarity.MYTHIC,     5 },
        { CardRarity.LEGENDARY,  7 },
        { CardRarity.DIVINE,     9 },
    };

    // Base pattern per COMMON (12 livelli)
    private static readonly int[] basePattern = new[] { 20, 40, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240 };

    private static readonly Dictionary<CardRarity, float> rarityMultipliers = new()
    {
        { CardRarity.COMMON,     1f },
        { CardRarity.UNCOMMON,   0.5f },
        { CardRarity.RARE,       0.25f },
        { CardRarity.EPIC,       0.125f },
        { CardRarity.MYTHIC,     0.05f },
        { CardRarity.LEGENDARY,  0.025f },
        { CardRarity.DIVINE,     0.0125f }
    };

    private static readonly Dictionary<CardRarity, int[]> cardsRequired = new();
    private static readonly Dictionary<CardRarity, int[]> goldRequired = new();

    static UpgradeRequirements()
    {
        foreach (var entry in baseLevels)
        {
            var rarity = entry.Key;
            int startLevel = entry.Value;
            float multiplier = rarityMultipliers[rarity];
            int count = 12 - (startLevel - 1);

            int[] scaledCards = new int[count];
            int[] scaledGold = new int[count];

            for (int i = 0; i < count; i++)
            {
                scaledCards[i] = Mathf.Max(1, Mathf.RoundToInt(basePattern[startLevel - 1 + i] * multiplier));
                scaledGold[i] = Mathf.RoundToInt((basePattern[startLevel - 1 + i] * multiplier) * 100); // oro base: 100x carte
            }

            cardsRequired[rarity] = scaledCards;
            goldRequired[rarity] = scaledGold;
        }
    }

    public static int GetBaseLevel(CardRarity rarity)
    {
        return baseLevels[rarity];
    }

    public static int GetCardsRequired(CardRarity rarity, int level)
    {
        if (!baseLevels.ContainsKey(rarity)) return 0;
        int baseLevel = baseLevels[rarity];

        int index = level - baseLevel;
        if (index < 0 || index >= cardsRequired[rarity].Length) return 0;

        return cardsRequired[rarity][index];
    }

    public static int GetGoldRequired(CardRarity rarity, int level)
    {
        if (!baseLevels.ContainsKey(rarity)) return 0;
        int baseLevel = baseLevels[rarity];

        int index = level - baseLevel;
        if (index < 0 || index >= goldRequired[rarity].Length) return 0;

        return goldRequired[rarity][index];
    }
}
