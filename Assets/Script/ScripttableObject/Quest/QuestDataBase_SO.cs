using System.Linq;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestDataBase_SO", menuName = "Quest/QuestDataBase")]
public class QuestDataBase_SO : ScriptableObject {
    public List<QuestData_SO> tasks = new List<QuestData_SO>();

    List<string> TasksID = new List<string>();
    List<QuestTask> questTasks = new List<QuestTask>();

    /// <summary>
    /// 根据任务ID查找并返回任务
    /// </summary>
    /// <param name="id">任务编号</param>
    /// <returns></returns>
    public QuestData_SO FindQuestDataByID(string id)
    {
        return tasks.Find(t => t.questId == id);
    }

    /// <summary>
    /// 获取任务列表中所有任务ID
    /// </summary>
    /// <param name="tasks">任务列表</param>
    /// <returns></returns>
    public List<string> GetTasksID(List<QuestTask> tasks)
    {
        if(TasksID.Count != 0) TasksID.Clear();
        
        foreach (var task in tasks)
        {
            TasksID.Add(task.questData.questId);
        }

        return TasksID;
    }
    
    /// <summary>
    /// 使用任务ID获取任务列表
    /// </summary>
    /// <param name="taskIDs">任务ID</param>
    /// <returns></returns>
    public List<QuestTask> GetQuestTasks(List<string> taskIDs)
    {
        if(questTasks.Count != 0) questTasks.Clear();

        foreach (var taskID in taskIDs)
        {
            QuestTask task = new QuestTask();
            task.questData = FindQuestDataByID(taskID);

            questTasks.Add(task);
        }

        return questTasks;
    }

    /// <summary>
    /// 使用任务ID获取任务列表
    /// </summary>
    /// <param name="taskProgresses">任务进度列表</param>
    /// <returns></returns>
    public List<QuestTask> GetQuestTasks(List<TaskProgress> taskProgresses)
    {
        if(questTasks.Count != 0) questTasks.Clear();

        foreach (var taskProgress in taskProgresses)
        {
            QuestTask task = new QuestTask();
            task.questData = FindQuestDataByID(taskProgress.taskID);
            
            task.questData.isStarted = taskProgress.isStarted;
            task.questData.isProgressed = taskProgress.isProgressed;
            task.questData.isComplete = taskProgress.isComplete;
            task.questData.isFinished = taskProgress.isFinished;

            // 根据ID找到的任务列表，再次遍历修改任务中的目标进度
            for (int i = 0; i < taskProgress.requiresName.Count; i++)
            {
                if(task.questData.questRequires.Find(r => r.name == taskProgress.requiresName[i]) != null)
                {
                    task.questData.questRequires.Find(r => r.name == taskProgress.requiresName[i]).currentAmout = taskProgress.requiresAmout[i];
                }
            }
            
            questTasks.Add(task);
        }

        return questTasks;
    }

    

}
