using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestRequirement : MonoBehaviour
{
    public TextMeshProUGUI requireName;
    public TextMeshProUGUI progressNumber;

    public void SetupRequirement(QuestRequire questRequire)
    {
        switch (questRequire.requireType)
        {
            case QuestRequireType.收集道具:
                requireName.text = "◇ 收集" + questRequire.name;
                if (questRequire.currentAmout >= questRequire.requireAmout)
                    progressNumber.text = questRequire.currentAmout.ToString() + "/" + questRequire.requireAmout.ToString() + " <color=#499D00>✔";
                else
                    progressNumber.text = questRequire.currentAmout.ToString() + "/" + questRequire.requireAmout.ToString();
                break;

            case QuestRequireType.击败敌人:
                requireName.text = "◇ 击败" + questRequire.name;
                if (questRequire.currentAmout >= questRequire.requireAmout)
                    progressNumber.text = questRequire.currentAmout.ToString() + "/" + questRequire.requireAmout.ToString() + " <color=#499D00>✔";
                else
                    progressNumber.text = questRequire.currentAmout.ToString() + "/" + questRequire.requireAmout.ToString();
                break;

            case QuestRequireType.到达特定等级:
                requireName.text = "◇ 提升" + questRequire.name + "等级 ";
                if (questRequire.currentAmout >= questRequire.requireAmout)
                    progressNumber.text = questRequire.currentAmout.ToString() + "/" + questRequire.requireAmout.ToString() + " <color=#499D00>✔";
                else
                    progressNumber.text = questRequire.currentAmout.ToString() + "/" + questRequire.requireAmout.ToString();
                break;

            case QuestRequireType.找特定的人对话:
                requireName.text = "◇ 与" + questRequire.name + "对话";
                break;

            case QuestRequireType.到达目的地:
                requireName.text = "◇ 前往" + questRequire.name;
                break;

            default:
                break;
        }
    }
    public void SetupRequirement(QuestRequire questRequire, bool isFinished)
    {
        if (isFinished)
        {
            requireName.text = questRequire.name;
            progressNumber.text = "完成 <color=green>✔";
        }

    }
}
