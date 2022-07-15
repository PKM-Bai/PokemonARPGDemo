using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MyPokemon.Inventory;

public class TaskNameButton : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI taskName;

    public QuestData_SO currentData;

    private void Awake()
    {
        GetComponent<Button>().onClick.AddListener(UpdateTaskContent);
    }

    public void UpdateTaskContent()
    {
        if (QuestManager.Instance.IsContainsQuest(currentData))
        {
           currentData = QuestManager.Instance.GetTask(currentData).questData;
        }
        TaskUI.Instance.SetupRequireList(currentData);
        TaskUI.Instance.SetupRewardList(currentData);
    }

    public void SetupTaskNameButton(QuestData_SO questData)
    {
        currentData = questData;
        taskName.text = questData.questName;
    }



}
