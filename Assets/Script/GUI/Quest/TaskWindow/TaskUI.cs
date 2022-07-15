using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TaskUI : Singleton<TaskUI>
{
    [Header("数据")]
    public QuestDataBase_SO taskDatabase;
    [Header("任务名界面、按钮及说明")]
    public RectTransform taskPanel;
    public RectTransform taskListTransform;
    public TaskNameButton taskNamePrefab;
    public TextMeshProUGUI taskNameText;
    public TextMeshProUGUI taskInfoText;

    public RectTransform taskDetalisPanel;

    [Header("任务目标")]
    public RectTransform requireTransform;
    public TaskRequire requirementPrefab;

    [Header("任务奖励")]
    public RectTransform rewardTransform;
    public TaskReward rewardPrefab;

    [Header("任务类型按钮")]
    public Button taskBarButton;
    public Button offerRewardButton;
    // public QuestType currentQuestPanelType;

    public TaskInteractButton interactButton;


    protected override void Awake() {
        base.Awake();
    }

    public void SetupTaskList()
    {
        //* 将原本面板中所有信息删除
        for (int i = 0; i < taskListTransform.childCount; i++) { Destroy(taskListTransform.GetChild(i).gameObject); }
        for (int i = 0; i < requireTransform.childCount; i++) { Destroy(requireTransform.GetChild(i).gameObject); }
        for (int i = 0; i < rewardTransform.childCount; i++) { Destroy(rewardTransform.GetChild(i).gameObject); }

        //* 获取任务管理中的任务并生成
        for (int i = 0; i < taskDatabase.tasks.Count; i++)
        {
            
            var task = Instantiate(taskNamePrefab, taskListTransform);
            task.SetupTaskNameButton(taskDatabase.tasks[i]);
            if(i == 0)
                task.GetComponent<Button>().Select();
        }
        
        if(taskDatabase.tasks.Count > 0)
        {
            SetupRequireList(taskDatabase.tasks[0]);
            SetupRewardList(taskDatabase.tasks[0]);
        }
    }

    /// <summary>
    ////* 当点击任务名时，显示这个任务的详情信息
    /// </summary>
    /// <param name="questData"></param>
    public void SetupRequireList(QuestData_SO questData)
    {
        taskInfoText.text = questData.description;
        taskNameText.text = questData.questName;
        SetupInteractButton(questData);

        for (int i = 0; i < requireTransform.childCount; i++) { Destroy(requireTransform.GetChild(i).gameObject); }
        if (questData.questRequires != null)
        {
            taskDetalisPanel.gameObject.SetActive(true);
            requireTransform.gameObject.SetActive(true);
        }

        for (int i = 0; i < questData.questRequires.Count; i++)
        {
            var require = Instantiate(requirementPrefab, requireTransform);
            if (questData.isFinished)
                require.SetupRequirement(questData.questRequires[i], true);
            else
                require.SetupRequirement(questData.questRequires[i]);
        }
        
    }

    public void SetupRewardList(QuestData_SO questData)
    {
        for (int i = 0; i < rewardTransform.childCount; i++) { Destroy(rewardTransform.GetChild(i).gameObject); }

        if (questData.questRewards != null)
            rewardTransform.gameObject.SetActive(true);

        for (int i = 0; i < questData.questRewards.Count; i++)
        {
            if (questData.questRewards[i].rewardType == QuestRewardType.道具 && questData.questRewards[i].item.itemCount < 0)
                continue;
            var reward = Instantiate(rewardPrefab, rewardTransform);
            reward.SetupQuestReward(questData.questRewards[i], questData);
        }
    }

    public void SetupInteractButton(QuestData_SO questData)
    {
        interactButton.currentQuestdata = questData;
        interactButton.SetupInteractButton();
    }



}
