using System.Collections.Generic;
using UnityEngine;

public class EquippedSkillsUI : MonoBehaviour
{
    public List<RectTransform> slots;
    private int childCount;

    public void DestroyAllChild()
    {
        childCount = transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void LoadChildSlot()
    {
        slots = new List<RectTransform>();
        if (childCount == 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                slots.Add(transform.GetChild(i).GetComponent<RectTransform>());
            }
        }
        else
        {
            for (int i = (int)transform.childCount / 2; i < transform.childCount; i++)
            {
                slots.Add(transform.GetChild(i).GetComponent<RectTransform>());
            }
        }

    }

}
