using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

namespace Game.Cards
{
    [System.Serializable]
    public class MinionData
    {
        [Header("Minion Base Stats")]
        public int Health;
        public int Damage;
        public float AttackSpeed;
        public float MoveSpeed;
        public float Range;
        public CardType Type;
        public TargetType TargetType;

        [Header("Minion Spawn Settings")]
        public int NumberToSpawn;
        public int MaxAlive;
        public float SpawnCooldown;

        [Header("Minion Level Up Progression")]
        public int HealthPerLevel;
        public int DamagePerLevel;
        public float AttackSpeedPerLevel;
        public float MoveSpeedPerLevel;
    }

    [System.Serializable]
    public class LevelUpStats
    {
        [Header("Card Level Up Progression")]
        public int HealthPerLevel;
        public int DamagePerLevel;
        public float AttackSpeedPerLevel;
        public float MoveSpeedPerLevel;

        [Header("Abilities Upgrade (0/1 per livello, se presenti)")]
        public int SplashDamagePerLevel;
        public int LifestealPerLevel;
        public int ShieldPerLevel;
        public int DashPerLevel;
        public int JumpPerLevel;
        public int RegenerationPerLevel;
    }

    [CreateAssetMenu(fileName = "NewCard", menuName = "Card/Create New Card")]
    public class Card : ScriptableObject
    {
        [Header("Base Info")]
        public string Name;
        public Sprite CardIcon;

        [Header("Prefabs per livello")]
        public GameObject UnitPrefabLow;   // 1-5
        public GameObject UnitPrefabMid;   // 6-10
        public GameObject UnitPrefabHigh;  // 11-12 (non usato da MYTHIC in su)

        [Header("Type & Rarity")]
        public CardRarity Rarity;
        public CardType Type;
        public TargetType TargetType;
        public MovementType MovementType;

        [Header("Base Stats")]
        public int Health;
        public int Damage;
        public float MoveSpeed;
        public float AttackSpeed;
        public float Range;

        [Header("Costs (Automatic)")]
        public int GoldCost;
        public int GemCost;
        public float EuroCost;

        [Header("Mana")]
        public int ManaCost;

        [Header("Level Progression")]
        public int BaseLevel;
        public int MinLeagueRequired;
        public LevelUpStats LevelProgression;

        [Header("Minion Data")]
        public bool SpawnsMinions;
        public MinionData Minion;

        [Header("Abilities")]
        public bool HasSplashDamage;
        public bool HasShield;
        public float ShieldHp;
        public int ShieldCharges;
        public bool HasDash;
        public float DashDistance;
        public bool HasLifesteal;
        public float LifestealPercent;
        public bool HasJump;
        public bool HasRegeneration;
        public float RegenerationPerSecond;

        [Header("Special Ability Description")]
        [TextArea]
        public string AbilityDescription;

        [Header("Unlock / Upgrade")]
        public int RequiredTrophies;
        public int CurrentCount;
        public int RequiredCount;

        [Header("Optional Video Preview")]
        public VideoClip CardPreviewVideo;

        private void OnValidate()
        {
            if (Type == CardType.STRUCTURE)
                MoveSpeed = 0f;

            SetBaseLevelByRarity();
            SetBaseCostsByRarity();
            SetupLevelProgression();
            SetupMinionProgression();
        }

        private void SetBaseLevelByRarity()
        {
            BaseLevel = Rarity switch
            {
                CardRarity.COMMON => 1,
                CardRarity.UNCOMMON => 2,
                CardRarity.RARE => 3,
                CardRarity.EPIC => 4,
                CardRarity.MYTHIC => 5,
                CardRarity.LEGENDARY => 7,
                CardRarity.DIVINE => 9,
                _ => 1
            };
        }

        private void SetBaseCostsByRarity()
        {
            switch (Rarity)
            {
                case CardRarity.COMMON: GoldCost = 10; break;
                case CardRarity.UNCOMMON: GoldCost = 50; break;
                case CardRarity.RARE: GoldCost = 200; break;
                case CardRarity.EPIC: GoldCost = 400; break;
                case CardRarity.MYTHIC: GoldCost = 800; break;
                case CardRarity.LEGENDARY: GoldCost = 2000; break;
                case CardRarity.DIVINE: GoldCost = 8000; break;
                default: GoldCost = 10; break;
            }
        }

        private void SetupLevelProgression()
        {
            LevelProgression.HealthPerLevel = Mathf.CeilToInt(Health * 0.1f);
            LevelProgression.DamagePerLevel = Mathf.CeilToInt(Damage * 0.1f);
            LevelProgression.MoveSpeedPerLevel = MoveSpeed * 0.05f;
            LevelProgression.AttackSpeedPerLevel = AttackSpeed * 0.05f;

            LevelProgression.SplashDamagePerLevel = HasSplashDamage ? 1 : 0;
            LevelProgression.LifestealPerLevel = HasLifesteal ? 1 : 0;
            LevelProgression.ShieldPerLevel = HasShield ? 1 : 0;
            LevelProgression.DashPerLevel = HasDash ? 1 : 0;
            LevelProgression.JumpPerLevel = HasJump ? 1 : 0;
            LevelProgression.RegenerationPerLevel = HasRegeneration ? 1 : 0;
        }

        private void SetupMinionProgression()
        {
            if (!SpawnsMinions || Minion == null) return;

            Minion.HealthPerLevel = Mathf.CeilToInt(Minion.Health * 0.1f);
            Minion.DamagePerLevel = Mathf.CeilToInt(Minion.Damage * 0.1f);
            Minion.AttackSpeedPerLevel = Minion.AttackSpeed * 0.05f;
            Minion.MoveSpeedPerLevel = Minion.MoveSpeed * 0.05f;
        }

        /// <summary>
        /// Restituisce automaticamente il prefab corretto in base al livello attuale.
        /// </summary>
        public GameObject GetPrefabForLevel(int level)
        {
            bool isHighRarity = Rarity == CardRarity.MYTHIC || Rarity == CardRarity.LEGENDARY || Rarity == CardRarity.DIVINE;

            if (level <= 5) return UnitPrefabLow;
            if (level <= 10) return UnitPrefabMid;

            // Prefab alto solo per rarità basse
            if (!isHighRarity && level >= 11) return UnitPrefabHigh;

            // fallback di sicurezza
            return UnitPrefabMid != null ? UnitPrefabMid : UnitPrefabLow;
        }

        public void SetEuroCost(float cost)
        {
            EuroCost = cost;
        }
    }
}
