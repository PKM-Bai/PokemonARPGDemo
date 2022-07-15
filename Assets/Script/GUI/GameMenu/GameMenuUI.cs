using System.Collections;
using System.Collections.Generic;
using MGame.Save;
using MyPokemon.Transition;
using MyPokemon.Inventory;
using UnityEngine;
using UnityEngine.UI;

public class GameMenuUI : MonoBehaviour
{
    [Header("按钮")]
    public Button pokemonUIButton;
    public Button bagUIButton;
    // public Button settingUIButton;
    public Button saveDataButton;
    public Button backStartMenuButton;
    public Button backTown;
    [Header("界面")]
    public GameObject bag;
    public GameObject roleInterface;
    // public GameObject settingInterface;
    public GameObject settingInfo;

    public bool isOpenBag = false;
    public bool isOpenSetting = false;
    public bool isOpenRoleInterface = false;

    public int index;

    private void Awake()
    {
        pokemonUIButton.onClick.AddListener(OnPokemonUIButton);
        bagUIButton.onClick.AddListener(OnBagUIButton);
        // settingUIButton.onClick.AddListener(OnSettingUIButton);
        saveDataButton.onClick.AddListener(OnSaveData);
        backStartMenuButton.onClick.AddListener(OnBackStartMenu);
        backTown.onClick.AddListener(OnBackTown);
    }

    private void OnEnable()
    {
        // isOpenBag = false;
        // isOpenSetting = false;
        // isOpenRoleInterface = false;
        settingInfo.SetActive(false);
    }
    private void OnDisable()
    {
        bag.SetActive(false);
        roleInterface.SetActive(false);
    }

    public void OnPokemonUIButton()
    {
        pokemonUIButton.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
        pokemonUIButton.transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
        bagUIButton.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
        bagUIButton.transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(false);

        roleInterface.SetActive(!roleInterface.activeSelf);

        bag.SetActive(false);
        // settingInterface.SetActive(false);
        settingInfo.SetActive(false);
    }
    public void OnBagUIButton()
    {
        bagUIButton.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(false);
        bagUIButton.transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(true);
        pokemonUIButton.transform.GetChild(0).GetComponent<Image>().gameObject.SetActive(true);
        pokemonUIButton.transform.GetChild(1).GetComponent<Image>().gameObject.SetActive(false);

        bag.SetActive(!bag.activeSelf);

        roleInterface.SetActive(false);
        // settingInterface.SetActive(false);
        settingInfo.SetActive(false);
    }
    public void OnSettingUIButton()
    {
        // settingInterface.SetActive(settingInterface.activeSelf);

        roleInterface.SetActive(false);
        bag.SetActive(false);
        settingInfo.SetActive(false);
    }

    public void OnSaveData()
    {
        if (TransitionManager.Instance.localSceneType == TeleportType.BattleScene)
        {
            DialogueUI.Instance.DIYDialog("请返回城镇进行记录。");
        }
        else
        {
            SavaLoadManager.Instance.Save(index);
            DialogueUI.Instance.DIYDialog("记录保存成功！");
        }
    }
    public void OnBackStartMenu()
    {
        EventHandler.CallLoadStartMenuSceneEvent("01.StartMenuUI");
    }

    
    private void OnBackTown()
    {
        if(TransitionManager.Instance.localSceneType == TeleportType.BattleScene)
            EventHandler.CallTransitionEvent("T01.LittlerootTown", new Vector3(), TeleportType.TownScene);
    }


    public void Init_GameUI()
    {
        // 宝可梦
        roleInterface.GetComponent<PokemonDataUI>().Init();

        // 背包
        InventoryManager.Instance.Init();

        // 任务
        
    }

}
