using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PokemonTeam_SO", menuName = "Pokemon/PokemonTeam_SO")]
public class PokemonTeam_SO : ScriptableObject
{
    public List<PokemonDetails> team;

}

[System.Serializable]
public class PokemonDetails
{
    public string PUID;
    public string pokmeonName;
    public Nature nature;
    public int level;
    public int experience;
    public int currentExperience;
    public int upLevelExperience;
    public string holdingsItem;
    public int currentHP;
    public float criticalHit;
    public Statistic Stat;
    public IndividualValues individual;
    public BasePoints basePoints;
    public List<SkillName> skillName_List;

    public void InitPokemonTeam(PokemonAttribute pokemon)
    {
        PUID = pokemon.PUID;
        pokmeonName = pokemon.pokmeonName;
        nature = pokemon.nature;
        level = pokemon.level;
        experience = pokemon.experience;
        currentExperience = pokemon.currentExperience;
        upLevelExperience = pokemon.upLevelExperience;
        holdingsItem = pokemon.holdingsItem;
        currentHP = pokemon.currentHP;
        criticalHit = pokemon.criticalHit;
        Stat = pokemon.Stat;
        individual = pokemon.individual;
        basePoints = pokemon.basePoints;
        skillName_List = pokemon.equippedSkills.GetAllSkillName();
    }

    public PokemonAttribute SetPokemonTeam(PokemonAttribute pokemon)
    {
        pokemon.PUID = PUID;
        pokemon.pokmeonName = pokmeonName;
        pokemon.nature = nature;
        pokemon.level = level;
        pokemon.experience = experience;
        pokemon.currentExperience = currentExperience;
        pokemon.upLevelExperience = upLevelExperience;
        pokemon.holdingsItem = holdingsItem;
        pokemon.currentHP = currentHP;
        pokemon.criticalHit = criticalHit;
        pokemon.Stat = Stat;
        pokemon.individual = individual;
        pokemon.basePoints = basePoints;
        // 装备技能
        pokemon.equippedSkills.skillDatabase.Clear();   // 清空
        foreach (SkillName skillName in skillName_List)
        {
            // 根据已保存的装备技能重新添加
            pokemon.equippedSkills.skillDatabase.Add(SkillManager.Instance.skillDB.GetSkill(skillName));
        }
        

        return pokemon;
    }

}