using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MyPokemon.Inventory;
using MGame.Save;

public class QuestManager : Singleton<QuestManager>, ISaveable
{
    public QuestDataBase_SO questDataBase;
    public InformationData_SO informationData;
    public List<QuestTask> tasks = new List<QuestTask>();

    private List<string> tasksID;

    public string GUID => GetComponent<DataGUID>().guid;

    private void OnEnable() {
        EventHandler.StartGameEvent += OnStartGameEvent;
        EventHandler.StartNewGameEvent += OnStartNewGameEvent;
    }

    private void OnDisable() {
        EventHandler.StartGameEvent -= OnStartGameEvent;
        EventHandler.StartNewGameEvent -= OnStartNewGameEvent;
    }

    private void Start() {
        ISaveable saveable = this;
        saveable.RegisterSaveable();
    }

    private void OnStartGameEvent(int index)
    {
        for(int i = 0; i < questDataBase.tasks.Count; i++) {
            questDataBase.tasks[i].QuestSO_Init();
        }
    }

    private void OnStartNewGameEvent(int index)
    {
        tasks = new List<QuestTask>();
        informationData.informations = new List<Information>();
    }

    //* 更新任务
    public void UpdateQuestProgress(QuestTask questTask)
    {
        foreach (var require in questTask.questData.questRequires)
        {
            switch (require.requireType)
            {
                case QuestRequireType.收集道具:
                    InventoryManager.Instance.CheckQuestItemInBag(require.name);
                    break;
                case QuestRequireType.击败敌人:
                    break;
                case QuestRequireType.到达特定等级:
                    if (require.name == "队伍最高")
                        QuestManager.Instance.UpdateTeamMaxLevelProgress();
                    else    //? 根据名字找到这只精灵 并且更新任务进度
                    {
                        var pokemon =  PokemonManager.Instance.GetPokemonTeamByName(require.name);
                        if (pokemon != null)
                            QuestManager.Instance.UpdatePokemonLevelProgress(pokemon, pokemon.level);
                    }
                    break;
                default:

                    break;
            }
        }

    }

    //* 更新任务进度（物品、敌人）
    public void UpdateItemAndEnemyProgress(string requireName, int amout)
    {
        foreach (var task in tasks)
        {
            var matchTask = task.questData.questRequires.Find(r => r.name == requireName);
            if(matchTask != null)
                matchTask.currentAmout += amout;
            task.questData.CheckQuestProgress();
        }
    }
    //* 宝可梦等级任务
    public void UpdatePokemonLevelProgress(PokemonAttribute pokemon, int level)
    {
        for(int i = 0; i < tasks.Count; i++) 
        {
            var matchTask = tasks[i].questData.questRequires.Find(r => r.name == pokemon.pokmeonName);
            
            if(matchTask != null)
                matchTask.currentAmout = level;
            tasks[i].questData.CheckQuestProgress();
        }
    }
    //* 队伍中最高等级
    public void UpdateTeamMaxLevelProgress()
    {
        foreach (var task in tasks)
        {
            var matchTask = task.questData.questRequires.Find(r => r.name == "队伍最高");
            if(matchTask != null)
                matchTask.currentAmout += PokemonManager.Instance.GetTeamMaxLevel();
            task.questData.CheckQuestProgress();
        }
    }
    //* 更新任务进度（NPC对话、到达某个地点）


    public bool IsContainsQuest(QuestData_SO data)
    {
        return tasks.Any(q => q.questData.questName == data.questName);
    }

    public QuestTask GetTask(QuestData_SO data)
    {
        return tasks.Find(q => q.questData.questName == data.questName);
    }

    public GameSaveData GenerateSaveData()
    {
        GameSaveData saveData = new GameSaveData();

        // 存储所有任务进度
        saveData.questTaskDict = new Dictionary<string, List<TaskProgress>>();
        List<TaskProgress> taskProgresses = new List<TaskProgress>();
        foreach (QuestTask task in tasks)
        {
            taskProgresses.Add(new TaskProgress(task.questData));
        }
        saveData.questTaskDict.Add("TasksProgresses", taskProgresses);

        // 存储所有小道消息
        saveData.informationDataDict = new Dictionary<string, List<Information>>();
        saveData.informationDataDict.Add("InformationData", informationData.informations);

        return saveData;
    }

    public void RestoreLoadData(GameSaveData saveData)
    {
        List<TaskProgress> taskProgresses = saveData.questTaskDict["TasksProgresses"];
        tasks = questDataBase.GetQuestTasks(taskProgresses);

        informationData.informations = saveData.informationDataDict["InformationData"];
    }
}
