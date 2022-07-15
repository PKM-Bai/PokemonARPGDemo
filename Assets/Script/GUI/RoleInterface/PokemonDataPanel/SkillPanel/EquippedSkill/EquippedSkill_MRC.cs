using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EquippedSkill_MRC : MonoBehaviour
{
    private RoleInterface roleInterface;

    public Button removeButton;
    public Button caneclButton;

    private RectTransform rectTransform;

    private void Awake()
    {
        roleInterface = FindObjectOfType<RoleInterface>();
        rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        caneclButton.onClick.AddListener(delegate
        {
            gameObject.SetActive(false);
        });
    }

    public void ShowEquippedSkillMenu(EquippedSkill_Slot equippedSkill_Slot, Skill_SO skill)
    {
        UpdatePosition();

        removeButton.onClick.RemoveAllListeners();
        removeButton.onClick.AddListener(delegate
        {
            Destroy(equippedSkill_Slot.transform.parent.gameObject);
            roleInterface.curSelectPokemon.equippedSkills.skillDatabase.Remove(skill);
            gameObject.SetActive(false);
        });

    }

    public void UpdatePosition()
    {
        Vector3 mousePos = Input.mousePosition;

        Vector3[] corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);

        float width = corners[3].x - corners[0].x;
        float height = corners[1].y - corners[0].y;

        rectTransform.position = mousePos + Vector3.down * height * 0.5f;

        if (mousePos.y < height)
            rectTransform.position = mousePos + Vector3.up * height * 0.6f;
        else if (Screen.width - mousePos.x > width * 0.5f)
            rectTransform.position = mousePos + Vector3.right * width * 0.6f;

        else
            rectTransform.position = mousePos + Vector3.left * width * 0.6f;
    }

}
