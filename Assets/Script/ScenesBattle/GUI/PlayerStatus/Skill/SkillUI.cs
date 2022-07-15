using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillUI : Singleton<SkillUI>
{
    public RectTransform[] skillSlot;
    public List<Skill_Slot> skill_Slots;
    public Skill_Slot skillPrefab;

    private SkillDataBase_SO skillDatas;

    protected override void Awake()
    {
        base.Awake();
        skillDatas = FindObjectOfType<PlayerControl>().playerAttribute.equippedSkills;
    }

    private void Start()
    {
        CreateSkillUI();
    }

    private void Update() {
        if(skill_Slots!=null)
        {
            for(int i = 0; i < skill_Slots.Count; i++) {
                SkillIconEnterCD(skill_Slots[i].curSkill.skillName);
            }
        }
    }

    public void CreateSkillUI()
    {
        if (skillDatas != null)
        {
            skill_Slots.Clear();
            for (int i = 0; i < skillDatas.skillDatabase.Count; i++)
            {
                var newSkillSlot = GameObject.Instantiate(skillPrefab, skillSlot[i]);
                newSkillSlot.SetSkillSlot(skillDatas.skillDatabase[i]);
                skill_Slots.Add(newSkillSlot.GetComponent<Skill_Slot>());
            }
        }
    }

    public Skill_Slot GetSkill_Slot(SkillName skillName)
    {
        return skill_Slots.Find(s => s.curSkill.skillName == skillName);
    }

    public void SkillIconEnterCD(SkillName skillName)
    {
        Skill_Slot newSkill = skill_Slots.Find(s => s.curSkill.skillName == skillName);
        if(newSkill!=null)
            newSkill.iconCD.fillAmount -= 1f / newSkill.curSkill.skillCD * Time.deltaTime;;
    }

}
