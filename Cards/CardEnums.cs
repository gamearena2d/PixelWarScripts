namespace Game.Cards
{
    public enum CardRarity
    {
        COMMON,
        UNCOMMON,
        RARE,
        EPIC,
        MYTHIC,
        LEGENDARY,
        DIVINE
    }

    public enum CardType
    {
        UNIT,
        STRUCTURE,
        MAGIC
    }

    public enum AttackType
    {
        GROUND,
        AIR,
        ALL,
        NONE // Only for passive spells or structures
    }

    public enum TargetType
    {
        GROUND,
        AIR,
        ALL
    }

    public enum MovementType
    {
        Terra,
        Aria
    }
	
    public enum SpecialAbility
    {
        Lifesteal,
        Regeneration,
        Shield,
        Jump,
        Dash,
        Stun,
        Splash,
        SummonMinions,
        ReflectDamage
    }

    public enum ActiveAbilityType
    {
        Fireball,
        Invisibility,
        Rage,
        Teleport
    }
}