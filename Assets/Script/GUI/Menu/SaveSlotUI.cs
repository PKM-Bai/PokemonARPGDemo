using System.Collections;
using System.Collections.Generic;
using MGame.Save;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveSlotUI : MonoBehaviour
{
    public TextMeshProUGUI dataScene, dataTime;
    private Button currentButton;
    private DataSlot currentData;

    public int Index => transform.GetSiblingIndex();

    private void Awake()
    {
        currentButton = GetComponent<Button>();
        currentButton.onClick.AddListener(LoadGameData);
    }

    private void OnEnable()
    {
        SetupSlotUI();
    }

    private void SetupSlotUI()
    {
        currentData = SavaLoadManager.Instance.dataSlots[Index];

        if (currentData != null)
        {
            dataTime.gameObject.SetActive(true);
            dataTime.text = currentData.DataTime;
            dataScene.text = currentData.DataScene;
        }
        else
        {
            dataTime.gameObject.SetActive(false);
            dataScene.text = "新游戏";
        }
    }

    private void LoadGameData()
    {
        EventHandler.CallStartGameEvent(Index);
        if (currentData != null)
        {
            SavaLoadManager.Instance.Load(Index);
        }
        else
        {
            EventHandler.CallStartNewGameEvent(Index);
        }
    }
}
