﻿namespace D2NG.D2GS.Items
{
    public enum StatType
    {
        Strength = 0x00,
        Energy,
        Dexterity,
        Vitality,
        AllAttributes,
        NewSkills1,
        NewSkills2,
        Life,
        MaximumLife,
        Mana,
        MaximumMana,
        MaximumStamina,
        Level,
        Experience,
        Gold,
        Bank,
        EnhancedDefense,
        EnhancedMaximumDamage,
        EnhancedMinimumDamage,
        AttackRating,
        IncreasedBlocking,
        MinimumDamage,
        MaximumDamage,
        SecondaryMinimumDamage,
        SecondaryMaximumDamage,
        EnhancedDamage,
        ManaRecovery,
        ManaRecoveryBonus,
        StaminaRecoveryBonus,
        LastExperience,
        NextExperience,
        Defense,
        DefenseVsMissiles,
        DefenseVsMelee,
        DamageReduction,
        MagicalDamageReduction,
        DamageReductionPercent,
        MagicalDamageReductionPercent,
        MaximumMagicalDamageReductionPercent,
        FireResistance,
        MaximumFireResistance,
        LightningResistance,
        MaximumLightningResistance,
        ColdResistance,
        MaximumColdResistance,
        PoisonResistance,
        MaximumPoisonResistance,
        DamageAura,
        MinimumFireDamage,
        MaximumFireDamage,
        MinimumLightningDamage,
        MaximumLightningDamage,
        MinimumMagicalDamage,
        MaximumMagicalDamage,
        MinimumColdDamage,
        MaximumColdDamage,
        ColdDamageLength,
        MinimumPoisonDamage,
        MaximumPoisonDamage,
        PoisonDamageLength,
        MinimumLifeStolenPerHit,
        MaximumLifeStolenPerHit,
        MinimumManaStolenPerHit,
        MaximumManaStolenPerHit,
        MinimumStaminaDrain,
        MaximumStaminaDrain,
        StunLength,
        VelocityPercent,
        AttackRate,
        OtherAnimationRate,
        Quantity,
        Value,
        Durability,
        MaximumDurability,
        ReplenishLife,
        EnhancedMaximumDurability,
        EnhancedLife,
        EnhancedMana,
        AttackerTakesDamage,
        ExtraGold,
        BetterChanceOfGettingMagicItem,
        Knockback,
        TimeDuration,
        AmazonSkills,
        PaladinSkills,
        NecromancerSkills,
        SorceressSkills,
        BarbarianSkills,
        UnsentParameter,
        AddExperience,
        LifeAfterEachKill,
        ReduceVendorPrices,
        DoubleHerbDuration,
        LightRadius,
        LightColour,
        ReducedRequirements,
        ReducedLevelRequirement,
        IncreasedAttackSpeed,
        ReducedLevelRequirementPercent,
        LastBlockFrame,
        FasterRunWalk,
        NonClassSkill,
        State,
        FasterHitRecovery,
        MonsterPlayerCount,
        SkillPoisonOverrideLength,
        FasterBlockRate,
        SkillBypassUndead,
        SkillBypassDemons,
        FasterCastRate,
        SkillBypassBeasts,
        SingleSkill1,
        SingleSkill2,
        SingleSkill3,
        PoisonLengthReduction,
        AddsDamage,
        HitCausesMonsterToFlee,
        HitBlindsTarget,
        DamageToMana,
        IgnoreTargetsDefense,
        ReduceTargetsDefense,
        PreventMonsterHeal,
        HalfFreezeDuration,
        ToHitPercent,
        MonsterDefenseReductionPerHit,
        DamageToDemons,
        DamageToUndead,
        AttackRatingAgainstDemons,
        AttackRatingAgainstUndead,
        Throwable,
        FireSkills,
        AllSkills,
        AttackerTakesLightningDamage,
        IronMaidenLevel,
        LifetapLevel,
        ThornsPercent,
        BoneArmor,
        MaximumBoneArmor,
        FreezesTarget,
        OpenWounds,
        CrushingBlow,
        KickDamage,
        ManaAfterEachKill,
        LifeAfterEachDemonKill,
        ExtraBlood,
        DeadlyStrike,
        FireAbsorptionPercent,
        FireAbsorption,
        LightningAbsorptionPercent,
        LightningAbsorption,
        MagicAbsorptionPercent,
        MagicAbsorption,
        ColdAbsorptionPercent,
        ColdAbsorption,
        SlowsDownEnemies,
        Aura,
        Indestructible,
        CannotBeFrozen,
        StaminaDrainPercent,
        Reanimate,
        PiercingAttack,
        FiresMagicArrows,
        FireExplosiveArrows,
        MinimumThrowingDamage,
        MaximumThrowingDamage,
        SkillHandOfAthena,
        SkillStaminaPercent,
        SkillPassiveStaminaPercent,
        Concentration,
        Enchant,
        Pierce,
        Conviction,
        ChillingArmor,
        Frenzy,
        Decrepify,
        SkillArmorPercent,
        Alignment,
        Target0,
        Target1,
        DruidSkills,
        AssassinSkills,
        GoldLost,
        ConversionLevel,
        ConversionMaximumLife,
        UnitDoOverlay,
        AttackRatingAgainstMonsterType,
        DamageToMonsterType,
        SingleSkill4,
        ArmorOverridePercent,
        Unused183,
        Unused184,
        Unused185,
        Unused186,
        Unused187,
        SkillTab,
        Unused189,
        Unused190,
        Unused191,
        Unused192,
        Unused193,
        SocketCount,
        SkillOnStriking,
        SkillOnKill,
        SkillOnDeath,
        SkillOnHit,
        SkillOnLevelUp,
        Unused200,
        SkillWhenStruck,
        Unused202,
        Unused203,
        Charged,
        Unused204,
        Unused205,
        Unused206,
        Unused207,
        Unused208,
        Unused209,
        Unused210,
        Unused211,
        Unused212,
        DefensePerLevel,
        EnhancedDefensePerLevel,
        LifePerLevel,
        ManaPerLevel,
        MaximumDamagePerLevel,
        MaximumEnhancedDamagePerLevel,
        StrengthPerLevel,
        DexterityPerLevel,
        EnergyPerLevel,
        VitalityPerLevel,
        AttackRatingPerLevel,
        BonusToAttackRatingPerLevel,
        MaximumColdDamagePerLevel,
        MaximumFireDamagePerLevel,
        MaximumLightningDamagePerLevel,
        MaximumPoisonDamagePerLevel,
        ColdResistancePerLevel,
        FireResistancePerLevel,
        LightningResistancePerLevel,
        PoisonResistancePerLevel,
        ColdAbsorptionPerLevel,
        FireAbsorptionPerLevel,
        LightningAbsorptionPerLevel,
        PoisonAbsorptionPerLevel,
        ThornsPerLevel,
        ExtraGoldPerLevel,
        BetterChanceOfGettingMagicItemPerLevel,
        StaminaRegenerationPerLevel,
        StaminaPerLevel,
        DamageToDemonsPerLevel,
        DamageToUndeadPerLevel,
        AttackRatingAgainstDemonsPerLevel,
        AttackRatingAgainstUndeadPerLevel,
        CrushingBlowPerLevel,
        OpenWoundsPerLevel,
        KickDamagePerLevel,
        DeadlyStrikePerLevel,
        FindGemsPerLevel,
        RepairsDurability,
        ReplenishesQuantity,
        IncreasedStackSize,
        FindItem,
        SlashDamage,
        SlashDamagePercent,
        CrushDamage,
        CrushDamagePercent,
        ThrustDamage,
        ThrustDamagePercent,
        SlashDamageAbsorption,
        CrushDamageAbsorption,
        ThrustDamageAbsorption,
        SlashDamageAbsorptionPercent,
        CrushDamageAbsorptionPercent,
        ThrustDamageAbsorptionPercent,
        DefensePerTime,
        EnhancedDefensePerTime,
        LifePerTime,
        ManaPerTime,
        MaximumDamagePerTime,
        MaximumEnhancedDamagePerTime,
        StrengthPerTime,
        DexterityPerTime,
        EnergyPerTime,
        VitalityPerTime,
        AttackRatingPerTime,
        ChanceToHitPerTime,
        MaximumColdDamagePerTime,
        MaximumFireDamagePerTime,
        MaximumLightningDamagePerTime,
        MaximumDamagePerPoison,
        ColdResistancePerTime,
        FireResistancePerTime,
        LightningResistancePerTime,
        PoisonResistancePerTime,
        ColdAbsorptionPerTime,
        FireAbsorptionPerTime,
        LightningAbsorptionPerTime,
        PoisonAbsorptionPerTime,
        ExtraGoldPerTime,
        BetterChanceOfGettingMagicItemPerTime,
        RegenerateStaminaPerTime,
        StaminaPerTime,
        DamageToDemonsPerTime,
        DamageToUndeadPerTime,
        AttackRatingAgainstDemonsPerTime,
        AttackRatingAgainstUndeadPerTime,
        CrushingBlowPerTime,
        OpenWoundsPerTime,
        KickDamagePerTime,
        DeadlyStrikePerTime,
        FindGemsPerTime,
        EnemyColdResistanceReduction,
        EnemyFireResistanceReduction,
        EnemyLightningResistanceReduction,
        EnemyPoisonResistanceReduction,
        DamageVsMonsters,
        EnhancedDamageVsMonsters,
        AttackRatingAgainstMonsters,
        BonusToAttackRatingAgainstMonsters,
        DefenseVsMonsters,
        EnhancedDefenseVsMonsters,
        FireDamageLength,
        MinimumFireDamageLength,
        MaximumFireDamageLength,
        ProgressiveDamage,
        ProgressiveSteal,
        ProgressiveOther,
        ProgressiveFire,
        ProgressiveCold,
        ProgressiveLightning,
        ExtraCharges,
        ProgressiveAttackRating,
        PoisonCount,
        DamageFramerate,
        PierceIdx,
        FireMastery,
        LightningMastery,
        ColdMastery,
        PoisonMastery,
        PassiveEnemyFireResistanceReduction,
        PassiveEnemyLightningResistanceReduction,
        PassiveEnemyColdResistanceReduction,
        PassiveEnemyPoisonResistanceReduction,
        CriticalStrike,
        Dodge,
        Avoid,
        Evade,
        Warmth,
        MeleeAttackRatingMastery,
        MeleeDamageMastery,
        MeleeCriticalHitMastery,
        ThrownWeaponAttackRatingMastery,
        ThrownWeaponDamageMastery,
        ThrownWeaponCriticalHitMastery,
        WeaponBlock,
        SummonResist,
        ModifierListSkill,
        ModifierListLevel,
        LastSentLifePercent,
        SourceUnitType,
        SourceUnitId,
        ShortParameter1,
        QuestItemDifficulty,
        PassiveMagicalDamageMastery,
        PassiveMagicalResistanceReduction
    }
}