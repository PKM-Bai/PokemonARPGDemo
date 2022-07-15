using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class EquippedSkill_Slot : MonoBehaviour, IPointerClickHandler
{
    private SkillPanel skillPanel;
    public Skill_SO skill;
    public Image type;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI power;
    public TextMeshProUGUI PP;

    public bool isDrag;


    private void Awake() {
        skillPanel = FindObjectOfType<SkillPanel>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillPanel.SelectSkill(eventData.pointerClick, skill);
        skillPanel.equippedSkill_MRC.gameObject.SetActive(false);

        if(eventData.button == PointerEventData.InputButton.Right)
        {
            skillPanel.equippedSkill_MRC.gameObject.SetActive(true);
            skillPanel.equippedSkill_MRC.ShowEquippedSkillMenu(eventData.pointerClick.GetComponent<EquippedSkill_Slot>(), skill);
        }


    }


}
