using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUI : MonoBehaviour
{
    private void OnEnable() {
        EventHandler.CallInGameMainMenuEvent();
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("退出游戏");
    }
}
