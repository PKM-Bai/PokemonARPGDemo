using System;
using System.Collections.Generic;
using MyPokemon.Transition;
using UnityEngine;
using MGame.Save;

public class TimeManager : Singleton<TimeManager>, ISaveable
{
    private int gameSecode, gameMinute, gameHour, gameDay, gameMonth, gameYear;

    private Season gameSeason = Season.春天;

    private int monthInSeason = 3;  // 季节更换的月份数

    public bool gameClockPause;
    private float tikTime;

    // 灯光时间差
    private float timeDifference;

    public TimeSpan GameTime => new TimeSpan(gameHour, gameMinute, gameSecode);

    public string GUID => GetComponent<DataGUID>().guid;

    private void OnEnable()
    {
        EventHandler.BeforeSceneUnloadEvent += OnBeforeSceneUnloadItemEvent;
        EventHandler.FinishSceneLoadedEvent += OnFinishSceneLoadedTimeEvent;
        EventHandler.StartNewGameEvent += OnStartGameEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable()
    {
        EventHandler.BeforeSceneUnloadEvent -= OnBeforeSceneUnloadItemEvent;
        EventHandler.FinishSceneLoadedEvent -= OnFinishSceneLoadedTimeEvent;
        EventHandler.StartNewGameEvent -= OnStartGameEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }


    private void Start()
    {
        gameClockPause = true;

        ISaveable saveable = this;
        saveable.RegisterSaveable();
    }

    private void OnBeforeSceneUnloadItemEvent()
    {
        gameClockPause = true;
    }

    private void OnFinishSceneLoadedTimeEvent()
    {
        //? 切换到城镇场景时，开始时间并呼叫时间绑定事件
        if (FindObjectOfType<TransitionManager>().localSceneType == TeleportType.TownScene)
        {
            ShiftTownScene();
            
        }
    }

    private void OnStartGameEvent(int index)
    {
        ShiftTownScene();
    }

    private void OnStartNewGameEvent(int index)
    {
        NewGameTime();
    }

    public void ShiftTownScene()
    {
        gameClockPause = false;
        EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
        EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
        // 切换灯光
        EventHandler.CallLightShiftChangeSceneEvent(gameSeason, GetCurrentLightShift(), timeDifference);
    }

    private void Update()
    {
        if (!gameClockPause)
        {
            tikTime += Time.deltaTime;

            if (tikTime >= Settings.secondThreshold)
            {
                tikTime -= Settings.secondThreshold;
                UpdateGameTime();
            }
        }

        if (Input.GetKey(KeyCode.T))
        {
            for (int i = 0; i < 60; i++)
            {
                UpdateGameTime();
            }
        }
    }

    //* 初始化参数
    private void NewGameTime()
    {
        gameSecode = 0;
        gameMinute = 10;
        gameHour = 8;
        gameDay = 1;
        gameMonth = 1;
        gameYear = 1102;
        gameSeason = Season.春天;
    }

    //* 更新游戏时间
    private void UpdateGameTime()
    {
        gameSecode++;
        if (gameSecode > Settings.secondHold)
        {
            gameSecode = 0;
            gameMinute++;

            if (gameMinute > Settings.minuteHold)
            {
                gameMinute = 0;
                gameHour++;

                if (gameHour > Settings.hourHold)
                {
                    gameHour = 0;
                    gameDay++;

                    if (gameDay > Settings.dayHold)
                    {
                        gameDay = 1;
                        gameMonth++;

                        if (gameMonth > 12)
                            gameMonth = 1;

                        monthInSeason--;
                        if (monthInSeason == 0) //? 当季节度完一轮代表过了一年
                        {
                            monthInSeason = 3;
                            int seasonNumber = (int)gameSeason;
                            seasonNumber++;
                            if (seasonNumber > Settings.seasonHold)
                            {
                                seasonNumber = 0;
                                gameYear++;
                            }

                            gameSeason = (Season)seasonNumber;
                        }
                    }
                }
                EventHandler.CallGameDateEvent(gameHour, gameDay, gameMonth, gameYear, gameSeason);
            }
            EventHandler.CallGameMinuteEvent(gameMinute, gameHour);
            EventHandler.CallLightShiftChangeEvent(gameSeason, GetCurrentLightShift(), timeDifference);
        }

        // Debug.Log("Second:" + gameSecode + "   Minute:" + gameMinute);
    }


    private LightShift GetCurrentLightShift()
    {
        if (GameTime >= Settings.morningTime && GameTime < Settings.nightTime)
        {
            timeDifference = Mathf.Abs((float)(GameTime - Settings.morningTime).TotalMinutes);
            return LightShift.Morning;
        }
        if (GameTime < Settings.morningTime || GameTime >= Settings.nightTime)
        {
            timeDifference = Mathf.Abs((float)(GameTime - Settings.nightTime).TotalMinutes);
            return LightShift.Night;
        }
        return LightShift.Morning;
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();
        saveData.timeDict = new Dictionary<string, int>();
        saveData.timeDict.Add("gameSeason", (int)gameSeason);
        saveData.timeDict.Add("gameYear", gameYear);
        saveData.timeDict.Add("gameMonth", gameMonth);
        saveData.timeDict.Add("gameDay", gameDay);
        saveData.timeDict.Add("gameHour", gameHour);
        saveData.timeDict.Add("gameMinute", gameMinute);
        saveData.timeDict.Add("gameSecode", gameSecode);
        return saveData;
    }

    public void RestoreLoadData(GameSaveData saveData)
    {
        gameSeason = (Season)saveData.timeDict["gameSeason"];
        gameYear = saveData.timeDict["gameYear"];
        gameMonth = saveData.timeDict["gameMonth"];
        gameDay = saveData.timeDict["gameDay"];
        gameHour = saveData.timeDict["gameHour"];
        gameMinute = saveData.timeDict["gameMinute"];
        gameSecode = saveData.timeDict["gameSecode"];
    }
}
