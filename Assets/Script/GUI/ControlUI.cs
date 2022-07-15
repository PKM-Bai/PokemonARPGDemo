using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControlUI : Singleton<ControlUI>
{
    public GameObject bag;
    public GameObject roleInterface;
    public QuestUI questInterface;
    public GameObject settingInfo;
    public GameMenuUI gameMenu;

    public bool isOpenQuestInterface = false;
    public bool isControl;

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnControlUI;
        EventHandler.InGameMainMenuEvent += OnInGameMainMenuEvent;
        EventHandler.StartGameEvent += OnStartGameEvent;
    }

    private void OnDisable()
    {
        isOpenQuestInterface = false;
        EventHandler.BeforeSceneUnloadEvent -= OnControlUI;
        EventHandler.StartGameEvent -= OnStartGameEvent;
    }

    private void Start()
    {
        OnControlUI();
    }

    private void OnInGameMainMenuEvent()
    {
        isControl = false;
    }

    private void OnStartGameEvent(int index)
    {
        isControl = true;
        gameMenu.index = index;

        gameMenu.Init_GameUI();
        questInterface.Init();
    }

    private void OnControlUI()
    {
        questInterface.gameObject.SetActive(false);
        roleInterface.SetActive(false);
        gameMenu.gameObject.SetActive(false);
    }

    void Update()
    {
        if(!isControl) return;

        if (Input.GetButtonDown("Bag"))
        {
            bag.SetActive(!bag.activeSelf);

            roleInterface.SetActive(false);
            isOpenQuestInterface = false;
            questInterface.gameObject.SetActive(false);
            gameMenu.gameObject.SetActive(false);
        }
        if (Input.GetButtonDown("RoleInterface"))
        {
            roleInterface.SetActive(!roleInterface.activeSelf);

            bag.SetActive(false);
            isOpenQuestInterface = false;
            questInterface.gameObject.SetActive(false);
            gameMenu.gameObject.SetActive(false);
        }
        if (Input.GetButtonDown("Quest"))
        {
            isOpenQuestInterface = !isOpenQuestInterface;
            questInterface.gameObject.SetActive(!questInterface.gameObject.activeSelf);

            bag.SetActive(false);
            roleInterface.SetActive(false);
            gameMenu.gameObject.SetActive(false);
        }

        if (Input.GetButtonDown("Esc"))
        {
            if(bag.activeSelf || roleInterface.activeSelf || settingInfo.activeSelf || questInterface.gameObject.activeSelf)  
            {
                gameMenu.gameObject.SetActive(true);

                bag.SetActive(false);
                roleInterface.SetActive(false);
                settingInfo.SetActive(false);
                isOpenQuestInterface = false;
                questInterface.gameObject.SetActive(false);
            }
            else if(!gameMenu.gameObject.activeSelf)
                gameMenu.gameObject.SetActive(true);
            else
                gameMenu.gameObject.SetActive(false);
            
        }
        
    }

    
    


}
