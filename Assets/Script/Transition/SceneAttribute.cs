using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using MyPokemon.Transition;
public class SceneAttribute : MonoBehaviour
{
    public TeleportType localSceneType;
    TransitionManager transitionManager;

    private void Awake() {
        transitionManager = FindObjectOfType<TransitionManager>();

        if (transitionManager.localSceneType == TeleportType.BattleScene)
        {
            TimeManager.Instance.gameClockPause = true;
            
        }
        if (transitionManager.localSceneType == TeleportType.TownScene)
        {
            TimeManager.Instance.gameClockPause = false;
        }

    }

}
