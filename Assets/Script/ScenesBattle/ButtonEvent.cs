using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvent : MonoBehaviour
{

    public GameObject Button;   // 按钮提示

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            Button.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Button.SetActive(false);
        }
            
    }

    private void Update()
    {
        if (Button.activeSelf && Input.GetKeyDown(KeyCode.E))
        {

        }
    }
}
