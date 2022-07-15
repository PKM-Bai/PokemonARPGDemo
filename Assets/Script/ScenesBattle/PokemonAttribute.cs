using System;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "New Player", menuName = "Player/New BasePlayer")]
public class PokemonAttribute : ScriptableObject
{
    [TitleGroup("宝可梦属性")]
    [LabelText("缩略图")] public List<Sprite> pokemonRoleImage;
    [LabelText("唯一ID")] public string PUID;
    [LabelText("图鉴编号")] public string pokédexID;
    [LabelText("名字")] public string pokmeonName;
    [LabelText("性别")] public RoleSexType sex;
    [LabelText("性格")] public Nature nature;

    [LabelText("第一属性")] public PokemonType firstType;
    [LabelText("第二属性")] public PokemonType secondType;
    
    [LabelText("特性")] public Ability ability;
    [LabelText("等级")] public int level;
    [LabelText("初始升级所需经验")] public int experience;
    [LabelText("当前经验")] public int currentExperience;
    [LabelText("升级需要的经验")] public int upLevelExperience;
    [LabelText("持有物品")] public string holdingsItem;
    [LabelText("当前HP"), HorizontalGroup("HP", 120), LabelWidth(50), MinValue(0)] public int currentHP;
    [LabelText("暴击率"), LabelWidth(50), Range(0, 1)] public float criticalHit;
    
    [TitleGroup("能力值")] public Statistic Stat;
    
    [TitleGroup("种族值")] public SpeciesStrength speciesStrength;
    
    [TitleGroup("个体值")] public IndividualValues individual;

    [TitleGroup("努力值")] public BasePoints basePoints;
    [LabelText("当前装备的技能")] public SkillDataBase_SO equippedSkills;
    [LabelText("可学习的技能库")] public SkillDataBase_SO skillToCanLearn;
}

//* 特性
/// [CreateAssetMenu(fileName = "New Ability", menuName = "Player/New Ability")]
[Serializable]
public class Ability
{
    public enum AbilityEnum
    {
        None, 
        AttackAdd
    }
    
    [LabelText("特性名称")] public string abilityName;
    
    [LabelText("特性效果")] public AbilityEnum abilityEnum;
}

//* 种族值
[Serializable]
public class SpeciesStrength
{
    [LabelText("HP"), LabelWidth(50), MinValue(0), MaxValue(255)] public int HP;
    [LabelText("攻击"), LabelWidth(50), MinValue(0), MaxValue(255)] public int Attack;
    [LabelText("防御"), LabelWidth(50), MinValue(0), MaxValue(255)] public int Defense;
    [LabelText("特攻"), LabelWidth(50), MinValue(0), MaxValue(255)] public int SpecialAttack;
    [LabelText("特防"), LabelWidth(50), MinValue(0), MaxValue(255)] public int SpecialDefense;
    [LabelText("速度"), LabelWidth(50), MinValue(0), MaxValue(255)] public int Speed;
}

//* 个体值
[Serializable]
public class IndividualValues
{
    [LabelText("HP个体值"), LabelWidth(80), Range(0,31)] public int HPIV;
    [LabelText("攻击个体值"), LabelWidth(80), Range(0,31)] public int AttackIV;
    [LabelText("防御个体值"), LabelWidth(80), Range(0,31)] public int DefenseIV;
    [LabelText("特攻个体值"), LabelWidth(80), Range(0,31)] public int SpecialAttackIV;
    [LabelText("特防个体值"), LabelWidth(80), Range(0,31)] public int SpecialDefenseIV;
    [LabelText("速度个体值"), LabelWidth(80), Range(0,31)] public int SpeedIV;
}

//* 能力值
[Serializable]
public class Statistic
{
    [LabelText("最大HP"), LabelWidth(50), MinValue(0)] public int HP;
    [LabelText("攻击"), LabelWidth(50), MinValue(0)] public int Attack;
    [LabelText("防御"), LabelWidth(50), MinValue(0)] public int Defense;
    [LabelText("特攻"), LabelWidth(50), MinValue(0)] public int SpecialAttack;
    [LabelText("特防"), LabelWidth(50), MinValue(0)] public int SpecialDefense;
    [LabelText("速度"), LabelWidth(50), MinValue(0)] public int Speed;
}

//* 基础点数（努力值）
[Serializable]
public class BasePoints
{
    [LabelText("HP努力值"), LabelWidth(80), Range(0,252)] public int HP;
    [LabelText("攻击努力值"), LabelWidth(80), Range(0,252)] public int Attack;
    [LabelText("防御努力值"), LabelWidth(80), Range(0,252)] public int Defense;
    [LabelText("特攻努力值"), LabelWidth(80), Range(0,252)] public int SpecialAttack;
    [LabelText("特防努力值"), LabelWidth(80), Range(0,252)] public int SpecialDefense;
    [LabelText("速度努力值"), LabelWidth(80), Range(0,252)] public int Speed;
}

