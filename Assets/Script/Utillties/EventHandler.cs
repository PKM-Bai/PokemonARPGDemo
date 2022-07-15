using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventHandler
{

    //* 鼠标在按钮上触发的事件
    public static event Action<Sprite> MouseOnButtonEvent;
    public static void CallMouseOnButtonEvent(Sprite cursorSprite)
    {
        MouseOnButtonEvent?.Invoke(cursorSprite);
    }

    //* 游戏分钟变化的事件
    public static event Action<int, int> GameMinuteEvent;
    public static void CallGameMinuteEvent(int minute, int hour)
    {
        GameMinuteEvent?.Invoke(minute, hour);
    }

    //* 游戏日期变化的事件
    public static event Action<int, int, int, int, Season> GameDateEvent;
    public static void CallGameDateEvent(int hour, int day, int month, int year, Season season)
    {
        GameDateEvent?.Invoke(hour, day, month, year, season);
    }

    //* 场景切换灯光的事件
    public static event Action<Season, LightShift, float> LightShiftChangeSceneEvent;
    public static void CallLightShiftChangeSceneEvent(Season season, LightShift lightShift, float timeDifference)
    {
        LightShiftChangeSceneEvent?.Invoke(season, lightShift, timeDifference);
    }

    //* 时间切换灯光的事件
    public static event Action<Season, LightShift, float> LightShiftChangeEvent;
    public static void CallLightShiftChangeEvent(Season season, LightShift lightShift, float timeDifference)
    {
        LightShiftChangeEvent?.Invoke(season, lightShift, timeDifference);
    }

    //* 粒子效果
    public static event Action<ParticleEffectType, Vector3> ParticleEffectEvent;
    public static void CallParticleEffectEvent(ParticleEffectType effectType, Vector3 pos)
    {
        ParticleEffectEvent?.Invoke(effectType, pos);
    }
    //* 音效
    public static event Action<SoundDetails> InitSoundEffect;
    public static void CallInitSoundEffect(SoundDetails soundDetails)
    {
        InitSoundEffect?.Invoke(soundDetails);
    }

    //* 传送事件
    public static event Action<string, Vector3, TeleportType> TransitionEvent;
    public static void CallTransitionEvent(string sceneName, Vector3 position, TeleportType targetScene)
    {
        TransitionEvent?.Invoke(sceneName, position, targetScene);
    }

    //* 加载游戏开始菜单
    public static event Action<string> LoadStartMenuSceneEvent;
    public static void CallLoadStartMenuSceneEvent(string sceneName)
    {
        LoadStartMenuSceneEvent?.Invoke(sceneName);
    }

    //* 开始卸载场景的事件
    public static event Action BeforeSceneUnloadEvent;
    public static void CallBeforeSceneUnloadEvent()
    {
        BeforeSceneUnloadEvent?.Invoke();
    }

    //* 场景加载完毕
    public static event Action FinishSceneLoadedEvent;
    public static void CallFinishSceneLoadedEvent()
    {
        FinishSceneLoadedEvent?.Invoke();
    }

    //* 加载场景之后的事件
    public static event Action AfterSceneLoadedEvent;
    public static void CallAfterSceneLoadedEvent()
    {
        AfterSceneLoadedEvent?.Invoke();
    }

    //* 移动坐标
    public static event Action<Vector3> MoveToPosition;
    public static void CallMoveToPosition(Vector3 pos)
    {
        MoveToPosition?.Invoke(pos);
    }

    //* 背包-切换类别标签
    public static event Action<TabTypeEnum> SwitchInventoryTab;
    public static void CallSwitchInventoryTab(TabTypeEnum tabType)
    {
        SwitchInventoryTab?.Invoke(tabType);
    }

    //* 角色升级
    public static event Action<PokemonAttribute, int> GetExp;
    public static void CallGetExp(PokemonAttribute pokemon, int exp)
    {
        GetExp?.Invoke(pokemon, exp);
    }

    //* 努力值增加
    public static event Action<PokemonAttribute> AddBasePoints;
    public static void CallAddBasePoints(PokemonAttribute pokemon)
    {
        AddBasePoints?.Invoke(pokemon);
    }

    //* 攻击敌人
    public static event Action<PokemonAttribute> AttackEnemy;
    public static void CallAttackEnemy(PokemonAttribute pokemon)
    {
        AttackEnemy?.Invoke(pokemon);
    }

    //* 在宝可梦界面选择宝可梦触发
    public static event Action<PokemonAttribute> SelectPokemonRole;
    public static void CallSelectPokemonRole(PokemonAttribute pokemon)
    {
        SelectPokemonRole?.Invoke(pokemon);
    }

    //* 角色死亡 => 游戏结束
    public static event Action GameOverEvent;
    public static void CallGameOverEvent()
    {
        GameOverEvent?.Invoke();
    }

    //* 在游戏主菜单
    public static event Action InGameMainMenuEvent;
    public static void CallInGameMainMenuEvent()
    {
        InGameMainMenuEvent?.Invoke();
    }

    //* 开始游戏
    public static event Action<int> StartGameEvent;
    public static void CallStartGameEvent(int index)
    {
        StartGameEvent?.Invoke(index);
    }
    //* 开始新游戏
    public static event Action<int> StartNewGameEvent;
    public static void CallStartNewGameEvent(int index)
    {
        StartNewGameEvent?.Invoke(index);
    }
}
