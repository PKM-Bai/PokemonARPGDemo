using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestUI : Singleton<QuestUI>
{
    [Header("任务名界面、按钮及说明")]
    public RectTransform questPanel;
    public RectTransform questListTransform;
    public QuestNameButton questNamePrefab;
    public TextMeshProUGUI questInfoText;

    [Header("任务目标")]
    public RectTransform requireTitleTransform;
    public RectTransform requireTransform;
    public QuestRequirement requirementPrefab;

    [Header("任务奖励")]
    public RectTransform rewardTransform;
    public QuestRewarding rewardPrefab;

    [Header("任务类型按钮")]
    public Button mainPlotButton;
    public Button branchPlotButton;
    public Button informationButton;
    public QuestType currentQuestPanelType;

    [Header("选择的按钮和任务数据（第二次打开面板显示的数据）")]
    public string selectQuestName;
    public QuestData_SO selectQuestData;

    [Header("小道消息界面、按钮")]
    public RectTransform infoPanel;
    public TownNameButton townNamePrefab;
    public RectTransform informationList;
    public TextMeshProUGUI infoSlotPrefab;


    [Header("选择的按钮和消息数据（第二次打开面板显示的数据）")]
    public string selectTownName;
    public List<string> selectInfoData;


    protected override void Awake()
    {
        base.Awake();
        questInfoText.text = string.Empty;
        requireTitleTransform.gameObject.SetActive(false);
    }

    private void Start()
    {
        currentQuestPanelType = QuestType.主线;
        mainPlotButton.onClick.AddListener(OnMainPlotButton);
        branchPlotButton.onClick.AddListener(OnBranchPlotButton);
        informationButton.onClick.AddListener(OnInformationButton);
    }

    private void OnEnable()
    {
        //* 类型按钮选中效果
        switch (currentQuestPanelType)
        {
            case QuestType.主线:
                mainPlotButton.Select();
                break;
            case QuestType.支线:
                branchPlotButton.Select();
                break;
            case QuestType.小道消息:
                informationButton.Select();
                break;
            default:

                break;
        }

        //* 显示面板内容
        if (currentQuestPanelType != QuestType.小道消息)
            SetupQuestList();
        else
            SetupInformationList();
    }

    public void Init()
    {
        OnMainPlotButton();
    }


    public void OnMainPlotButton()
    {
        //* 切换标签移除所有之前所选的数据
        selectQuestName = string.Empty;
        selectQuestData = null;

        selectTownName = string.Empty;
        selectInfoData = null;
        currentQuestPanelType = QuestType.主线;
        SwitchQuestPanelByType(QuestType.主线);
    }
    public void OnBranchPlotButton()
    {
        //* 切换标签移除所有之前所选的数据
        selectQuestName = string.Empty;
        selectQuestData = null;

        selectTownName = string.Empty;
        selectInfoData = null;

        currentQuestPanelType = QuestType.支线;
        SwitchQuestPanelByType(QuestType.支线);
    }
    public void OnInformationButton()
    {
        //* 切换标签移除所有之前所选的数据
        selectQuestName = string.Empty;
        selectQuestData = null;

        selectTownName = string.Empty;
        selectInfoData = null;

        currentQuestPanelType = QuestType.小道消息;
        SwitchQuestPanelByType(QuestType.小道消息);
    }

    #region 任务列表和任务信息
    public void SwitchQuestPanelByType(QuestType questType)
    {
        if (questType != QuestType.小道消息)
        {
            questPanel.gameObject.SetActive(true);
            infoPanel.gameObject.SetActive(false);

            currentQuestPanelType = questType;
            SetupQuestList();
            questInfoText.text = string.Empty;
            requireTitleTransform.gameObject.SetActive(false);
            requireTransform.gameObject.SetActive(false);
            rewardTransform.gameObject.SetActive(false);
        }
        else
        {
            infoPanel.gameObject.SetActive(true);
            questPanel.gameObject.SetActive(false);

            informationList.gameObject.SetActive(false);

            SetupInformationList();
        }
    }

    public void SetupQuestList()
    {
        //* 将原本面板中所有信息删除
        for (int i = 0; i < questListTransform.childCount; i++) { Destroy(questListTransform.GetChild(i).gameObject); }
        for (int i = 0; i < requireTransform.childCount; i++) { Destroy(requireTransform.GetChild(i).gameObject); }
        for (int i = 0; i < rewardTransform.childCount; i++) { Destroy(rewardTransform.GetChild(i).gameObject); }

        //* 获取任务管理中的任务并生成
        if (QuestManager.Instance)
        {
            for (int i = 0; i < QuestManager.Instance.tasks.Count; i++)
            {
                if (QuestManager.Instance.tasks[i].questData.questType == currentQuestPanelType)
                {
                    var task = Instantiate(questNamePrefab, questListTransform);
                    task.SetupQuestNameButton(QuestManager.Instance.tasks[i].questData);
                    if (selectQuestName != string.Empty)
                        task.SelectButton(selectQuestName);
                }
            }
        }

        //* 在退出任务界面，再次进入的时候显示上次退出前的任务详情
        if (selectQuestData != null)
        {
            SetupRequireList(selectQuestData);
            SetupRewardList(selectQuestData);
        }
    }

    /// <summary>
    ////* 当点击任务名时，显示这个任务的详情信息
    /// </summary>
    /// <param name="questData"></param>
    public void SetupRequireList(QuestData_SO questData)
    {
        questInfoText.text = questData.description;

        for (int i = 0; i < requireTransform.childCount; i++) { Destroy(requireTransform.GetChild(i).gameObject); }
        if (questData.questRequires != null)
        {
            requireTitleTransform.gameObject.SetActive(true);
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
    #endregion

    #region 小道消息列表和信息数据
    public void SetupInformationList()
    {
        for (int i = 0; i < questListTransform.childCount; i++) { Destroy(questListTransform.GetChild(i).gameObject); }

        //* 获取InformationData_SO中所有信息并筛选townType生成
        if (QuestManager.Instance.informationData != null)
        {
            if (QuestManager.Instance.informationData.GetTownNameTypeList().Count > 0)
            {
                foreach (var townNameType in QuestManager.Instance.informationData.GetTownNameTypeList())
                {
                    var info = Instantiate(townNamePrefab, questListTransform);
                    info.SetupTownNameButton(townNameType.ToString(), QuestManager.Instance.informationData.GetInformationByTownType(townNameType));
                    if(selectTownName != string.Empty)
                        info.SelectButton(selectTownName);
                    else
                        info.SelectButton(QuestManager.Instance.informationData.informations[0].townName.ToString());
                }
            }
        }


        //* 在退出任务界面，再次进入的时候显示上次退出前的信息详情
        if (selectInfoData != null)
        {
            SetupInformationContent(selectInfoData);
        }

    }

    public void SetupInformationContent(List<string> infoList)
    {
        for (int i = 0; i < informationList.childCount; i++) { Destroy(informationList.GetChild(i).gameObject); }

        informationList.gameObject.SetActive(true);
        for (int i = 0; i < infoList.Count; i++)
        {
            var info = Instantiate(infoSlotPrefab, informationList);
            info.text = "✍  " + infoList[i];
        }
    }
    #endregion

}
