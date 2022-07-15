using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class StudySkill_Slot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private SkillPanel skillPanel;
    
    public Skill_SO skill;
    public Image skillIcon;
    public TextMeshProUGUI skillName;

    public bool isLearn;

    private void Awake() {
        skillPanel = FindObjectOfType<SkillPanel>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        skillPanel.SelectSkill(eventData.pointerClick, skill);
        skillPanel.studySkill_MRC.gameObject.SetActive(false);
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            skillPanel.studySkill_MRC.gameObject.SetActive(true);
            skillPanel.studySkill_MRC.ShowStudySkillMenu(eventData.pointerClick.GetComponent<StudySkill_Slot>(), skill);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {  
        skillPanel.skillInfoTip.gameObject.SetActive(true);
        skillPanel.ShowSkillInfoTip(skill);
    }

    public void OnPointerExit(PointerEventData eventData)
    {  
        skillPanel.skillInfoTip.gameObject.SetActive(false);
    }


}
