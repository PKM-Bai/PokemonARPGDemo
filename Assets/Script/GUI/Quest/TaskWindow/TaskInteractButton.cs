using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyPokemon.Inventory;

public class TaskInteractButton : MonoBehaviour
{
    public Button interactButton;
    public TextMeshProUGUI text;

    public QuestData_SO currentQuestdata;

    private void Awake()
    {
        interactButton.onClick.AddListener(OnInteractButton);
    }

    public void OnInteractButton()
    {
        if (QuestManager.Instance.IsContainsQuest(currentQuestdata))
        {
            if (QuestManager.Instance.GetTask(currentQuestdata).IsComplete)
            {
                QuestManager.Instance.GetTask(currentQuestdata).questData.GiveReward();
                QuestManager.Instance.GetTask(currentQuestdata).IsFinished = true;

                DialogueUI.Instance.GetReward(QuestManager.Instance.GetTask(currentQuestdata).questData.questRewards);

                interactButton.interactable = false;
                interactButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                text.text = "已完成";
                text.fontSize = 36;
                text.color = Color.red;
            }
        }
        else
        {
            var newTask = new QuestTask { questData = Instantiate(currentQuestdata) };
            //? 添加到任务列表中
            QuestManager.Instance.tasks.Add(newTask);
            QuestManager.Instance.GetTask(newTask.questData).IsStarted = true;
            QuestManager.Instance.UpdateQuestProgress(QuestManager.Instance.GetTask(newTask.questData));

            if (QuestManager.Instance.GetTask(newTask.questData).IsComplete)
                text.text = "领取奖励";
            else
                text.text = "进行中";

            TaskUI.Instance.SetupRequireList(QuestManager.Instance.GetTask(newTask.questData).questData);
            TaskUI.Instance.SetupRewardList(QuestManager.Instance.GetTask(newTask.questData).questData);
        }
    }

    public void SetupInteractButton()
    {
        if (QuestManager.Instance)
        {
            if (QuestManager.Instance.IsContainsQuest(currentQuestdata))
            {
                QuestManager.Instance.GetTask(currentQuestdata).questData.CheckQuestProgress();
                if (QuestManager.Instance.GetTask(currentQuestdata).IsComplete)
                {
                    if (QuestManager.Instance.GetTask(currentQuestdata).IsFinished)
                    {
                        text.text = "已完成";
                        interactButton.interactable = false;
                        interactButton.GetComponent<Image>().color = new Color(1, 1, 1, 0);
                        text.fontSize = 36;
                        text.color = Color.red;
                    }
                    else
                    {
                        text.text = "领取奖励";
                        interactButton.interactable = true;
                        interactButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                        text.fontSize = 24;
                        text.color = new Color(0.2f, 0.2f, 0.2f, 255);
                    }
                }
                else
                {
                    text.text = "进行中";
                    interactButton.interactable = true;
                    interactButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                    text.fontSize = 24;
                    text.color = new Color(0.2f, 0.2f, 0.2f, 255);
                }
            }
            else
            {
                text.text = "接取任务";
                interactButton.interactable = true;
                interactButton.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                text.fontSize = 24;
                text.color = new Color(0.2f, 0.2f, 0.2f, 255);
            }


        }
    }

}
