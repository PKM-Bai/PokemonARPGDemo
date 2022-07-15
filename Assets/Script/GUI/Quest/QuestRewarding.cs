using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MyPokemon.Inventory;

public class QuestRewarding : MonoBehaviour
{
    public TextMeshProUGUI rewardInfo;

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


    // public void SetupRewardItem(QuestReward questReward, QuestData_SO questData)
    // {
    //     rewardInfo.text = "道具：" + questReward.itemName + " x" + questReward.itemCount;
    // }
}
