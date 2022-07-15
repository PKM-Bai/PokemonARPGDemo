using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskReward : MonoBehaviour
{
    public TextMeshProUGUI rewardInfo;

    private void Awake() {
        rewardInfo = GetComponent<TextMeshProUGUI>();
    }

    public void SetupQuestReward(QuestReward questReward, QuestData_SO questData)
    {
        switch (questReward.rewardType)
        {
            case QuestRewardType.零钱:
                rewardInfo.text = "◆ " + questReward.money + " 零钱";
                break;
            case QuestRewardType.经验:
                rewardInfo.text = "◆ " + questReward.exp + " 经验";
                break;
            case QuestRewardType.道具:
                rewardInfo.text = "◆ " + questReward.item.itemName + " x" + questReward.item.itemCount;
                break;
            default:
                rewardInfo.text = "没有奖励";
                break;
        }
    }
}
