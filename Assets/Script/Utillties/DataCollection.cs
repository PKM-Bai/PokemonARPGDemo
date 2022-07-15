using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
////*   道具
/// </summary>
[System.Serializable]
public class ItemDetails
{
    [FoldoutGroup("Item"), HorizontalGroup("Item/icon", 75, LabelWidth = 50), HideLabel, PreviewField(75)]
    public Sprite itemImage;
    [FoldoutGroup("Item"), VerticalGroup("Item/icon/ItemField"), LabelWidth(80)]
    public int itemID;
    [FoldoutGroup("Item"), VerticalGroup("Item/icon/ItemField"), LabelWidth(80)]
    public string itemName;
    [FoldoutGroup("Item"), VerticalGroup("Item/icon/ItemField"), LabelWidth(80)]
    public TabTypeEnum tabType;
    [FoldoutGroup("Item"), VerticalGroup("Item/icon/ItemField"), LabelWidth(80)]
    public ItemActionType itemType;

    [TextArea, FoldoutGroup("Item")] public string itemInfo;
    [FoldoutGroup("Item")] public int buyMoney;
    [FoldoutGroup("Item")] public int sellMoney;
    [FoldoutGroup("Item")] public int amount;

    //* 接收一个道具操作类型 判断该道具是否可以使用这个操作类型
    public bool isUseThisActionType(ItemActionType itemActionType)
    {
        
        return false;
    }

}


/// <summary>
////*   宝可梦
/// </summary>
[System.Serializable]
//* 宝可梦属性相克
public class PokemonTypeDetails
{
    public PokemonType pokemonType;
    [PreviewField] public Sprite typeImage;


    [TabGroup("作为攻击方"), LabelText("效果绝佳"), LabelWidth(50)] public List<PokemonType> atkAdvantageType;

    [TabGroup("作为攻击方"), LabelText("效果不好"), LabelWidth(50)] public List<PokemonType> atkInferiorityType;

    [TabGroup("作为攻击方"), LabelText("无效"), LabelWidth(50)] public List<PokemonType> atkInvalidType;


    [TabGroup("作为被攻击方"), LabelText("效果绝佳"), LabelWidth(50)] public List<PokemonType> hitAdvantageType;

    [TabGroup("作为被攻击方"), LabelText("效果不好"), LabelWidth(50)] public List<PokemonType> hitInferiorityType;

    [TabGroup("作为被攻击方"), LabelText("无效"), LabelWidth(50)] public List<PokemonType> hitInvalidType;
}

[System.Serializable]
//* 技能伤害类型
public class SkillHurtType
{
    public SkillType skillType;
    public Sprite skillSprite;
}

[System.Serializable]
//* 技能释放
public class ReleaseSkill
{
    public Skill_SO skill;
    public Animator animator;
    public GameObject thisObject;   // 自己
    public Transform releasePoint;     // 技能释放的位置
    public Vector3 skillArea;      // 释放技能范围
}

/// <summary>
////*   场景相关
/// </summary>
[System.Serializable]
public class SerializableVector3
{
    public float x, y, z;

    public SerializableVector3(Vector3 pos)
    {
        this.x = pos.x;
        this.y = pos.y;
        this.z = pos.z;
    }

    public Vector3 ToVector3()
    {
        return new Vector3(x, y, z);
    }

    public Vector2 ToVector2()
    {
        return new Vector2Int((int)x, (int)y);
    }
}

//* 保存的场景道具
[System.Serializable]
public class SceneItem
{
    public string[] itemNames;
    public SerializableVector3 position;
}

/// <summary>
////* 对话系统相关
/// </summary>
[System.Serializable]
public class DialoguePiece
{
    public string id;
    public Sprite image;
    [TextArea]
    public string text;
    [LabelText("对话中存在的提示信息")] public Information information = new Information();
    [LabelText("对话是否结束")] public bool isEnd;
    [LabelText("对话是否获得奖励")] public bool isGetReward;

    public QuestData_SO quest;
    public List<DialogueOption> options = new List<DialogueOption>();
}
//* 对话选项
[System.Serializable]
public class DialogueOption
{
    public string text;
    public string targetID;
    public bool takeQuest;      // 任务
    public bool isTreatment;    // 治疗
}
//* 对话中存在的小道消息（线索）
[System.Serializable]
public class Information
{
    public TownNameEnum townName;
    [TextArea]
    public string info;
}


//* 任务
[System.Serializable]
public class QuestTask
{
    public QuestData_SO questData;

    public bool IsStarted { get { return questData.isStarted; } set { questData.isStarted = value; } }
    public bool IsProgressed { get { return questData.isProgressed; } set { questData.isProgressed = value; } }
    public bool IsComplete { get { return questData.isComplete; } set { questData.isComplete = value; } }
    public bool IsFinished { get { return questData.isFinished; } set { questData.isFinished = value; } }
}
//* 任务要求|任务条件
[System.Serializable]
public class QuestRequire
{
    public QuestRequireType requireType;
    [HideIf("requireType", QuestRequireType.到达目的地)]
    public string name;
    public int requireAmout;
    public int currentAmout;
}
//* 任务奖励
[System.Serializable]
public class QuestReward
{
    public QuestRewardType rewardType;

    [LabelText("钱"), ShowIf("rewardType", QuestRewardType.零钱)] public int money;
    [LabelText("经验"), ShowIf("rewardType", QuestRewardType.经验)] public int exp;
    [ShowIf("rewardType", QuestRewardType.道具)] public RewardItemName item;
}
//* 奖励道具
[System.Serializable]
public class RewardItemName
{
    [LabelText("道具")] public string itemName;
    [LabelText("道具数量")] public int itemCount;
}
[System.Serializable]
public class RewardItem
{
    [LabelText("道具")] public ItemDetails item;
    [LabelText("道具数量")] public int itemCount;
}



