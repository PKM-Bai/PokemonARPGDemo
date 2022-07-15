using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillEffectDataBase_SO", menuName = "Pokemon/Skill/SkillEffectDataBase")]
public class SkillEffectDataBase_SO : ScriptableObject
{
    public List<SkillEffectPrefab> skillEffectPrefabs;

    public SkillEffectPrefab GetSkillEffectPrefab(SkillName skillName)
    {
        return skillEffectPrefabs.Find(s => s.skillName == skillName);
    }
}


[System.Serializable]
public class SkillEffectPrefab
{
    public SkillName skillName;
    public Transform prefab;
}