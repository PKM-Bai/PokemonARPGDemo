using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueController : MonoBehaviour
{
    public DialogueData_SO currentData;
    bool canTalk = false;
    public float m_time;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentData != null)
        {
            canTalk = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canTalk = false;
        }
    }

    void Update()
    {
        if (canTalk && Input.GetButtonDown("Interactive") && DialogueUI.Instance.intervalPress)
        {
            if (!DialogueUI.Instance.dialoguePanel.activeSelf)
                OpenDialogue();
        }
        
    }



    void OpenDialogue()
    {
        //* 传输对话内容信息
        DialogueUI.Instance.UpdateDialogueData(currentData);
        //* 打开对话框的UI
        DialogueUI.Instance.UpdateDialogueUI(currentData.dialoguePieces[0]);
        //* 对话框持续对话的时间
        DialogueUI.Instance.m_time = m_time;
    }

}
