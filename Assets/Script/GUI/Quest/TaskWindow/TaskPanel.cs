using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskPanel : MonoBehaviour
{
    public TaskUI taskUI;

    private void Awake()
    {
        taskUI = GetComponentInParent<TaskUI>();
    }

    private void OnEnable()
    {
        taskUI.SetupTaskList();
    }
}
