using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

public class SkillPanel : MonoBehaviour
{
    //* 鼠标拖动替换已携带的技能
    public class DragData
    {
        public EquippedSkill_Slot originalHolder;
        public RectTransform origianlParent;
    }
    [Header("Data")]
    public Canvas dragCanvas;
    public DragData currentDragData;

    public RoleInterface roleInterface;
    public Skill_SlotParent equippedSkill_SlotParent;
    private EquippedSkill_Slot equippedSkill_Slot;
    public StudySkill_Slot studySkill_Slot;
    public SkillHurtTypes_SO skillHurtTypes;
    public PokemonType_SO pokemonTypes;

    [Header("UI")]
    public EquippedSkillsUI equippedSkillsUI;
    public GameObject studySkillPanel_1;
    public GameObject studySkillPanel_2;
    public StudySkillsUI[] studySkillsUIs;
    public ShowSkillDetailsUI showSkillDetailsUI;
    public SkillInfoTip skillInfoTip;
    public EquippedSkill_MRC equippedSkill_MRC;
    public StudySkill_MRC studySkill_MRC;

    private List<Skill_SO> equippedSkillList;
    private List<Skill_SO> studySkillList;

    private void OnEnable()
    {
        if (roleInterface.curSelectPokemon != null)
            OnShowPokemonSkills(roleInterface.curSelectPokemon);

        EventHandler.SelectPokemonRole += OnShowPokemonSkills;
    }

    private void OnDisable()
    {
        EventHandler.SelectPokemonRole -= OnShowPokemonSkills;

        skillInfoTip.gameObject.SetActive(false);
        equippedSkill_MRC.gameObject.SetActive(false);
        studySkill_MRC.gameObject.SetActive(false);
    }

    public void Init()
    {
        equippedSkillsUI.gameObject.SetActive(true);
        studySkillPanel_1.SetActive(false);
        studySkillPanel_2.SetActive(false);
    }

    public void OnShowPokemonSkills(PokemonAttribute pokemon)
    {
        CreatePokemonEquippedSkills(pokemon);
        CreatePokemonStudySkills(pokemon);
        equippedSkillsUI.LoadChildSlot();
    }

    //* 创建当前宝可梦已装备的技能列表
    private void CreatePokemonEquippedSkills(PokemonAttribute pokemon)
    {
        //* 删除所有技能
        equippedSkillsUI.DestroyAllChild();
        showSkillDetailsUI.gameObject.SetActive(false);

        //* 再读取目前的技能列表 重新生成
        equippedSkillList = pokemon.equippedSkills.skillDatabase;
        if (equippedSkillList.Count != 0)
        {
            equippedSkill_Slot = equippedSkill_SlotParent.GetComponentInChildren<EquippedSkill_Slot>();
            for (int i = 0; i < equippedSkillList.Count; i++)
            {
                equippedSkill_Slot.skill = equippedSkillList[i];
                equippedSkill_Slot.type.sprite = pokemonTypes.pokemonTypeList.Find(o => o.pokemonType == equippedSkillList[i].type).typeImage;
                equippedSkill_Slot.skillName.text = equippedSkillList[i].skillName.ToString();
                equippedSkill_Slot.power.text = "威力：" + equippedSkillList[i].power.ToString();
                equippedSkill_Slot.PP.text = equippedSkillList[i].currentPP.ToString() + "/" + equippedSkillList[i].maxPP.ToString();
                equippedSkill_Slot.GetComponent<Image>().enabled = false;
                equippedSkill_SlotParent.slot_index = i;
                Instantiate(equippedSkill_SlotParent, equippedSkill_Slot.transform.position, Quaternion.identity, equippedSkillsUI.transform);
            }
        }
    }

    //* 创建当前宝可梦可学习的技能列表
    private void CreatePokemonStudySkills(PokemonAttribute pokemon)
    {
        //* 删除所有技能
        for (int i = 0; i < studySkillsUIs.Length; i++)
        {
            studySkillsUIs[i].DestroyAllChild();
        }
        // showSkillDetailsUI.gameObject.SetActive(false);

        Transform parent;
        //* 读取目前可学习的技能列表，重新生成
        studySkillList = pokemon.skillToCanLearn.skillDatabase;
        studySkillList.Sort((a, b) => a.reachLevel.CompareTo(b.reachLevel));    //? 根据技能解锁等级升序排序

        if (studySkillList.Count != 0)
        {
            for (int i = 0; i < studySkillList.Count; i++)
            {
                // studySkill_Slot.skillIcon.sprite = studySkillList[i].image;
                studySkill_Slot.skillName.text = studySkillList[i].skillName.ToString();
                studySkill_Slot.skill = studySkillList[i];
                //* 判断是否达到可学习该技能的等级
                if (pokemon.level >= studySkillList[i].reachLevel)
                {
                    studySkill_Slot.isLearn = true;
                    studySkill_Slot.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1);
                    // studySkill_Slot.GetComponent<Button>().interactable = true;
                }
                else
                {
                    studySkill_Slot.isLearn = false;
                    studySkill_Slot.transform.GetChild(0).GetComponent<Image>().color = new Color(0.5647059f, 0.5647059f, 0.5647059f);
                }
                switch (studySkillList[i].category)
                {
                    case SkillType.物理招式:
                        parent = studySkillsUIs[0].GetComponent<Transform>();
                        break;
                    case SkillType.特殊招式:
                        parent = studySkillsUIs[1].GetComponent<Transform>();
                        break;
                    case SkillType.变化招式:
                        parent = studySkillsUIs[2].GetComponent<Transform>();
                        break;
                    case SkillType.通用招式:
                        parent = studySkillsUIs[3].GetComponent<Transform>();
                        break;
                    case SkillType.秘技招式:
                        parent = studySkillsUIs[4].GetComponent<Transform>();
                        break;
                    default:
                        parent = studySkillsUIs[0].GetComponent<Transform>();
                        break;
                }
                Instantiate(studySkill_Slot, studySkill_Slot.transform.position, Quaternion.identity, parent);
            }
        }
    }


    /// <summary>
    ////* 点击技能显示对应的技能详情
    /// </summary>
    /// <param name="skill_slot">技能显示槽</param>
    /// <param name="skill">技能SO文件</param>
    public void SelectSkill(GameObject skill_slot, Skill_SO skill)
    {
        foreach (var slot in equippedSkillsUI.GetComponentsInChildren<EquippedSkill_Slot>())
        {
            slot.GetComponent<Image>().enabled = false;
        }
        for (int i = 0; i < studySkillsUIs.Length; i++)
        {
            foreach (var slot in studySkillsUIs[i].GetComponentsInChildren<StudySkill_Slot>())
            {
                slot.GetComponent<Image>().enabled = false;
            }
        }
        skill_slot.GetComponent<Image>().enabled = true;
        ShowSkillDetails(skill);
    }

    //* 鼠标移动
    public void ShowSkillInfoTip(Skill_SO skill)
    {
        skillInfoTip.SetupSkillTip(skill);
    }

    //* 显示选中技能的详情
    private void ShowSkillDetails(Skill_SO skill)
    {
        showSkillDetailsUI.gameObject.SetActive(true);
        showSkillDetailsUI.skillCategory.sprite = skillHurtTypes.skillTypes.Find(o => o.skillType == skill.hurtCategory).skillSprite;
        showSkillDetailsUI.power.text = skill.power;
        showSkillDetailsUI.skillType.sprite = pokemonTypes.pokemonTypeList.Find(o => o.pokemonType == skill.type).typeImage;
        showSkillDetailsUI.skillType.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = skill.type.ToString();
        showSkillDetailsUI.skillDetails.text = skill.info;
    }

    //* 检查拖拽技能是否在Slot范围内
    public bool CheckInEquippedSkillUI(Vector3 position)
    {
        for (int i = 0; i < equippedSkillsUI.slots.Count; i++)
        {
            RectTransform rt = equippedSkillsUI.slots[i];
            if (RectTransformUtility.RectangleContainsScreenPoint(rt, position))
            {
                return true;
            }
        }
        return false;
    }
}
