using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StudySkillsUI : MonoBehaviour
{
    public List<StudySkill_Slot> studySkillList;

    private void OnEnable() {
        studySkillList.Clear();
        for (int i = 0; i < transform.GetComponentsInChildren<StudySkill_Slot>().Length; i++)
        {
            studySkillList.Add(transform.GetComponentsInChildren<StudySkill_Slot>()[i]);
        }
    }

    public void DestroyAllChild()
    {
        if (transform.childCount != 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
    }
}
