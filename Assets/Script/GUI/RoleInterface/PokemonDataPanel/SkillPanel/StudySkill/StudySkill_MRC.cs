using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StudySkill_MRC : MonoBehaviour
{
    private RoleInterface roleInterface;

    public Button studyButton;
    public Button cancelButton;

    private RectTransform rectTransform;

    private void Awake()
    {
        roleInterface = FindObjectOfType<RoleInterface>();
        rectTransform = GetComponent<RectTransform>();
    }
    private void Start()
    {
        cancelButton.onClick.AddListener(delegate
        {
            gameObject.SetActive(false);
        });
    }

    public void ShowStudySkillMenu(StudySkill_Slot studySkill_Slot, Skill_SO skill)
    {
        UpdatePosition();

        studyButton.onClick.RemoveAllListeners();
        studyButton.onClick.AddListener(delegate
        {
            if (studySkill_Slot.isLearn)
            {
                if (roleInterface.curSelectPokemon.equippedSkills.skillDatabase.Count >= 4)
                {
                    Debug.Log("携带的技能已达到上限，请先遗忘一个技能");
                    DialogueUI.Instance.DIYDialog("携带的技能已达到上限，请先遗忘一个技能");
                }
                else if(roleInterface.curSelectPokemon.equippedSkills.skillDatabase.Find(i => i.id == skill.id))
                {
                    Debug.Log("本技能已经携带了");
                    DialogueUI.Instance.DIYDialog("本技能已经携带了");
                }
                else{
                    Debug.Log(skill.skillName + "添加");
                    DialogueUI.Instance.DIYDialog(skill.skillName + " 携带成功！");
                    roleInterface.curSelectPokemon.equippedSkills.skillDatabase.Add(skill);
                }
                
            }
            else
            {
                Debug.Log("还未解锁该技能。");
                DialogueUI.Instance.DIYDialog("还未解锁该技能。");
            }
            gameObject.SetActive(false);
        });
    }

    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        if (mousePos.y < height)
            rectTransform.position = mousePos + Vector3.up * height * 0.6f;
        else if (Screen.width - mousePos.x > width * 0.5f)
            rectTransform.position = mousePos + Vector3.right * width * 0.6f;
        else
            rectTransform.position = mousePos + Vector3.left * width * 0.6f;
    }

}
