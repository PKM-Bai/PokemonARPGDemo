using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using MyPokemon.Inventory;

[CreateAssetMenu(fileName = "QuestData_SO", menuName = "Quest/QuestData_SO")]
public class QuestData_SO : ScriptableObject
{
    [LabelText("任务编号"), FoldoutGroup("基础信息")] public string questId;
    [LabelText("任务类型"), FoldoutGroup("基础信息")] public QuestType questType;
    [LabelText("任务名称"), FoldoutGroup("基础信息")] public string questName;
    [LabelText("任务描述"), TextArea, FoldoutGroup("基础信息")] public string description;

    [LabelText("接取任务"), FoldoutGroup("任务状态")] public bool isStarted;
    [LabelText("任务进行中"), FoldoutGroup("任务状态")] public bool isProgressed;
    [LabelText("任务完成"), FoldoutGroup("任务状态")] public bool isComplete;
    [LabelText("任务结束"), FoldoutGroup("任务状态")] public bool isFinished;

    [LabelText("接取"), FoldoutGroup("状态对话")] public DialogueData_SO startDialogue;
    [LabelText("进行中"), FoldoutGroup("状态对话")] public DialogueData_SO progressDialogue;
    [LabelText("完成"), FoldoutGroup("状态对话")] public DialogueData_SO completeDialogue;
    [LabelText("结束"), FoldoutGroup("状态对话")] public DialogueData_SO finishDialogue;

    [LabelText("任务目标")] public List<QuestRequire> questRequires = new List<QuestRequire>();
    [LabelText("任务奖励")] public List<QuestReward> questRewards = new List<QuestReward>();


    //* 检测任务完成进度
    public void CheckQuestProgress()
    {
        //* 当前任务达成目标 完成的数量，如果完成数量等于这个任务条件的数量就代表完成了这个任务
        var finishRequires = questRequires.Where(r => r.requireAmout <= r.currentAmout);
        isComplete = finishRequires.Count() == questRequires.Count;
    }

    //* 获得任务奖励
    public void GiveReward()
    {
        foreach (var reward in questRewards)
        {
            switch (reward.rewardType)
            {
                case QuestRewardType.零钱:
                    InventoryManager.Instance.playerBag.money += reward.money;
                    break;
                case QuestRewardType.经验:
                    foreach (var pokemon in PokemonManager.Instance.pokemonTeam.pokemons)
                    {
                        PokemonManager.Instance.OnGetExp(pokemon, reward.exp);
                    }
                    break;
                case QuestRewardType.道具:
                    RewardItem rewardItem = GetRewardItem().Find(i => i.item.itemName == reward.item.itemName);
                    if (rewardItem != null)
                        InventoryManager.Instance.AddNewItem(rewardItem.item, rewardItem.itemCount);
                    break;
                default:

                    break;
            }
        }
    }

    //* 道具奖励名获得道具和数量
    public List<RewardItem> GetRewardItem()
    {
        List<RewardItem> rewardItems = new List<RewardItem>();

        foreach (var reward in questRewards)
        {
            if (InventoryManager.Instance.WithItemNameGetItemList(reward.item.itemName) != null)
            {
                RewardItem newItem = new RewardItem
                {
                    item = InventoryManager.Instance.WithItemNameGetItemList(reward.item.itemName),
                    itemCount = reward.item.itemCount
                };
                rewardItems.Add(newItem);
            }
        }
        return rewardItems;
    }

    //* 任务目标归零
    public void QuestSO_Init()
    {
        isStarted = false;
        isProgressed = false;
        isComplete = false;
        isFinished = false;
        
        for (int i = 0; i < questRequires.Count; i++)
        {
            questRequires[i].currentAmout = 0;
        }
    }

}


public class TaskProgress
{
    public string taskID;

    public bool isStarted;
    public bool isProgressed;
    public bool isComplete;
    public bool isFinished;

    public List<string> requiresName = new List<string>(8);
    public List<int> requiresAmout = new List<int>(8);

    public TaskProgress(QuestData_SO task)
    {
        if (task != null)
        {
            taskID = task.questId;
            isStarted = task.isStarted;
            isProgressed = task.isProgressed;
            isComplete = task.isComplete;
            isFinished = task.isFinished;

            foreach (QuestRequire require in task.questRequires)
            {
                requiresName.Add(require.name);
                requiresAmout.Add(require.currentAmout);
            }
        }

    }
}
