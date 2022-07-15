using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using MyPokemon.Inventory;

public class DialogueUI : Singleton<DialogueUI>
{
    [Header("Basic Elements")]
    public TextMeshProUGUI content;
    public Image image;
    public Button nextButton;
    public GameObject dialoguePanel;

    [Header("Option")]
    public RectTransform optionPanel;
    public OptionUI optionPrefab;

    [Header("Data")]
    public DialogueData_SO editorDialogueData;
    public DialogueData_SO currentData;
    public QuestData_SO questData;
    int currentIndex = 0;

    [Header("控制对话时间")]
    public float m_time = 0.7f;
    public float default_time = 0.7f;
    public float timer = 0;

    public bool intervalPress = true;

    protected override void Awake()
    {
        base.Awake();
        nextButton.onClick.AddListener(ContinueDialogue);
    }

    private void Update()
    {
        StartTimer();

        if (dialoguePanel.activeSelf)
        {
            if (intervalPress)
            {
                if (Input.GetButtonDown("Interactive"))
                {
                    ContinueDialogue();
                    EventHandler.CallInitSoundEffect(AudioManager.Instance.soundDetailsList.GetSoundDetails(SoundName.CoutinueDialogue));
                    intervalPress = false;
                    timer = m_time;
                }
            }
        }
    }

    public void StartTimer()
    {
        if (timer != 0)
        {
            intervalPress = false;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                timer = 0;
                default_time = 0.7f;
                intervalPress = true;
            }
        }
    }

    public void ContinueDialogue()
    {
        //! 判断任务状态
        if (currentIndex - 1 >= 0 && currentData.dialoguePieces[currentIndex - 1].quest != null)
        {
            if (QuestManager.Instance.IsContainsQuest(currentData.dialoguePieces[currentIndex - 1].quest))
            {
                var quest = QuestManager.Instance.GetTask(currentData.dialoguePieces[currentIndex - 1].quest);
                //* 完成任务的对话，将任务状态改为“结束”，有奖励将给予奖励
                if (quest.IsComplete && currentData.dialoguePieces[currentIndex - 1].isGetReward)
                {
                    //* 获取奖励
                    if (currentData.dialoguePieces[currentIndex - 1].quest.questRewards != null)
                    {
                        currentData.dialoguePieces[currentIndex - 1].quest.GiveReward();
                        GetReward(currentData.dialoguePieces[currentIndex - 1].quest.questRewards);
                    }
                    quest.IsFinished = true;
                }
            }
        }
        //! 获得对话中存在的小道消息
        if (currentIndex - 1 >= 0 && currentData.dialoguePieces[currentIndex - 1].information.townName != TownNameEnum.None)
        {
            Information information = new Information
            {
                townName = currentData.dialoguePieces[currentIndex - 1].information.townName,
                info = currentData.dialoguePieces[currentIndex - 1].information.info
            };
            //* 如果是新的消息，添加到列表中
            if (QuestManager.Instance)
            {
                if (!QuestManager.Instance.informationData.IsContainsInfo(information))
                    QuestManager.Instance.informationData.informations.Add(information);
            }
        }

        if (currentIndex < currentData.dialoguePieces.Count)
        {
            if (currentIndex - 1 >= 0 && currentData.dialoguePieces[currentIndex - 1].isEnd)
                dialoguePanel.SetActive(false);
            else
                UpdateDialogueUI(currentData.dialoguePieces[currentIndex]);
        }
        else
            dialoguePanel.SetActive(false);
    }

    public void UpdateDialogueData(DialogueData_SO data)
    {
        currentData = data;
        currentIndex = 0;
    }

    public void UpdateDialogueUI(DialoguePiece piece)
    {
        dialoguePanel.SetActive(true);

        currentIndex = currentData.dialoguePieces.IndexOf(piece);
        if (piece.image)
        {
            image.enabled = true;
            image.sprite = piece.image;
        }
        else image.enabled = false;

        content.text = "";
        // content.text = piece.text;
        content.DOText(piece.text, 0.6f);


        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            nextButton.gameObject.SetActive(true);
            currentIndex++;
        }
        else
            nextButton.gameObject.SetActive(false);
        //* 创建选项
        CreateOptions(piece);
    }

    public void UpdateDialogueUI(DialoguePiece piece, float d_timer)
    {
        m_time = d_timer;
        currentIndex = currentData.dialoguePieces.IndexOf(piece);
        dialoguePanel.SetActive(true);
        if (piece.image)
        {
            image.enabled = true;
            image.sprite = piece.image;
        }
        else image.enabled = false;

        content.text = "";
        // content.text = piece.text;
        content.DOText(piece.text, 0.6f);


        if (piece.options.Count == 0 && currentData.dialoguePieces.Count > 0)
        {
            nextButton.gameObject.SetActive(true);
            currentIndex++;
        }
        else
            nextButton.gameObject.SetActive(false);
        //* 创建选项
        CreateOptions(piece);
    }



    public void CreateOptions(DialoguePiece piece)
    {
        if (piece.options.Count > 0)
        {
            // 销毁
            for (int i = 0; i < optionPanel.childCount; i++)
            {
                Destroy(optionPanel.GetChild(i).gameObject);
            }
            optionPanel.gameObject.SetActive(true);
            // 创建
            for (int i = 0; i < piece.options.Count; i++)
            {
                var option = Instantiate(optionPrefab, optionPanel.transform);
                option.UpdateOption(piece, piece.options[i]);
            }
        }
        else optionPanel.gameObject.SetActive(false);
    }

    public void DIYDialog(string text)
    {
        editorDialogueData.dialoguePieces.Clear();
        DialoguePiece piece = new DialoguePiece();
        piece.text = text;
        editorDialogueData.dialoguePieces.Add(piece);
        timer = 1f;
        UpdateDialogueData(editorDialogueData);
        UpdateDialogueUI(editorDialogueData.dialoguePieces[0]);
    }

    //* 获得奖励
    public void GetReward(List<QuestReward> questRewards)
    {
        editorDialogueData.dialoguePieces.Clear();
        foreach (var reward in questRewards)
        {

            DialoguePiece piece = new DialoguePiece();
            switch (reward.rewardType)
            {
                case QuestRewardType.零钱:
                    piece.text = "获得了" + reward.money + "零钱！";
                    editorDialogueData.dialoguePieces.Add(piece);
                    break;
                case QuestRewardType.经验:
                    piece.text = "队伍中获得了" + reward.exp + "经验！";
                    editorDialogueData.dialoguePieces.Add(piece);
                    break;
                case QuestRewardType.道具:
                    if (reward.item.itemCount > 0)
                    {
                        var item = InventoryManager.Instance.itemDataList.itemList.Find(i => i.itemName == reward.item.itemName);
                        piece.text = "得到了" + item.itemName + "！把它放到了<color=blue>" + item.tabType.ToString() + "</color>中。";
                        editorDialogueData.dialoguePieces.Add(piece);
                    }
                    break;
                default:
                    break;
            }
        }
        AudioManager.Instance.getItem.Play();
        timer = 2f;
        UpdateDialogueData(editorDialogueData);
        UpdateDialogueUI(editorDialogueData.dialoguePieces[0]);
    }

    //* 拾取道具
    public void PickUpItem(ItemDetails item)
    {
        editorDialogueData.dialoguePieces.Clear();
        DialoguePiece piece = new DialoguePiece();
        piece.text = "得到了" + item.itemName + "！把它放到了<color=blue>" + item.tabType.ToString() + "</color>中。";
        editorDialogueData.dialoguePieces.Add(piece);

        AudioManager.Instance.getItem.Play();
        timer = 2f;
        UpdateDialogueData(editorDialogueData);
        UpdateDialogueUI(editorDialogueData.dialoguePieces[0]);
    }

}
