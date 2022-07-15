using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskControl : MonoBehaviour
{
    public bool isInteract;

    void Update()
    {
        if(isInteract)
        {
            if(Input.GetButtonDown("Interactive") && !TaskUI.Instance.taskPanel.gameObject.activeSelf)
            {
                TaskUI.Instance.taskPanel.gameObject.SetActive(true);
            }
        }
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            isInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            isInteract = false;
        }
    }

}
