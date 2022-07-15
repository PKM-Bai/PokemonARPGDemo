using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestNameButton : MonoBehaviour
{
    public Image questImg;
    public TextMeshProUGUI questName;
    public QuestData_SO currentData;


    private void Awake() {
        GetComponent<Button>().onClick.AddListener(UpdateQuestContent);
    }

    public void UpdateQuestContent()
    {
        QuestUI.Instance.selectQuestName = questName.text;
        QuestUI.Instance.selectQuestData = currentData;

        QuestUI.Instance.SetupRequireList(currentData);
        QuestUI.Instance.SetupRewardList(currentData);
    }

    public void SetupQuestNameButton(QuestData_SO questData)
    {
        currentData = questData;
        if(questData.isComplete)
            questName.text = currentData.questName + "(完成)";
        else if(questData.isFinished)
            questName.text = currentData.questName + "(结束)";
        else
            questName.text = currentData.questName;
    }

    public void SelectButton(string selectQuestName)
    {
        if(selectQuestName == questName.text)
        {
            GetComponent<Button>().Select();
            UpdateQuestContent();
        }
    }

}
