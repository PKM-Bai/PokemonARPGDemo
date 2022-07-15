using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragSkill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    SkillPanel skillPanel;

    EquippedSkill_Slot currentSkill;
    EquippedSkill_Slot targetSkill;

    private void Awake()
    {
        skillPanel = FindObjectOfType<SkillPanel>();
        currentSkill = transform.GetComponent<EquippedSkill_Slot>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //* 记录原始数据
        skillPanel.currentDragData = new SkillPanel.DragData();
        skillPanel.currentDragData.origianlParent = transform.parent as RectTransform;

        transform.SetParent(skillPanel.dragCanvas.transform, true);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //* 跟随鼠标位置移动
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //* 放下 交换数据
        //?是否指向Skill_slot（技能槽）
        if (EventSystem.current.IsPointerOverGameObject())  //当前鼠标的射线是否在UI
        {
            if (skillPanel.CheckInEquippedSkillUI(eventData.position))
            {
                if (eventData.pointerEnter.gameObject.GetComponent<EquippedSkill_Slot>())
                    targetSkill = eventData.pointerEnter.gameObject.GetComponent<EquippedSkill_Slot>();
                else
                    targetSkill = eventData.pointerEnter.gameObject.GetComponentInParent<EquippedSkill_Slot>();
                if(targetSkill != null)
                    ReplaceSkill();
            }
        }
        transform.SetParent(skillPanel.currentDragData.origianlParent, true);
        transform.GetComponent<RectTransform>().localPosition = new Vector3(0, -2.5f, 0);
    }

    public void ReplaceSkill()
    {
        var skillDataBase = skillPanel.roleInterface.curSelectPokemon.equippedSkills.skillDatabase;
        var tempSkill = currentSkill;

        for (int i = 0; i < skillDataBase.Count; i++)
        {
            if (skillDataBase[i].id == currentSkill.skill.id)
                skillDataBase[i] = targetSkill.skill;
            else if (skillDataBase[i].id == targetSkill.skill.id)
                skillDataBase[i] = tempSkill.skill;
        }
        transform.SetParent(targetSkill.transform as RectTransform, true);
        if(SkillUI.Instance)
        {
            SkillUI.Instance.CreateSkillUI();
        }
        EventHandler.CallSelectPokemonRole(skillPanel.roleInterface.curSelectPokemon);
    }

}
