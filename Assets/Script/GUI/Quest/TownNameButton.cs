using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TownNameButton : MonoBehaviour
{
    public TextMeshProUGUI townName;
    public List<string> currentData;

    private void Awake() {
        GetComponent<Button>().onClick.AddListener(UpdateInformationContent);
    }

    public void UpdateInformationContent()
    {
        QuestUI.Instance.selectTownName = townName.text;
        QuestUI.Instance.selectInfoData = currentData;

        QuestUI.Instance.SetupInformationContent(currentData);
    }

    public void SetupTownNameButton(string name, List<string> data)
    {
        currentData = data;
        townName.text = name;
        // questName.text = currentData.
    }

    public void SelectButton(string selectTownName)
    {
        if(selectTownName == townName.text)
        {
            GetComponent<Button>().Select();
            UpdateInformationContent();
        }
    }

}
