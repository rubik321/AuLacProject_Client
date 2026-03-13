using System.Collections.Generic;

namespace Rubik.CardPlayer
{
    [System.Serializable]
    public class CardSkillPassive
    {
        public TypePassive Index;
        public string Name;
        public string Detail;
        public string Image;
        public int MaxLv;
        public SkillValueByLevel[] Value;
    }

    [System.Serializable]
    public class CardSkillActive
    {
        public TypeActive Index;
        public string Name;
        public string Detail;
        public string Image;
        public int MaxLv;
        public SkillValueByLevel[] Value;
    }

    [System.Serializable]
    public class SkillValueByLevel
    {
        public int Level;
        public int[] Value;
    }

    [System.Serializable]
    public class CardSkillActiveLv
    {
        public TypeActive Index;
        public int Level;
        public bool IsLock;
        public int StarUnlock;
    }

    [System.Serializable]
    public class CardSkillPassiveLv
    {
        public TypePassive Index;
        public int Level;
        public bool IsLock;
        public int StarUnlock;
    }

    [System.Serializable]
    public class CardSkillLv
    {
        public CardSkillActiveLv Active;
        public List<CardSkillPassiveLv> Passive;
    }

    [System.Serializable]
    public class SkillLevelByStar
    {
        public int[] Active;
        public int[] Passive_1;
        public int[] Passive_2;
        public int[] Passive_3;
    }

    public enum TypeActive
    {
        Basic_skill = 0,
        Deal_250p_3_random_increase_15_SPD_all_allies_2_round = 1,
        Deal_195p_back = 2,
        Deal_260p_2_random_reduce_13_SPD_2_round = 3,
        Deal_330p_least_HP_apply_Bleed_35p_3_round = 4,
        Deal_138p_damage_3_random_reduce_15p_SPD_3_round_heal_all_allies_12p_Max_HP = 5,
        Deal_280p_damage_back_row_enemies_has_40p_silence_2_round = 6,
        Deal_240p_damage_back_row_enemies_has_50p_silence_2_round = 7,
        Deal_220p_damage_back_row_enemies_lowering_36_Rage_2_random_enemies = 8,
        Deal_190p_damage_all_enemies_has_30p_Freeze_2_round = 9,
        Deal_400p_damage_highest_ATK_affter_by_120p_previous = 10,
        Deal_200p_damage_highest_HP_plus_True_Dmg_10p_MaxHP_cap_10_ATK_burn_true_dmg_4p_MaxHP_2_round = 11,
        Deal_80p_damage_all_enemies_heal_least_HP_350p_of_ATK_has_40p_change_stun_1_enemy_2_round = 12,
        Slashes_through_a_line_of_enemies_with_an_sickle_dealing_100p_ATK_damage_Has_a_60p_chance_to_Freeze_affected_enemies_for_2_turns = 13,
        Pounces_on_a_random_enemy_dealing_200p_ATK_damage_Ignores_Block_Deals_additional_damage_equal_to_10p_of_the_targets_Max_HP_damage_cap_1000p_ATK = 14,
        Summons_a_storm_of_spectral_axes_unleashing_a_wave_of_frost_laced_force_Deals_0_ATK_damage_to_all_enemies_with_a_1_chance_to_Freeze_them_for_2_rounds = 15,
        Engulfs_his_hands_in_searing_flame_and_strikes_outward_unleashing_a_wave_of_fire_that_scorches_all_enemies_for_0_of_ATK_damage_and_burning_them_for_1_of_ATK_per_round_for_2_turns = 16,
        Unleashes_a_strike_on_the_two_enemies_with_the_lowest_HP_dealing_0p_ATK_damage_Ignores_Block_Inflicts_bonus_damage_equal_to_10p_of_the_targets_missing_HP_capped_at_1000p_ATK_If_the_targets_HP_is_lower_than_the_spiders_there_is_a_30p_chance_to_Stun_them_for_2_rounds = 17,
        Unleashes_a_blazing_hammer_strike_on_the_front_row_dealing_0p_ATK_damage_Increase_1p_ATK_and_2p_Armor_Break_for_3_rounds_Also_deals_bonus_damage_equal_to_4p_of_the_targets_Max_HP_damage_capped_at_5p_ATK = 18,
        Deals_0p_ATK_damage_to_all_enemies_and_inflicts_Poison_dealing_1p_ATK_damage_per_round_for_2_rounds_Increases_own_ATK_by_3p_and_Crit_by_20p_for_3_rounds = 19,
        Unleashes_a_thunderous_roar_infused_with_stormlight_dealing_0p_ATK_damage_to_all_enemies_Each_target_has_a_1p_chance_to_be_Stunned_for_2_rounds_and_a_3p_chance_to_lose_4p_Energy = 20,
        Unleashes_a_wave_of_scorching_flames_dealing_0p_ATK_damage_to_all_enemies_and_reducing_their_Speed_by_1_for_2_rounds_For_each_target_deals_an_additional_0p_ATK_damage_for_every_Burn_effect_currently_on_that_target_total_damage_capped_at_0p_ATK = 21,

        Lava_Quake_damage_all_90p_atk_10p_stun_2_round = 1001,
    }

    public enum TypePassive
    {
        BasicAttack = 0,
        Decrease_30_SPD_all = 1,
        Reduce_15p_DEF_font = 2,
        Buff_20p_DEF_20p_HP = 3,
        Buff_18p_ATK_13p_HP = 4,
        Attack_front_35p_stun_2_round = 5,
        Attack_back = 6,
        Attack_highest_HP_50p_silence_2_round = 7,
        Attack_100p_bleed_2_round_and_lowering_50_rage = 8,
        Hit_25p_freeze_attacked_2_round = 9,
        Attack_front_recover_110p_atk = 10,
        Attack_rand_3_burn_4p_max_hp_2_round = 11,
        ATK_20p_HP_10p_first_below_50p_HP_gain_100p_DR_2_round = 12,
        Attacked_gain_3p_DEF_2_round = 13,
        Deal_45p_extra_damage_to_Bleed_enemies = 14,
        Buff_25p_HP_35p_DEF = 15,
        Buff_35p_ATK = 16,
        Buff_10p_ATK_30p_CRT_20p_CRD = 17,
        Buff_20p_DEF_30_SPD = 18,
        Deal_70p_extra_damage_to_Frozen_enemies = 19,
        Buff_30p_ATK_15p_DEF_20p_CRT_20p_CRD = 20,
        Buff_30p_HP_20p_SkDR = 21,
        End_round_heal_3_highest_ATK_allies_180p_ATK_has_40p_increasr_15p_ATK_2_round = 22,
        Battle_start_grant_all_alies_10p_DR = 23,
        Basic_attacks_increase_CRT_20p_3_round = 24,
        Enemy_die_gain_10p_atk_this_battle = 25,
        First_below_50p_HP_reduce_20_SPD_and_Bleed_deal_300p_dmg_in_2_round = 26,
        Dead_has_50p_Freeze_all_eniemes_in_2_round_restore_400p_hp_of_atk_all_allies = 27,
        At_battle_start_gain_80p_atk = 28,
        When_affected_by_DoT_effect_gain_Shield_base_on_5p_of_MaxHP = 29,
        Dead_heal_all_allies_for_200_of_atk_increase_DR_8_in_3_round = 30,
        Basic_attacks_have_a_40_chance_to_Freeze_the_target_for_1_turn = 31,
        Deals_40p_extra_damage_to_enemies_affected_by_Freeze = 32,
        Gain_20p_ATK_and_15_SPD = 33,
        ATK_plus_10p_Crit_plus_30p_Crit_DMG_plus_30p = 34,
        Basic_attacks_deal_100p_ATK_damage_and_automatically_target_the_enemy_hero_with_the_lowest_HP = 35,
        Basic_attack_inflicts_Bleed_dealing_an_additional_30p_ATK_damage_per_round_for_2_turns = 36,
        HP_plus_0_ATK_plus_1_Block_plus_2 = 37,
        Reduces_all_damage_taken_by_0_and_decreases_the_attackers_ATK_by_1_for_2_rounds = 38,
        Each_basic_attack_deals_0_of_ATK_as_damage_and_restores_HP_equal_to_1_of_ATK = 39,
        Basic_attacks_deal_0_of_ATK_damage_and_carry_an_1_chance_to_sear_the_target_inflicting_Burn_for_2_turns_that_deals_65_of_ATK_each_round = 40,
        HP_plus_0_DEF_plus_1_Upon_death_unleashes_a_searing_curse_that_Burns_all_enemies_dealing_2_of_ATK_damage_per_round_for_3_turns = 41,
        When_struck_there_is_a_0_chance_to_Ignite_the_attacker_Burning_them_for_1_of_ATK_damage_per_round_over_2_turns = 42,
        Basic_attacks_always_target_the_enemy_with_the_lowest_HP_dealing_0p_ATK_damage_and_ignoring_Block = 43,
        Increases_HP_0p_Damage_by_1_Speed_by_2_and_Stun_Resistance_by_3 = 44,
        Whenever_an_enemy_dies_restores_0_Energy_and_heals_for_1_of_ATK_Also_gains_2_Damage_Increase_for_3_rounds = 45,
        Basic_attacks_deal_0p_ATK_damage_ignore_Block_and_steal_1p_of_the_targets_current_Armor_for_2_rounds = 46,
        IncreasesHP_by_0_ATK_by_1_and_Armor_by_2 = 47,
        During_the_first_0_rounds_of_battle_at_the_end_of_each_round_increases_own_Crit_by_1_Crit_DMG_by_2_and_Armor_Break_by_3_for_4_rounds = 48,
        Toxic_strength_seeps_into_its_body_fortifying_its_flesh_and_shell_boosting_HP_by_0_and_Armor_by_1_for_3_rounds = 49,
        The_Bears_toxic_essence_courses_through_the_battlefield_Whenever_an_ally_strikes_with_deadly_precision_the_Bear_siphons_vitality_through_the_poison_haze_restoring_HP_equal_to_0_of_its_ATK = 50,
        When_attacked_the_Bears_venomous_body_releases_a_toxic_backlash_reducing_the_attacker_s_Armor_Break_by_0_and_inflicting_Bleed_dealing_1_ATK_damage_per_round_for_2_rounds = 51,
        With_its_bone_and_steel_claws_wrapped_in_crackling_thunder_the_dragon_s_Basic_Attacks_deal_0p_ATK_damage_and_have_a_1p_chance_to_Stun_the_target_for_2_round = 52,
        Born_of_the_Dragon_Clan_lineage_and_wrapped_in_thunder_s_eternal_fury_the_dragon_inherits_0p_HP_1p_Skill_Damage_and_2p_Speed = 53,
        When_its_HP_falls_below_0p_for_the_first_time_the_dragon_abandons_its_defenses_It_sacrifices_1p_of_its_Armor_to_gain_2p_ATK_for_3_rounds_unleashing_its_primal_storm_wrought_fury = 54,
        Increases_HP_by_0p_ATK_by_1_and_Crit_by_2 = 55,
        Each_basic_attack_strikes_with_burning_fury_dealing_0p_ATK_damage_and_inflicting_Burn_which_scorches_the_target_for_1p_ATK_damage_per_round_for_2_rounds = 56,
        At_the_end_of_each_round_unleashes_waves_of_searing_fire_that_Burn_all_enemies_dealing_0p_ATK_damage_per_round_for_2_rounds_The_battlefield_itself_becomes_an_inferno_under_its_fury = 57,

        // Boss
        Boss_Reborn_When_Die = 1001,
        Magma_Slam_Smashes_the_ground_dealing_100p_of_ATK_and_burn_true_dmg_30p_of_ATK_for_2_turns = 1002,
        Immune_to_all_control_effects_including_Stun_Freeze_Petrify_and_Silence = 1003,
    }
}