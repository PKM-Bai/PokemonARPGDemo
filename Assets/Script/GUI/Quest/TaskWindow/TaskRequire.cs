using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskRequire : MonoBehaviour
{
    public TextMeshProUGUI requireText;

    private void Awake() {
        requireText = GetComponent<TextMeshProUGUI>();
    }

    public void SetupRequirement(QuestRequire questRequire)
    {
       switch (questRequire.requireType) {
           case QuestRequireType.收集道具:
               requireText.text = "◇ 收集 " + questRequire.name + " " + questRequire.currentAmout.ToString() + "/" + questRequire.requireAmout.ToString();
               break;
           case QuestRequireType.击败敌人:
               requireText.text = "◇ 击败" + questRequire.name + " " + questRequire.currentAmout.ToString() + "/" + questRequire.requireAmout.ToString();
               break;
           case QuestRequireType.到达特定等级:
               requireText.text = "◇ 提升" + questRequire.name + "等级 " + questRequire.currentAmout.ToString() + "/" + questRequire.requireAmout.ToString();
               break;
           case QuestRequireType.找特定的人对话:
               requireText.text = "◇ 与 " + questRequire.name + " 对话";
               break;
           case QuestRequireType.到达目的地:
               requireText.text = "◇ 前往 " + questRequire.name;
               break;
           default :
               break;
       }
        
    }

    public void SetupRequirement(QuestRequire questRequire, bool isFinished)
    {
        if (isFinished)
        {
            requireText.text = "◇ " + questRequire.name + questRequire.currentAmout.ToString() + "/" + questRequire.requireAmout.ToString() + "  <color=#499D00>√";
        }
    }
}
