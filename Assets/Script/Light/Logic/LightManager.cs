using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightManager : MonoBehaviour
{
    private LightControl[] sceneLights;
    private LightShift currentLightShift;
    private Season currentSesson;
    private float timeDifference;

    private void OnEnable()
    {
        EventHandler.LightShiftChangeSceneEvent += OnLightShiftChangeSceneEvent;
        EventHandler.LightShiftChangeEvent += OnLightShiftChangeEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.LightShiftChangeSceneEvent -= OnLightShiftChangeSceneEvent;
        EventHandler.LightShiftChangeEvent -= OnLightShiftChangeEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void OnStartNewGameEvent(int index)
    {
        currentLightShift = LightShift.Morning;
    }

    /// <summary>
    /// 加载场景之后切换灯光
    /// </summary>
    private void OnLightShiftChangeSceneEvent(Season season, LightShift lightShift, float timeDifference)
    {
        sceneLights = FindObjectsOfType<LightControl>();
        if (sceneLights != null)
        {
            foreach (LightControl light in sceneLights)
            {
                light.ChangeLightShift(season, lightShift, timeDifference);
            }
        }
    }


    /// <summary>
    /// 日昼灯光切换
    /// </summary>
    /// <param name="season"></param>
    /// <param name="lightShift"></param>
    /// <param name="timeDifference"></param>
    private void OnLightShiftChangeEvent(Season season, LightShift lightShift, float timeDifference)
    {
        currentSesson = season;
        sceneLights = FindObjectsOfType<LightControl>();
        this.timeDifference = timeDifference;
        if (currentLightShift != lightShift)
        {
            currentLightShift = lightShift;
            if (sceneLights != null)
            {
                foreach (LightControl light in sceneLights)
                {
                    light.ChangeLightShift(currentSesson, currentLightShift, timeDifference);
                }
            }
        }
    }


}
