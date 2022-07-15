using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Skill_Slot : MonoBehaviour
{
    public Image iconCD;
    public Skill_SO curSkill;
    public Image skillIcon;
    public TextMeshProUGUI skillName;


    public void SetSkillSlot(Skill_SO skill)
    {
        curSkill = skill;

        // 技能图标
        // skillIcon.sprite = skill.image;
        
        skillName.text = skill.skillName.ToString();
    }

}
