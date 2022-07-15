using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionUI : MonoBehaviour
{
    public TextMeshProUGUI optionText;
    private Button thisButton;
    public DialoguePiece currentPiece;

    private bool takeQuest;
    private bool isTreatment;
    private string nextPieceID;

    private void Awake()
    {
        thisButton = GetComponent<Button>();
        thisButton.onClick.AddListener(OnOptionClicked);
    }

    public void UpdateOption(DialoguePiece piece, DialogueOption option)
    {
        currentPiece = piece;
        
        optionText.text = option.text;
        optionText.margin = new Vector4(1, 0, 1, 0);
        // optionText.padding = new Vector4(1, 0, 1, 0);

        nextPieceID = option.targetID;
        takeQuest = option.takeQuest;
        isTreatment = option.isTreatment;
    }

    public void OnOptionClicked()
    {
        // 选择中是否存在任务
        if (currentPiece.quest != null)
        {
            var newTask = new QuestTask { questData = Instantiate(currentPiece.quest) };
            if (takeQuest)
            {
                //* 判断当前这条任务是否已经在管理器中
                if (!QuestManager.Instance.IsContainsQuest(currentPiece.quest))
                {
                    //? 添加到任务列表中
                    QuestManager.Instance.tasks.Add(newTask);
                    QuestManager.Instance.GetTask(newTask.questData).IsStarted = true; 

                    QuestManager.Instance.UpdateQuestProgress(newTask);
                }
            }
        }
        // 选项是否提供治疗
        if (isTreatment)
        {
            // 队伍中所有宝可梦HP恢复到最大值
            foreach (PokemonAttribute pokemon in PokemonManager.Instance.pokemonTeam.pokemons)
            {
                pokemon.currentHP = pokemon.Stat.HP;
            }
            // 播放音效
            AudioManager.Instance.PlayEffectSound(SoundName.PokemonCenter);
            DialogueUI.Instance.timer = 2.5f;
        }

        if (nextPieceID == "" || currentPiece.isEnd)
        {
            DialogueUI.Instance.dialoguePanel.SetActive(false);
        }
        else
        {
            DialogueUI.Instance.UpdateDialogueUI(DialogueUI.Instance.currentData.dialogueIndex[nextPieceID]);
        }
    }

}

