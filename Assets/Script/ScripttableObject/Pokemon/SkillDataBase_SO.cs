using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataBase_SO", menuName = "Pokemon/Skill/SkillDataBase_SO")]
public class SkillDataBase_SO : ScriptableObject
{
    public List<Skill_SO> skillDatabase;

    public Skill_SO GetSkill(SkillName skillName)
    {
        return skillDatabase.Find(s => s.skillName == skillName);
    }

    public List<SkillName> GetAllSkillName()
    {
        List<SkillName> skillIDs = new List<SkillName>();

        for (int i = 0; i < skillDatabase.Count; i++)
        {
            skillIDs.Add(skillDatabase[i].skillName);
        }

        return skillIDs;
    }
}
