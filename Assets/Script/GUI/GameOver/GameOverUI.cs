using System.Collections;
using System.Collections.Generic;
using MGame.Save;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour
{
    public Button loadSaveButton, backStartMenuButton;

    public int index;

    private void Awake()
    {
        loadSaveButton.onClick.AddListener(LoadSaveData);
        backStartMenuButton.onClick.AddListener(BackStartMenu);
    }

    private void OnEnable()
    {
        EventHandler.StartGameEvent += OnStartGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.StartGameEvent -= OnStartGameEvent;
    }

    private void OnStartGameEvent(int index)
    {
        this.index = index;
    }

    public void LoadSaveData()
    {
        SavaLoadManager.Instance.Load(index);
    }

    public void BackStartMenu()
    {
        EventHandler.CallLoadStartMenuSceneEvent("01.StartMenuUI");
    }


}
