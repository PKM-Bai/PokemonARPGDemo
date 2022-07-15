using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DialogueController))]
public class QuestGiver : MonoBehaviour
{
    DialogueController controller;
    QuestData_SO currentQuest;

    public DialogueData_SO startDialogue;
    public DialogueData_SO progressDialogue;
    public DialogueData_SO completeDialogue;
    public DialogueData_SO finishDialogue;

    public bool IsStarted
    {
        get
        {
            if (QuestManager.Instance.IsContainsQuest(currentQuest))
                return QuestManager.Instance.GetTask(currentQuest).IsStarted;
            else return false;

        }
    }
    public bool IsComplete
    {
        get
        {
            if (QuestManager.Instance.IsContainsQuest(currentQuest))
                return QuestManager.Instance.GetTask(currentQuest).IsComplete;
            else return false;

        }
    }
    public bool IsFinished
    {
        get
        {
            if (QuestManager.Instance.IsContainsQuest(currentQuest))
                return QuestManager.Instance.GetTask(currentQuest).IsFinished;
            else return false;

        }
    }

    private void Awake()
    {
        controller = GetComponent<DialogueController>();
    }

    private void Start()
    {
        // controller.currentData = startDialogue;
        currentQuest = controller.currentData.GetQuestData();
        if (currentQuest != null)
        {
            startDialogue = currentQuest.startDialogue;
            progressDialogue = currentQuest.progressDialogue;
            completeDialogue = currentQuest.completeDialogue;
            finishDialogue = currentQuest.finishDialogue;
        }

    }

    private void Update()
    {
        if (IsStarted)
        {
            if (IsComplete)
                controller.currentData = completeDialogue;
            else if (IsFinished)
                controller.currentData = finishDialogue;
            else
                controller.currentData = progressDialogue;
        }
        else
        {
            controller.currentData = startDialogue;
        }

    }

}
