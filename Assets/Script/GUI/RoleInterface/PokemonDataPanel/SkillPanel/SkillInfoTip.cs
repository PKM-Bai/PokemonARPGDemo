using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SkillInfoTip : MonoBehaviour
{
    public TextMeshProUGUI hurtCategorty;
    public TextMeshProUGUI moveType;
    public TextMeshProUGUI addedEffect;
    public TextMeshProUGUI PP;
    public TextMeshProUGUI reachCondition;

    private RectTransform rectTransform;

    private void OnEnable()
    {
        UpdatePosition();
    }

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void SetupSkillTip(Skill_SO skill)
    {
        hurtCategorty.text = "<b>招式分类：</b>" + skill.hurtCategory.ToString();
        var moveTypes = "";
        for (int i = 0; i < skill.moveType.Count; i++)
        {
            moveTypes += skill.moveType[i].ToString();
        }
        moveType.text = moveTypes == "" ? "<b>技能类型：</b>无" : "技能类型：" + moveTypes;
        addedEffect.text = skill.addedEffect != null ? "<b>追加效果：</b>" + skill.addedEffect.name : "<b>追加效果：</b>无";
        PP.text = "<b>PP：</b>" + skill.maxPP;
        reachCondition.text = "<b>解锁条件：</b>达到" + skill.reachLevel + "级";
    }

    private void Update()
    {
        UpdatePosition();
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
        else if (Screen.width - mousePos.x > width * 1.1f)
            rectTransform.position = mousePos + Vector3.right * width * 0.1f;
        else
            rectTransform.position = mousePos + Vector3.left * width * 1.1f;
    }

}
